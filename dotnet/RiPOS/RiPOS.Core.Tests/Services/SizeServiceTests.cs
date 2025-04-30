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

public class SizeServiceTests
{
    private readonly SizeService _sizeService;
    private readonly Mock<ISizeRepository> _sizeRepositoryMock;
    
    public SizeServiceTests()
    {
        var config = new MapperConfiguration(config =>
        {
            config.AddProfile(new SizeProfile());
        });
        
        var mapper = config.CreateMapper();
        _sizeRepositoryMock = new Mock<ISizeRepository>();

        var repositorySessionMock = new Mock<IRepositorySession>();
        Mock<IRepositorySessionFactory> repositorySessionFactoryMock = new Mock<IRepositorySessionFactory>();
        repositorySessionFactoryMock.Setup(f => f.CreateAsync(It.IsAny<bool>()))
            .ReturnsAsync(repositorySessionMock.Object);
        
        _sizeService = new SizeService(repositorySessionFactoryMock.Object, _sizeRepositoryMock.Object, mapper);
    }
    
    [Fact]
    public async Task GetAllAsync_ReturnsAllActiveSizes_WhenIncludeInactivesIsFalse()
    {
        var sizes = new List<Size>
        {
            new Size { Id = 1, Name = "Small", ShortName = "S", IsActive = true },
            new Size { Id = 2, Name = "Medium", ShortName = "M", IsActive = true },
            new Size { Id = 3, Name = "Large", ShortName = "L", IsActive = false }
        };
        _sizeRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Size, bool>>>(), null, null, false, 0, 0))
            .ReturnsAsync(sizes.Where(s => s.IsActive).ToList());

        var result = await _sizeService.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.All(result, s => Assert.True(s.IsActive));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllSizes_WhenIncludeInactivesIsTrue()
    {
        var sizes = new List<Size>
        {
            new Size { Id = 1, Name = "Small", ShortName = "S", IsActive = true },
            new Size { Id = 2, Name = "Medium", ShortName = "M", IsActive = true },
            new Size { Id = 3, Name = "Large", ShortName = "L", IsActive = false }
        };
        _sizeRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Size, bool>>>(), null, null, false, 0, 0))
            .ReturnsAsync(sizes);

        var result = await _sizeService.GetAllAsync(true);

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsSize_WhenSizeExists()
    {
        var size = new Size { Id = 1, Name = "Small", ShortName = "S", IsActive = true };
        _sizeRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Size, bool>>>(), null))
            .ReturnsAsync(size);

        var result = await _sizeService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenSizeDoesNotExist()
    {
        _sizeRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Size, bool>>>(), null))
            .ReturnsAsync((Size?)null);

        var result = await _sizeService.GetByIdAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task ExistsByIdAsync_ReturnsTrue_WhenSizeExists()
    {
        _sizeRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Size, bool>>>()))
            .ReturnsAsync(true);

        var result = await _sizeService.ExistsByIdAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsByIdAsync_ReturnsFalse_WhenSizeDoesNotExist()
    {
        _sizeRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Size, bool>>>()))
            .ReturnsAsync(false);

        var result = await _sizeService.ExistsByIdAsync(1);

        Assert.False(result);
    }

    [Fact]
    public async Task AddAsync_ReturnsSuccessMessage_WhenSizeIsAdded()
    {
        var request = new SizeRequest { Name = "ExtraLarge", ShortName = "XL" };

        _sizeRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Size, bool>>>(), null))
            .ReturnsAsync((Size?)null);

        var result = await _sizeService.AddAsync(request, 1);

        Assert.True(result.Success);
        Assert.Equal("Talla agregada correctamente", result.Message);
    }

    [Fact]
    public async Task AddAsync_ReturnsErrorMessage_WhenSizeAlreadyExists()
    {
        var request = new SizeRequest { Name = "Small", ShortName = "S" };
        var size = new Size { Id = 1, Name = "Small", ShortName = "S", IsActive = true };
        _sizeRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Size, bool>>>(), null))
            .ReturnsAsync(size);

        var result = await _sizeService.AddAsync(request, 1);

        Assert.False(result.Success);
        Assert.Equal("Ya existe una talla con el nombre \"Small\"", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsSuccessMessage_WhenSizeIsUpdated()
    {
        var request = new SizeRequest { Name = "UpdatedSize", ShortName = "US" };
        var size = new Size { Id = 1, Name = "OldSize", ShortName = "OS", IsActive = true };
        _sizeRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(size);
        _sizeRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Size, bool>>>(), null))
            .ReturnsAsync((Size?)null);

        var result = await _sizeService.UpdateAsync(1, request, 1);

        Assert.True(result.Success);
        Assert.Equal("Talla modificada correctamente", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsErrorMessage_WhenSizeDoesNotExist()
    {
        var request = new SizeRequest { Name = "UpdatedSize", ShortName = "US" };
        _sizeRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Size?)null);

        var result = await _sizeService.UpdateAsync(1, request, 1);

        Assert.False(result.Success);
        Assert.Equal("Talla no encontrada", result.Message);
    }

    [Fact]
    public async Task DeactivateAsync_ReturnsSuccessMessage_WhenSizeIsDeactivated()
    {
        var size = new Size { Id = 1, Name = "Small", ShortName = "S", IsActive = true };
        _sizeRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(size);

        var result = await _sizeService.DeactivateAsync(1, 1);

        Assert.True(result.Success);
        Assert.Equal("Talla eliminada correctamente", result.Data);
    }

    [Fact]
    public async Task DeactivateAsync_ReturnsErrorMessage_WhenSizeDoesNotExist()
    {
        _sizeRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Size?)null);

        var result = await _sizeService.DeactivateAsync(1, 1);

        Assert.False(result.Success);
        Assert.Equal("Talla no encontrada", result.Message);
    }
}