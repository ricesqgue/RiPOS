using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Moq;
using RiPOS.Core.Services;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;
using RiPOS.Shared.Models.Settings;

namespace RiPOS.Core.Tests.Services;

public class LoginServiceTests
{
    private readonly LoginService _loginService;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    
    public LoginServiceTests()
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
        
        Mock<ILoginRepository> loginRepositoryMock = new Mock<ILoginRepository>();
        Mock<IMemoryCache> memoryCacheMock = new Mock<IMemoryCache>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();
        _loginService = new LoginService(
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
        var request = new LoginRequest { Username = "validUser", Password = "password" };
        var user = new User { Name = "User1", Surname = "User1", Username = "validUser", PasswordHash = "dh3TVAEOnBb9wUqqiz+Izb3SSUSKVemPrPTOUEMQ0xg6lDdY", IsActive = true };
        var userResponse = new UserResponse { Name = "User1",  Surname = "User1", Username = "validUser" };

        _userRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<User, bool>>>(), null))
            .ReturnsAsync(user);
        _mapperMock.Setup(mapper => mapper.Map<UserResponse>(user))
            .Returns(userResponse);

        var result = await _loginService.AuthenticateAsync(request);

        Assert.True(result.Success);
        Assert.Equal(userResponse, result.Data);
    }

    [Fact]
    public async Task AuthenticateAsync_ReturnsError_WhenUserDoesNotExist()
    {
        var request = new LoginRequest { Username = "invalidUser", Password = "password" };

        _userRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<User, bool>>>(), null))
            .ReturnsAsync((User?)null);

        var result = await _loginService.AuthenticateAsync(request);

        Assert.False(result.Success);
        Assert.Equal("Nombre de usuario y/o contraseña incorrectos", result.Message);
    }

    [Fact]
    public async Task AuthenticateAsync_ReturnsError_WhenUserIsInactive()
    {
        var request = new LoginRequest { Username = "inactiveUser", Password = "password" };
        var user = new User { Name = "InactiveUser", Surname = "test", Username = "inactiveUser", PasswordHash = "ZXMiN0NbKXi5ohoR2bH9+gjs+FaLAN9dosxywFeMPZ6AXJVJ", IsActive = false };

        _userRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<User, bool>>>(), null))
            .ReturnsAsync(user);

        var result = await _loginService.AuthenticateAsync(request);

        Assert.False(result.Success);
        Assert.Equal("Usuario inactivo", result.Message);
    }

    [Fact]
    public void BuildTokens_ReturnsValidTokenResponse()
    {
        var user = new UserResponse { Username = "validUser", Name = "User1", Surname = "User1" };
        var result = _loginService.BuildTokens(user);

        Assert.NotNull(result.AccessToken);
        Assert.NotNull(result.RefreshToken);
        Assert.True(result.Expires > DateTime.Now);
    }
}