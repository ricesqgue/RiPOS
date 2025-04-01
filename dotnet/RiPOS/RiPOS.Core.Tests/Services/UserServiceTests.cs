using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using RiPOS.Core.MapProfiles;
using RiPOS.Core.Services;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Enums;

namespace RiPOS.Core.Tests.Services;

public class UserServiceTests
{
    private readonly UserService _userService;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public UserServiceTests()
    {
        var config = new MapperConfiguration(config =>
        {
            config.AddProfile(new UserProfile());
        });
        
        var mapper = config.CreateMapper();
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object, mapper);
    }
    
    [Fact]
    public async Task GetAllAsync_ReturnsAllActiveUsers_WhenIncludeInactivesIsFalse()
    {
        var users = new List<User>
        {
            new User { Id = 1, Name = "User1", Surname = "User1",IsActive = true, Username = "User1", PasswordHash = "password" },
            new User { Id = 2, Name = "User2", Surname = "User2",IsActive = true, Username = "User2", PasswordHash = "password" },
            new User { Id = 3, Name = "User3", Surname = "User3",IsActive = false, Username = "User3", PasswordHash = "password" },
        };
        _userRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(), null, null, false, 0, 0))
            .ReturnsAsync(users.Where(u => u.IsActive).ToList());

        var result = await _userService.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.All(result, u => Assert.True(u.IsActive));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllUsers_WhenIncludeInactivesIsTrue()
    {
        var users = new List<User>
        {
            new User { Id = 1, Name = "User1", Surname = "User1",IsActive = true, Username = "User1", PasswordHash = "password" },
            new User { Id = 2, Name = "User2", Surname = "User2",IsActive = true, Username = "User2", PasswordHash = "password" },
            new User { Id = 3, Name = "User3", Surname = "User3",IsActive = false, Username = "User3", PasswordHash = "password" },
        };
        _userRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(), null, null, false, 0, 0))
            .ReturnsAsync(users);

        var result = await _userService.GetAllAsync(true);

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetAllByStoreAsync_ReturnsAllActiveUsersInStore_WhenIncludeInactivesIsFalse()
    {
        var users = new List<User>
        {
            new User { Id = 1, Name = "User1", Surname = "User1", IsActive = true, Username = "User1", PasswordHash = "password", UserStoreRoles = new List<UserStoreRole> { new UserStoreRole { StoreId = 1 } } },
            new User { Id = 2, Name = "User2", Surname = "User2", IsActive = true, Username = "User2", PasswordHash = "password", UserStoreRoles = new List<UserStoreRole> { new UserStoreRole { StoreId = 1 } } },
            new User { Id = 3, Name = "User3", Surname = "User3", IsActive = false, Username = "User3", PasswordHash = "password", UserStoreRoles = new List<UserStoreRole> { new UserStoreRole { StoreId = 1 } } }
        };
        _userRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>(), null, false, 0, 0))
            .ReturnsAsync(users.Where(u => u.IsActive).ToList());

        var result = await _userService.GetAllByStoreAsync(1);

        Assert.Equal(2, result.Count);
        Assert.All(result, u => Assert.True(u.IsActive));
    }

    [Fact]
    public async Task GetByIdInStoreAsync_ReturnsUser_WhenUserExistsInStore()
    {
        var user = new User
        {
            Id = 1, Name = "User1", Surname = "User1", IsActive = true, Username = "User1", PasswordHash = "password",
            UserStoreRoles = new List<UserStoreRole> { new UserStoreRole { StoreId = 1 } }
        };
        _userRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
            .ReturnsAsync(user);

        var result = await _userService.GetByIdInStoreAsync(1, 1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByIdInStoreAsync_ReturnsNull_WhenUserDoesNotExistInStore()
    {
        _userRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
            .ReturnsAsync((User?)null);

        var result = await _userService.GetByIdInStoreAsync(1, 1);

        Assert.Null(result);
    }

    [Fact]
    public async Task ExistsByIdInStoreAsync_ReturnsTrue_WhenUserExistsInStore()
    {
        _userRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(true);

        var result = await _userService.ExistsByIdInStoreAsync(1, 1);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsByIdInStoreAsync_ReturnsFalse_WhenUserDoesNotExistInStore()
    {
        _userRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(false);

        var result = await _userService.ExistsByIdInStoreAsync(1, 1);

        Assert.False(result);
    }

    [Fact]
    public async Task GetUserRolesByStoreIdAsync_ReturnsRoles_WhenUserHasRolesInStore()
    {
        var roles = new List<Role> { new Role { Id = 1, Name = "Admin", Code = "ADM", Description = ""}, new Role { Id = 100, Name = "SuperAdmin", Code = "SADM", Description = ""} };
        _userRepositoryMock.Setup(repo => repo.GetStoreRolesAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(roles);

        var result = await _userService.GetUserRolesByStoreIdAsync(1, 1);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r == RoleEnum.Admin);
        Assert.Contains(result, r => r == RoleEnum.SuperAdmin);
    }
}