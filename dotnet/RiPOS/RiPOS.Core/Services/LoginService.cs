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

namespace RiPOS.Core.Services
{
    public class LoginService(IUserRepository userRepository, ILoginRepository loginRepository, IMapper mapper, IConfiguration configuration, IMemoryCache memoryCache)
        : ILoginService
    {
        
        public async Task<MessageResponse<UserResponse>> AuthenticateAsync(LoginRequest request)
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

        public TokenResponse BuildTokens(UserResponse user)
        {
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            var accessToken = GenerateAccessToken(user, jwtSettings);
            var refreshToken = GenerateRefreshToken();

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expires = DateTime.Now.AddMinutes(jwtSettings.AccessTokenExpirationMinutes)
            };
        }
        
        private string GenerateAccessToken(UserResponse user, JwtSettings jwtSettings)
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
                expires: DateTime.Now.AddMinutes(jwtSettings.AccessTokenExpirationMinutes),
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
    }

}
