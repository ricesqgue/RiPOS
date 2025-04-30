using System.Linq.Expressions;
using AutoMapper;
using Moq;
using RiPOS.Core.MapProfiles;
using RiPOS.Core.Services;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Repository.Session;
using RiPOS.Shared.Models.Requests;

namespace RiPOS.Core.Tests.Services;

public class GenderServiceTests
{
    private readonly GenderService _genderService;
    private readonly Mock<IGenderRepository> _genderRepositoryMock;
    
    public GenderServiceTests()
    {
        var config = new MapperConfiguration(config =>
        {
            config.AddProfile(new GenderProfile());
        });
        
        var mapper = config.CreateMapper();
        _genderRepositoryMock = new Mock<IGenderRepository>();

        var repositorySessionMock = new Mock<IRepositorySession>();
        Mock<IRepositorySessionFactory> repositorySessionFactoryMock = new Mock<IRepositorySessionFactory>();
        repositorySessionFactoryMock.Setup(f => f.CreateAsync(It.IsAny<bool>()))
            .ReturnsAsync(repositorySessionMock.Object);
        
        _genderService = new GenderService(repositorySessionFactoryMock.Object, _genderRepositoryMock.Object, mapper);
    }
    
    [Fact]
    public async Task GetAllAsync_ReturnsAllActiveGenders_WhenIncludeInactivesIsFalse()
    {
        var genders = new List<Gender>
        {
            new Gender { Id = 1, Name = "Male", IsActive = true },
            new Gender { Id = 2, Name = "Female", IsActive = true },
            new Gender { Id = 3, Name = "Other", IsActive = false }
        };
        _genderRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Gender, bool>>>(), null, null, false, 0, 0))
            .ReturnsAsync(genders.Where(g => g.IsActive).ToList());

        var result = await _genderService.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.All(result, g => Assert.True(g.IsActive));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllGenders_WhenIncludeInactivesIsTrue()
    {
        var genders = new List<Gender>
        {
            new Gender { Id = 1, Name = "Male", IsActive = true },
            new Gender { Id = 2, Name = "Female", IsActive = true },
            new Gender { Id = 3, Name = "Other", IsActive = false }
        };
        _genderRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Gender, bool>>>(), null, null, false, 0, 0))
            .ReturnsAsync(genders);

        var result = await _genderService.GetAllAsync(true);

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsGender_WhenGenderExists()
    {
        var gender = new Gender { Id = 1, Name = "Male", IsActive = true };
        _genderRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Gender, bool>>>(), null))
            .ReturnsAsync(gender);

        var result = await _genderService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenGenderDoesNotExist()
    {
        _genderRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Gender, bool>>>(), null))
            .ReturnsAsync((Gender?)null);

        var result = await _genderService.GetByIdAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task ExistsByIdAsync_ReturnsTrue_WhenGenderExists()
    {
        _genderRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Gender, bool>>>()))
            .ReturnsAsync(true);

        var result = await _genderService.ExistsByIdAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsByIdAsync_ReturnsFalse_WhenGenderDoesNotExist()
    {
        _genderRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Gender, bool>>>()))
            .ReturnsAsync(false);

        var result = await _genderService.ExistsByIdAsync(1);

        Assert.False(result);
    }

    [Fact]
    public async Task AddAsync_ReturnsSuccessMessage_WhenGenderIsAdded()
    {
        var request = new GenderRequest { Name = "Boy" };
        _genderRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Gender, bool>>>()))
            .ReturnsAsync(false);

        var result = await _genderService.AddAsync(request, 1);

        Assert.True(result.Success);
        Assert.Equal("Género agregado correctamente", result.Message);
    }

    [Fact]
    public async Task AddAsync_ReturnsErrorMessage_WhenGenderAlreadyExists()
    {
        var request = new GenderRequest { Name = "Male" };
        _genderRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Gender, bool>>>()))
            .ReturnsAsync(true);

        var result = await _genderService.AddAsync(request, 1);

        Assert.False(result.Success);
        Assert.Equal("Ya existe un género con el nombre \"Male\"", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsSuccessMessage_WhenGenderIsUpdated()
    {
        var request = new GenderRequest { Name = "UpdatedGender" };
        var gender = new Gender { Id = 1, Name = "OldGender", IsActive = true };
        _genderRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(gender);
        _genderRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Gender, bool>>>()))
            .ReturnsAsync(false);

        var result = await _genderService.UpdateAsync(1, request, 1);

        Assert.True(result.Success);
        Assert.Equal("Género modificado correctamente", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsErrorMessage_WhenGenderDoesNotExist()
    {
        var request = new GenderRequest { Name = "UpdatedGender" };
        _genderRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Gender?)null);

        var result = await _genderService.UpdateAsync(1, request, 1);

        Assert.False(result.Success);
        Assert.Equal("Género no encontrado", result.Message);
    }

    [Fact]
    public async Task DeactivateAsync_ReturnsSuccessMessage_WhenGenderIsDeactivated()
    {
        var gender = new Gender { Id = 1, Name = "Male", IsActive = true };
        _genderRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(gender);

        var result = await _genderService.DeactivateAsync(1, 1);

        Assert.True(result.Success);
        Assert.Equal("Género eliminado correctamente", result.Data);
    }

    [Fact]
    public async Task DeactivateAsync_ReturnsErrorMessage_WhenGenderDoesNotExist()
    {
        _genderRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Gender?)null);

        var result = await _genderService.DeactivateAsync(1, 1);

        Assert.False(result.Success);
        Assert.Equal("Género no encontrado", result.Message);
    }
}