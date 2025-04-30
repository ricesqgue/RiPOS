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

public class CategoryServiceTests
{
    private readonly CategoryService _categoryService;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    
    public CategoryServiceTests()
    {
        var config = new MapperConfiguration(config =>
        {
            config.AddProfile(new CategoryProfile());
        });
        var mapper = config.CreateMapper();
        
        _categoryRepositoryMock = new Mock<ICategoryRepository>();

        var repositorySessionMock = new Mock<IRepositorySession>();
        Mock<IRepositorySessionFactory> repositorySessionFactoryMock = new Mock<IRepositorySessionFactory>();
        repositorySessionFactoryMock.Setup(f => f.CreateAsync(It.IsAny<bool>()))
            .ReturnsAsync(repositorySessionMock.Object);
        
        _categoryService = new CategoryService(repositorySessionFactoryMock.Object, _categoryRepositoryMock.Object, mapper);
    }
    
    [Fact]
    public async Task GetAllAsync_ReturnsAllActiveCategories_WhenIncludeInactivesIsFalse()
    {
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Category1", IsActive = true },
            new Category { Id = 2, Name = "Category2", IsActive = true },
            new Category { Id = 3, Name = "Category3", IsActive = false }
        };
        _categoryRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Category, bool>>>(), null, null, false, 0, 0))
            .ReturnsAsync(categories.Where(c => c.IsActive).ToList());

        var result = await _categoryService.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.All(result, c => Assert.True(c.IsActive));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllCategories_WhenIncludeInactivesIsTrue()
    {
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Category1", IsActive = true },
            new Category { Id = 2, Name = "Category2", IsActive = true },
            new Category { Id = 3, Name = "Category3", IsActive = false }
        };
        _categoryRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Category, bool>>>(), null, null, false, 0, 0))
            .ReturnsAsync(categories);

        var result = await _categoryService.GetAllAsync(true);

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCategory_WhenCategoryExists()
    {
        var category = new Category { Id = 1, Name = "Category1", IsActive = true };
        _categoryRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Category, bool>>>(), null))
            .ReturnsAsync(category);

        var result = await _categoryService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenCategoryDoesNotExist()
    {
        _categoryRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Category, bool>>>(), null))
            .ReturnsAsync((Category?)null);

        var result = await _categoryService.GetByIdAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task ExistsByIdAsync_ReturnsTrue_WhenCategoryExists()
    {
        _categoryRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(true);

        var result = await _categoryService.ExistsByIdAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsByIdAsync_ReturnsFalse_WhenCategoryDoesNotExist()
    {
        _categoryRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(false);

        var result = await _categoryService.ExistsByIdAsync(1);

        Assert.False(result);
    }

    [Fact]
    public async Task AddAsync_ReturnsSuccessMessage_WhenCategoryIsAdded()
    {
        var request = new CategoryRequest { Name = "NewCategory" };
        _categoryRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(false);

        var result = await _categoryService.AddAsync(request, 1);

        Assert.True(result.Success);
        Assert.Equal("Categoría agregada correctamente", result.Message);
    }

    [Fact]
    public async Task AddAsync_ReturnsErrorMessage_WhenCategoryAlreadyExists()
    {
        var request = new CategoryRequest { Name = "ExistingCategory" };
        _categoryRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(true);

        var result = await _categoryService.AddAsync(request, 1);

        Assert.False(result.Success);
        Assert.Equal("Ya existe una categoría con el nombre \"ExistingCategory\"", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsSuccessMessage_WhenCategoryIsUpdated()
    {
        var request = new CategoryRequest { Name = "UpdatedCategory" };
        var category = new Category { Id = 1, Name = "OldCategory", IsActive = true };
        _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(category);
        _categoryRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(false);

        var result = await _categoryService.UpdateAsync(1, request, 1);

        Assert.True(result.Success);
        Assert.Equal("Categoría modificada correctamente", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsErrorMessage_WhenCategoryDoesNotExist()
    {
        var request = new CategoryRequest { Name = "UpdatedCategory" };
        _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Category?)null);

        var result = await _categoryService.UpdateAsync(1, request, 1);

        Assert.False(result.Success);
        Assert.Equal("Categoría no encontrada", result.Message);
    }

    [Fact]
    public async Task DeactivateAsync_ReturnsSuccessMessage_WhenCategoryIsDeactivated()
    {
        var category = new Category { Id = 1, Name = "Category1", IsActive = true };
        _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(category);
        
        var result = await _categoryService.DeactivateAsync(1, 1);

        Assert.True(result.Success);
        Assert.Equal("Categoría eliminada correctamente", result.Data);
    }

    [Fact]
    public async Task DeactivateAsync_ReturnsErrorMessage_WhenCategoryDoesNotExist()
    {
        _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Category?)null);

        var result = await _categoryService.DeactivateAsync(1, 1);

        Assert.False(result.Success);
        Assert.Equal("Categoría no encontrada", result.Message);
    }
}