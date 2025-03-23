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
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace RiPOS.Core.Services
{
    public class LoginService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration, IMemoryCache memoryCache)
        : ILoginService
    {
        
        /*public async Task<MessageResponse<UserResponse>> AuthenticateAsync(LoginRequest request)
        {
            var response = new MessageResponse<UserResponse>();

            var user = await userRepository
                .FindAsync(u => u.Username.ToUpper() == request.Username.ToUpper()
                    && u.Company.Code == request.CompanyCode);

            if (user == null)
            {
                response.Success = false;
                response.Message = "Nombre de usuario y/o contraseña incorrectos";
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

        public string BuildToken(UserResponse user)
        {
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
             {
                new Claim("Username", user.Username),
                new Claim("Name", user.Name),
                new Claim("LastName", user.Surname),
                new Claim("UserId", user.Id.ToString()),
                new Claim("MobilePhone", user.MobilePhone),
                new Claim("PhoneNumber", user.PhoneNumber),
                new Claim("Email", user.Email),
                new Claim("Company", user.Id.ToString()),
            };

            var token = new JwtSecurityToken(jwtSettings.Issuer,
                jwtSettings.Issuer,
                expires: DateTime.Now.AddMinutes(300),
                signingCredentials: creds,
                claims: claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }*/
        public LoginResponse GenerateToken(UserResponse user)
        {
            var claims = new List<Claim>
            {
                new Claim("Username", user.Username),
                new Claim("Name", user.Name),
                new Claim("LastName", user.Surname),
                new Claim("UserId", user.Id.ToString()),
                new Claim("MobilePhone", user.MobilePhone),
                new Claim("PhoneNumber", user.PhoneNumber),
                new Claim("Email", user.Email),
            };

            foreach (var role in user.Roles)
            {
                claims.Add((new Claim(ClaimTypes.Role, role.Name)));
            }
            
            // cache the claims
            var memoryCacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

            return new LoginResponse();

        }

        public ClaimsIdentity GetPrincipalFromExpiredToken(string accessToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateRefreshTokenAsync(string refreshToken, int userId, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task RevokeRefreshTokenAsync(string refreshToken, int userId, int companyId)
        {
            throw new NotImplementedException();
        }
    }

}
