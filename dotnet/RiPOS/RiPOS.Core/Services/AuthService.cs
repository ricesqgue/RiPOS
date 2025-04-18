using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RiPOS.Core.Interfaces;
using RiPOS.Core.Utilities;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models.Settings;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Repositories;
using RiPOS.Shared.Enums;

namespace RiPOS.Core.Services
{
    public class AuthService(IUserRepository userRepository, IAuthRepository authRepository,
        IMemoryCacheService memoryCacheService, IMapper mapper, IConfiguration configuration)
        : IAuthService
    {
        public async Task<MessageResponse<UserWithStoresResponse>> AuthenticateAsync(AuthRequest request)
        {
            var response = new MessageResponse<UserWithStoresResponse>();

            var user = await userRepository
                .FindAsync(u => u.Username.ToUpper() == request.Username.ToUpper(), 
                    includeProps: u => u.Include(x => x.UserStoreRoles)!.ThenInclude(ur => ur.Store)!);

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

            var userResponse = mapper.Map<UserWithStoresResponse>(user);
            response.Success = true;
            response.Data = userResponse;
            return response;
        }

        public async Task<TokenResponse> BuildAndStoreTokensAsync(UserWithStoresResponse user)
        {
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            
            var accessTokenExpiresDateTime = DateTime.UtcNow.AddMinutes(jwtSettings!.AccessTokenExpirationMinutes);
            var refreshTokenExpiresDateTime = DateTime.UtcNow.AddHours(jwtSettings.RefreshTokenExpirationHours);
            
            var accessToken = GenerateAccessToken(user, accessTokenExpiresDateTime, jwtSettings);
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
                Expires = accessTokenExpiresDateTime,
                AvailableStores = user.Stores
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

            var user = await userRepository.FindAsync(u => u.Id == int.Parse(userId), 
                includeProps: u => u.Include(x => x.UserStoreRoles)!.ThenInclude(ur => ur.Store)!);

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
            
            var userResponse = mapper.Map<UserWithStoresResponse>(user);
            
            var newTokens = await BuildAndStoreTokensAsync(userResponse);

            return new MessageResponse<TokenResponse>()
            {
                Success = true,
                Data = newTokens,
            };
        }
        
        public async Task<UserResponse> GetUserFromClaims(IEnumerable<Claim> userClaims, int storeId)
        {
            var userClaimsList = userClaims.ToList();
            var userResponse = new UserResponse
            {   
                Id = int.Parse(userClaimsList.First(c => c.Type == "UserId").Value),
                Name = userClaimsList.First(c => c.Type == "Name").Value,
                Surname = userClaimsList.First(c => c.Type == "Surname").Value,
                SecondSurname = userClaimsList.First(c => c.Type == "SecondSurname").Value,
                Username = userClaimsList.First(c => c.Type == "Username").Value,
            };

            var userRoles = memoryCacheService.GetUserStoreRoles(userResponse.Id, storeId);

            if (!userRoles.Any())
            {
                var roles = await userRepository.GetStoreRolesAsync(userResponse.Id, storeId);
                userRoles = roles.Select(r => (RoleEnum)r.Id).ToList();
                
                if (userRoles.Any())
                {
                    memoryCacheService.SetUserStoreRoles(userResponse.Id, storeId, userRoles);
                }
            }

            userResponse.Roles = mapper.Map<ICollection<RoleResponse>>(userRoles);
            
            return userResponse;

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

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
