using System.Linq.Expressions;
using AutoMapper;
using Moq;
using RiPOS.Core.MapProfiles;
using RiPOS.Core.Services;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models.Requests;

namespace RiPOS.Core.Tests.Services;

public class BrandServiceTests
{
    private readonly BrandService _brandService;
    private readonly Mock<IBrandRepository> _brandRepositoryMock;

    public BrandServiceTests()
    {
        var config = new MapperConfiguration(config =>
        {
            config.AddProfile(new BrandProfile());
        });
        var mapper = config.CreateMapper();
        
        _brandRepositoryMock = new Mock<IBrandRepository>();
        _brandService = new BrandService(_brandRepositoryMock.Object, mapper);
    }
    
    [Fact]
    public async Task GetAllAsync_ReturnsAllActiveBrands_WhenIncludeInactivesIsFalse()
    {
        var brands = new List<Brand>
        {
            new Brand { Id = 1, Name = "Brand1", IsActive = true },
            new Brand { Id = 2, Name = "Brand2", IsActive = true },
            new Brand { Id = 3, Name = "Brand3", IsActive = false }
        };
        _brandRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Brand, bool>>>(), null, null, false, 0, 0))
            .ReturnsAsync(brands.Where(b => b.IsActive).ToList());

        var result = await _brandService.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, b => b.Name == "Brand1");
        Assert.Contains(result, b => b.Name == "Brand2");
        Assert.DoesNotContain(result, b => b.Name == "Brand3");
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllBrandsIncludingInactives_WhenIncludeInactivesIsTrue()
    {
        var brands = new List<Brand>
        {
            new Brand { Id = 1, Name = "Brand1", IsActive = true },
            new Brand { Id = 2, Name = "Brand2", IsActive = true },
            new Brand { Id = 3, Name = "Brand3", IsActive = false }
        };
        _brandRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Brand, bool>>>(), null, null, false, 0, 0))
            .ReturnsAsync(brands);

        var result = await _brandService.GetAllAsync(true);

        Assert.Equal(3, result.Count);
        Assert.Contains(result, b => b.Name == "Brand1");
        Assert.Contains(result, b => b.Name == "Brand2");
        Assert.Contains(result, b => b.Name == "Brand3");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsBrand_WhenBrandExists()
    {
        var brand = new Brand { Id = 1, Name = "Brand1", IsActive = true };
        _brandRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Brand, bool>>>(), null))
            .ReturnsAsync(brand);

        var result = await _brandService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Brand1", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenBrandDoesNotExist()
    {
        _brandRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Brand, bool>>>(), null))
            .ReturnsAsync((Brand?)null);

        var result = await _brandService.GetByIdAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task ExistsByIdAsync_ReturnsTrue_WhenBrandExists()
    {
        _brandRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Brand, bool>>>()))
            .ReturnsAsync(true);

        var result = await _brandService.ExistsByIdAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsByIdAsync_ReturnsFalse_WhenBrandDoesNotExist()
    {
        _brandRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Brand, bool>>>()))
            .ReturnsAsync(false);

        var result = await _brandService.ExistsByIdAsync(1);

        Assert.False(result);
    }

    [Fact]
    public async Task AddAsync_ReturnsSuccessMessage_WhenBrandIsAdded()
    {
        var request = new BrandRequest { Name = "NewBrand" };
        _brandRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Brand, bool>>>()))
            .ReturnsAsync(false);
        _brandRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Brand>()))
            .ReturnsAsync(true);

        var result = await _brandService.AddAsync(request, 1);

        Assert.True(result.Success);
        Assert.Equal("Marca agregada correctamente", result.Message);
    }

    [Fact]
    public async Task AddAsync_ReturnsFailureMessage_WhenBrandAlreadyExists()
    {
        var request = new BrandRequest { Name = "ExistingBrand" };
        _brandRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Brand, bool>>>()))
            .ReturnsAsync(true);

        var result = await _brandService.AddAsync(request, 1);

        Assert.False(result.Success);
        Assert.Equal("Ya existe una marca con el nombre \"ExistingBrand\"", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsSuccessMessage_WhenBrandIsUpdated()
    {
        var request = new BrandRequest { Name = "UpdatedBrand" };
        var brand = new Brand { Id = 1, Name = "OldBrand", IsActive = true };
        _brandRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(brand);
        _brandRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Brand, bool>>>()))
            .ReturnsAsync(false);
        _brandRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Brand>()))
            .ReturnsAsync(true);

        var result = await _brandService.UpdateAsync(1, request, 1);

        Assert.True(result.Success);
        Assert.Equal("Marca modificada correctamente", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsFailureMessage_WhenBrandDoesNotExist()
    {
        var request = new BrandRequest { Name = "UpdatedBrand" };
        _brandRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Brand?)null);

        var result = await _brandService.UpdateAsync(1, request, 1);

        Assert.False(result.Success);
        Assert.Equal("No se encontró la marca", result.Message);
    }

    [Fact]
    public async Task DeactivateAsync_ReturnsSuccessMessage_WhenBrandIsDeactivated()
    {
        var brand = new Brand { Id = 1, Name = "BrandToDeactivate", IsActive = true };
        _brandRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(brand);
        _brandRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Brand>()))
            .ReturnsAsync(true);

        var result = await _brandService.DeactivateAsync(1, 1);

        Assert.True(result.Success);
        Assert.Equal("Marca eliminada correctamente", result.Data);
    }

    [Fact]
    public async Task DeactivateAsync_ReturnsFailureMessage_WhenBrandDoesNotExist()
    {
        _brandRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Brand?)null);

        var result = await _brandService.DeactivateAsync(1, 1);

        Assert.False(result.Success);
        Assert.Equal("No se encontró la marca", result.Message);
    }
}