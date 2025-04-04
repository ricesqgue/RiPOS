using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RiPOS.Core.Interfaces;
using RiPOS.Core.Utilities;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using RiPOS.Domain.Entities;

namespace RiPOS.Core.Services
{
    public class AuthService(IUserRepository userRepository, IAuthRepository authRepository, IMapper mapper, IConfiguration configuration, IMemoryCache memoryCache)
        : IAuthService
    {
        public async Task<MessageResponse<UserResponse>> AuthenticateAsync(AuthRequest request)
        {
            var response = new MessageResponse<UserResponse>();

            var user = await userRepository
                .FindAsync(u => u.Username.ToUpper() == request.Username.ToUpper());

            if (user == null)
            {
                response.Success = false;
                response.Message = "Nombre de usuario y/o contraseña incorrectos";
                return response;
            }

            if (!user.IsActive)
            {
                response.Success = false;
                response.Message = "Usuario inactivo";
                return response;
            }

            var passwordHash = user.PasswordHash;

            byte[] hashBytes = Convert.FromBase64String(passwordHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var loginPasswordHashBytes = PasswordHelper.GenerateHash(request.Password, salt);

            if (!PasswordHelper.IsCorrectPassword(hashBytes, loginPasswordHashBytes))
            {
                response.Success = false;
                response.Message = "Nombre de usuario y/o contraseña incorrectos";
                return response;
            }

            var userResponse = mapper.Map<UserResponse>(user);
            response.Success = true;
            response.Data = userResponse;
            return response;
        }

        public async Task<TokenResponse> BuildAndStoreTokensAsync(UserResponse user)
        {
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            
            var accessTokenExpiresDateTime = DateTime.Now.AddMinutes(jwtSettings!.AccessTokenExpirationMinutes);
            var refreshTokenExpiresDateTime = DateTime.Now.AddHours(jwtSettings.RefreshTokenExpirationHours);
            
            var accessToken = GenerateAccessToken(user, accessTokenExpiresDateTime, jwtSettings!);
            var refreshToken = GenerateRefreshToken();
            

            await authRepository.AddRefreshTokenAsync(new RefreshToken {
                Token = refreshToken,
                Expires = refreshTokenExpiresDateTime,
                UserId = user.Id
            });

            try
            {
                await authRepository.DeleteUserExpiredTokensAsync(user.Id);
            }
            catch
            {
                // ignored
            }

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expires = accessTokenExpiresDateTime
            };
        }

        public async Task<MessageResponse<TokenResponse>> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            
            ClaimsPrincipal? principal = GetPrincipalFromExpiredToken(accessToken);

            if (principal == null)
            {
                return new MessageResponse<TokenResponse>()
                {
                    Success = false,
                    Message = "Token inválido"
                };
            }
            
            var userId = principal.FindFirst(c => c.Type == "UserId")?.Value;
            
            if (userId == null)
            {
                return new MessageResponse<TokenResponse>()
                {
                    Success = false,
                    Message = "Claims inválidos"
                };
            }

            var user = await userRepository.GetByIdAsync(int.Parse(userId));

            if (user == null)
            {
                return new MessageResponse<TokenResponse>()
                {
                    Success = false,
                    Message = "Usuario inválido"
                };
            }
            
            var refreshTokenDb = await authRepository.GetRefreshTokenAsync(refreshToken);
            
            if (refreshTokenDb == null || refreshTokenDb.UserId.ToString() != userId || refreshTokenDb.Expires < DateTime.Now)
            {
                return new MessageResponse<TokenResponse>()
                {
                    Success = false,
                    Message = "Refresh token inválido"
                };
            }
            
            var userResponse = mapper.Map<UserResponse>(user);
            
            var newTokens = await BuildAndStoreTokensAsync(userResponse);

            return new MessageResponse<TokenResponse>()
            {
                Success = true,
                Data = newTokens,
            };
        }
        
        private string GenerateAccessToken(UserResponse user, DateTime expiresDateTime, JwtSettings jwtSettings)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var claims = new[]
            {
                new Claim("Username", user.Username),
                new Claim("Name", user.Name),
                new Claim("Surname", user.Surname),
                new Claim("SecondSurname", user.SecondSurname ?? string.Empty),
                new Claim("UserId", user.Id.ToString()),
                new Claim("MobilePhone", user.MobilePhone ?? string.Empty),
                new Claim("PhoneNumber", user.PhoneNumber ?? string.Empty),
                new Claim("Email", user.Email ?? string.Empty),
            };
            
            var token = new JwtSecurityToken(
                jwtSettings.Issuer,
                jwtSettings.Audience,
                expires: expiresDateTime,
                signingCredentials: credentials,
                claims: claims);
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false, // we are only validating the token's signature
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ClockSkew = TimeSpan.FromMinutes(2),
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings!.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
            };
            
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

                if (!(securityToken is JwtSecurityToken jwtSecurityToken) || 
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, 
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }
                
                // var jwtToken = securityToken;
                // if (jwtToken.ValidTo > DateTime.UtcNow)
                // {
                //     isTokenExpired = false;
                // }

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
