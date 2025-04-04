using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Moq;
using RiPOS.Core.Services;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Repository.Repositories;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models.Settings;

namespace RiPOS.Core.Tests.Services;

public class AuthServiceTests
{
    private readonly AuthService _authService;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    
    public AuthServiceTests()
    {
        var jwtSettings = new JwtSettings
        {
            Issuer = "testIssuer",
            Audience = "testAudience",
            Key = "testKey123!@#123!@#",
            AccessTokenExpirationMinutes = 5,
            RefreshTokenExpirationHours = 24
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                {"JwtSettings:Issuer", jwtSettings.Issuer},
                {"JwtSettings:Audience", jwtSettings.Audience},
                {"JwtSettings:Key", jwtSettings.Key},
                {"JwtSettings:AccessTokenExpirationMinutes", jwtSettings.AccessTokenExpirationMinutes.ToString()},
                {"JwtSettings:RefreshTokenExpirationHours", jwtSettings.RefreshTokenExpirationHours.ToString()}
            }!)
            .Build();
        
        Mock<IAuthRepository> loginRepositoryMock = new Mock<IAuthRepository>();
        Mock<IMemoryCache> memoryCacheMock = new Mock<IMemoryCache>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();
        _authService = new AuthService(
            _userRepositoryMock.Object,
            loginRepositoryMock.Object,
            _mapperMock.Object,
            configuration,
            memoryCacheMock.Object
        );
    }

    [Fact]
    public async Task AuthenticateAsync_ReturnsSuccess_WhenCredentialsAreValid()
    {
        var request = new AuthRequest { Username = "validUser", Password = "password" };
        var user = new User { Name = "User1", Surname = "User1", Username = "validUser", PasswordHash = "dh3TVAEOnBb9wUqqiz+Izb3SSUSKVemPrPTOUEMQ0xg6lDdY", IsActive = true };
        var userResponse = new UserWithStoresResponse { Name = "User1",  Surname = "User1", Username = "validUser" };

        _userRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Func<IQueryable<User>,IIncludableQueryable<User, object>>>()))
            .ReturnsAsync(user);
        _mapperMock.Setup(mapper => mapper.Map<UserResponse>(user))
            .Returns(userResponse);

        var result = await _authService.AuthenticateAsync(request);

        Assert.True(result.Success);
        Assert.Equal(userResponse, result.Data);
    }

    [Fact]
    public async Task AuthenticateAsync_ReturnsError_WhenUserDoesNotExist()
    {
        var request = new AuthRequest { Username = "invalidUser", Password = "password" };

        _userRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<User, bool>>>(), null))
            .ReturnsAsync((User?)null);

        var result = await _authService.AuthenticateAsync(request);

        Assert.False(result.Success);
        Assert.Equal("Nombre de usuario y/o contraseña incorrectos", result.Message);
    }

    [Fact]
    public async Task AuthenticateAsync_ReturnsError_WhenUserIsInactive()
    {
        var request = new AuthRequest { Username = "inactiveUser", Password = "password" };
        var user = new User { Name = "inactiveUser", Surname = "test", Username = "inactiveUser", PasswordHash = "Uc8n+zzkYC8gTW30vI/X9WM6JhQ9yym8/i+dWpwNV+zhNcJT", IsActive = false };

        _userRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Func<IQueryable<User>,IIncludableQueryable<User, object>>>()))
            .ReturnsAsync(user);

        var result = await _authService.AuthenticateAsync(request);

        Assert.False(result.Success);
        Assert.Equal("Usuario inactivo", result.Message);
    }

    [Fact]
    public async Task BuildTokens_ReturnsValidTokenResponse()
    {
        var user = new UserWithStoresResponse { Username = "validUser", Name = "User1", Surname = "User1" };
        var result = await _authService.BuildAndStoreTokensAsync(user);

        Assert.NotNull(result.AccessToken);
        Assert.NotNull(result.RefreshToken);
        Assert.True(result.Expires > DateTime.Now);
    }
}