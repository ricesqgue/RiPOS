using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using RiPOS.Core.MapProfiles;
using RiPOS.Core.Services;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models.Requests;

namespace RiPOS.Core.Tests.Services;

public class VendorServiceTests
{
    private readonly VendorService _vendorService;
    private readonly Mock<IVendorRepository> _vendorRepositoryMock;

    public VendorServiceTests()
    {
        var config = new MapperConfiguration(config =>
        {
            config.AddProfile(new VendorProfile());
        });
        
        var mapper = config.CreateMapper();
        _vendorRepositoryMock = new Mock<IVendorRepository>();
        _vendorService = new VendorService(_vendorRepositoryMock.Object, mapper);
    }
    
    [Fact]
    public async Task GetAllAsync_ReturnsAllActiveVendors_WhenIncludeInactivesIsFalse()
    {
        var vendors = new List<Vendor>
        {
            new Vendor { Id = 1, Name = "Vendor1", Surname = "Test", IsActive = true },
            new Vendor { Id = 2, Name = "Vendor2", Surname = "Test", IsActive = true },
            new Vendor { Id = 3, Name = "Vendor3", Surname = "Test", IsActive = false }
        };
        _vendorRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Vendor, bool>>>(), It.IsAny<Func<IQueryable<Vendor>, IIncludableQueryable<Vendor, object>>>(), null, false, 0, 0))
            .ReturnsAsync(vendors.Where(v => v.IsActive).ToList());

        var result = await _vendorService.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.All(result, v => Assert.True(v.IsActive));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllVendors_WhenIncludeInactivesIsTrue()
    {
        var vendors = new List<Vendor>
        {
            new Vendor { Id = 1, Name = "Vendor1", Surname = "Test", IsActive = true },
            new Vendor { Id = 2, Name = "Vendor2", Surname = "Test", IsActive = true },
            new Vendor { Id = 3, Name = "Vendor3", Surname = "Test", IsActive = false }
        };
        _vendorRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Vendor, bool>>>(), It.IsAny<Func<IQueryable<Vendor>, IIncludableQueryable<Vendor, object>>>(), null, false, 0, 0))
            .ReturnsAsync(vendors);

        var result = await _vendorService.GetAllAsync(true);

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsVendor_WhenVendorExists()
    {
        var vendor = new Vendor { Id = 1, Name = "Vendor1", Surname = "Test", IsActive = true };
        _vendorRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Vendor, bool>>>(), It.IsAny<Func<IQueryable<Vendor>, IIncludableQueryable<Vendor, object>>>()))
            .ReturnsAsync(vendor);

        var result = await _vendorService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenVendorDoesNotExist()
    {
        _vendorRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Vendor, bool>>>(), It.IsAny<Func<IQueryable<Vendor>, IIncludableQueryable<Vendor, object>>>()))
            .ReturnsAsync((Vendor?)null);

        var result = await _vendorService.GetByIdAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task ExistsByIdAsync_ReturnsTrue_WhenVendorExists()
    {
        _vendorRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Vendor, bool>>>()))
            .ReturnsAsync(true);

        var result = await _vendorService.ExistsByIdAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsByIdAsync_ReturnsFalse_WhenVendorDoesNotExist()
    {
        _vendorRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Vendor, bool>>>()))
            .ReturnsAsync(false);

        var result = await _vendorService.ExistsByIdAsync(1);

        Assert.False(result);
    }

    [Fact]
    public async Task AddAsync_ReturnsSuccessMessage_WhenVendorIsAdded()
    {
        var request = new VendorRequest { Name = "NewVendor", Surname = "Surname", CountryStateId = 1 };
        _vendorRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Vendor, bool>>>()))
            .ReturnsAsync(false);
        _vendorRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Vendor>()))
            .ReturnsAsync(true);

        var result = await _vendorService.AddAsync(request, 1);

        Assert.True(result.Success);
        Assert.Equal("Proveedor agregado correctamente", result.Message);
    }

    [Fact]
    public async Task AddAsync_ReturnsErrorMessage_WhenVendorAlreadyExists()
    {
        var request = new VendorRequest { Name = "ExistingVendor", Surname = "Surname", CountryStateId = 1, Email = "test@test.com"};
        _vendorRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Vendor, bool>>>()))
            .ReturnsAsync(true);

        var result = await _vendorService.AddAsync(request, 1);

        Assert.False(result.Success);
        Assert.Equal("Ya existe un proveedor con el email \"test@test.com\"", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsSuccessMessage_WhenVendorIsUpdated()
    {
        var request = new VendorRequest { Name = "UpdatedVendor", Surname = "UpdatedSurname", CountryStateId = 1 };
        var vendor = new Vendor { Id = 1, Name = "OldVendor", Surname = "OldSurname", IsActive = true };
        _vendorRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(vendor);
        _vendorRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Vendor, bool>>>()))
            .ReturnsAsync(false);
        _vendorRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Vendor>()))
            .ReturnsAsync(true);

        var result = await _vendorService.UpdateAsync(1, request, 1);

        Assert.True(result.Success);
        Assert.Equal("Proveedor modificado correctamente", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsErrorMessage_WhenVendorDoesNotExist()
    {
        var request = new VendorRequest { Name = "UpdatedVendor", Surname = "UpdatedSurname", CountryStateId = 1 };
        _vendorRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Vendor?)null);

        var result = await _vendorService.UpdateAsync(1, request, 1);

        Assert.False(result.Success);
        Assert.Equal("Proveedor no encontrado", result.Message);
    }

    [Fact]
    public async Task DeactivateAsync_ReturnsSuccessMessage_WhenVendorIsDeactivated()
    {
        var vendor = new Vendor { Id = 1, Name = "Vendor1", Surname = "Test", IsActive = true };
        _vendorRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(vendor);
        _vendorRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Vendor>()))
            .ReturnsAsync(true);

        var result = await _vendorService.DeactivateAsync(1, 1);

        Assert.True(result.Success);
        Assert.Equal("Proveedor eliminado correctamente", result.Data);
    }

    [Fact]
    public async Task DeactivateAsync_ReturnsErrorMessage_WhenVendorDoesNotExist()
    {
        _vendorRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Vendor?)null);

        var result = await _vendorService.DeactivateAsync(1, 1);

        Assert.False(result.Success);
        Assert.Equal("Proveedor no encontrado", result.Message);
    }
}