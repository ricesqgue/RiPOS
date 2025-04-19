using System.Linq.Expressions;
using AutoMapper;
using Moq;
using RiPOS.Core.MapProfiles;
using RiPOS.Core.Services;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models.Requests;

namespace RiPOS.Core.Tests.Services;

public class StoreServiceTests
{
    private readonly StoreService _storeService;
    private readonly Mock<IStoreRepository> _storeRepositoryMock;

    public StoreServiceTests()
    {
        var config = new MapperConfiguration(config =>
        {
            config.AddProfile(new StoreProfile());
        });
        
        var mapper = config.CreateMapper();
        _storeRepositoryMock = new Mock<IStoreRepository>();
        _storeService = new StoreService(_storeRepositoryMock.Object, mapper);
    }
    
    [Fact]
    public async Task GetAllAsync_ReturnsAllStores()
    {
        var stores = new List<Store>
        {
            new Store { Id = 1, Name = "Store1", IsActive = true },
            new Store { Id = 2, Name = "Store2", IsActive = true }
        };
        _storeRepositoryMock.Setup(repo => repo.GetAllAsync(null, null, null, false, 0, 0))
            .ReturnsAsync(stores);

        var result = await _storeService.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal("Store1", result.First().Name);
        Assert.Equal("Store2", result.Last().Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsStore_WhenStoreExists()
    {
        var store = new Store { Id = 1, Name = "Store1", IsActive = true };
        _storeRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Store, bool>>>(), null))
            .ReturnsAsync(store);

        var result = await _storeService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenStoreDoesNotExist()
    {
        _storeRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Store, bool>>>(), null))
            .ReturnsAsync((Store?)null);

        var result = await _storeService.GetByIdAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task ExistsByIdAsync_ReturnsTrue_WhenStoreExists()
    {
        _storeRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Store, bool>>>()))
            .ReturnsAsync(true);

        var result = await _storeService.ExistsByIdAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsByIdAsync_ReturnsFalse_WhenStoreDoesNotExist()
    {
        _storeRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Store, bool>>>()))
            .ReturnsAsync(false);

        var result = await _storeService.ExistsByIdAsync(1);

        Assert.False(result);
    }

    [Fact]
    public async Task AddAsync_ReturnsSuccessMessage_WhenStoreIsAdded()
    {
        var request = new StoreRequest { Name = "NewStore" };
        _storeRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Store, bool>>>()))
            .ReturnsAsync(false);
        _storeRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Store>()))
            .ReturnsAsync(true);

        var result = await _storeService.AddAsync(request, 1);

        Assert.True(result.Success);
        Assert.Equal("Tienda agregada correctamente", result.Message);
    }

    [Fact]
    public async Task AddAsync_ReturnsErrorMessage_WhenStoreAlreadyExists()
    {
        var request = new StoreRequest { Name = "ExistingStore" };
        _storeRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Store, bool>>>()))
            .ReturnsAsync(true);

        var result = await _storeService.AddAsync(request, 1);

        Assert.False(result.Success);
        Assert.Equal("Ya existe una tienda con el nombre \"ExistingStore\"", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsSuccessMessage_WhenStoreIsUpdated()
    {
        var request = new StoreRequest { Name = "UpdatedStore" };
        var store = new Store { Id = 1, Name = "OldStore", IsActive = true };
        _storeRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(store);
        _storeRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Store, bool>>>()))
            .ReturnsAsync(false);
        _storeRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Store>()))
            .ReturnsAsync(true);

        var result = await _storeService.UpdateAsync(1, request, 1);

        Assert.True(result.Success);
        Assert.Equal("Tienda modificada correctamente", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsErrorMessage_WhenStoreDoesNotExist()
    {
        var request = new StoreRequest { Name = "UpdatedStore" };
        _storeRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Store?)null);

        var result = await _storeService.UpdateAsync(1, request, 1);

        Assert.False(result.Success);
        Assert.Equal("Tienda no encontrada", result.Message);
    }

    [Fact]
    public async Task DeactivateAsync_ReturnsSuccessMessage_WhenStoreIsDeactivated()
    {
        var store = new Store { Id = 1, Name = "Store1", IsActive = true };
        _storeRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(store);
        _storeRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Store>()))
            .ReturnsAsync(true);

        var result = await _storeService.DeactivateAsync(1, 1);

        Assert.True(result.Success);
        Assert.Equal("Tienda eliminada correctamente", result.Data);
    }

    [Fact]
    public async Task DeactivateAsync_ReturnsErrorMessage_WhenStoreDoesNotExist()
    {
        _storeRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Store?)null);

        var result = await _storeService.DeactivateAsync(1, 1);

        Assert.False(result.Success);
        Assert.Equal("Tienda no encontrada", result.Message);
    }
}