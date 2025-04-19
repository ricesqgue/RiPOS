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

public class CustomerServiceTests
{
    private readonly CustomerService _customerService;
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;

    public CustomerServiceTests()
    {
        var config = new MapperConfiguration(config =>
        {
            config.AddProfile(new CustomerProfile());
        });
        var mapper = config.CreateMapper();
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _customerService = new CustomerService(_customerRepositoryMock.Object, mapper);
    }
    
    [Fact]
    public async Task GetAllAsync_ReturnsAllActiveCustomers_WhenIncludeInactivesIsFalse()
    {
        var customers = new List<Customer>
        {
            new Customer { Id = 1, Name = "Customer1", Surname = "Customer1", CountryStateId = 1, IsActive = true },
            new Customer { Id = 2, Name = "Customer2", Surname = "Customer2", CountryStateId = 1, IsActive = true },
            new Customer { Id = 3, Name = "Customer3", Surname = "Customer3", CountryStateId = 1,IsActive = false }
        };
        _customerRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Customer, bool>>>(), It.IsAny<Func<IQueryable<Customer>, IIncludableQueryable<Customer, object>>>(), null, false, 0, 0))
            .ReturnsAsync(customers.Where(c => c.IsActive).ToList());

        var result = await _customerService.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.All(result, c => Assert.True(c.IsActive));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllCustomers_WhenIncludeInactivesIsTrue()
    {
        var customers = new List<Customer>
        {
            new Customer { Id = 1, Name = "Customer1", Surname = "Customer1", CountryStateId = 1, IsActive = true },
            new Customer { Id = 2, Name = "Customer2", Surname = "Customer2", CountryStateId = 1, IsActive = true },
            new Customer { Id = 3, Name = "Customer3", Surname = "Customer3", CountryStateId = 1, IsActive = false }
        };
        _customerRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Customer, bool>>>(), It.IsAny<Func<IQueryable<Customer>, IIncludableQueryable<Customer, object>>>(), null, false, 0, 0))
            .ReturnsAsync(customers);

        var result = await _customerService.GetAllAsync(true);

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCustomer_WhenCustomerExists()
    {
        var customer = new Customer { Id = 1, Name = "Customer1", Surname = "Customer1", CountryStateId = 1, IsActive = true };
        _customerRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Customer, bool>>>(), It.IsAny<Func<IQueryable<Customer>, IIncludableQueryable<Customer, object>>>()))
            .ReturnsAsync(customer);

        var result = await _customerService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenCustomerDoesNotExist()
    {
        _customerRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Customer, bool>>>(), It.IsAny<Func<IQueryable<Customer>, IIncludableQueryable<Customer, object>>>()))
            .ReturnsAsync((Customer?)null);

        var result = await _customerService.GetByIdAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task ExistsByIdAsync_ReturnsTrue_WhenCustomerExists()
    {
        _customerRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync(true);

        var result = await _customerService.ExistsByIdAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsByIdAsync_ReturnsFalse_WhenCustomerDoesNotExist()
    {
        _customerRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
            .ReturnsAsync(false);

        var result = await _customerService.ExistsByIdAsync(1);

        Assert.False(result);
    }

    [Fact]
    public async Task AddAsync_ReturnsSuccessMessage_WhenCustomerIsAdded()
    {
        var request = new CustomerRequest { Name = "NewCustomer", Surname = "customer1", CountryStateId  = 1, Email = "newcustomer@example.com" };
        _customerRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Customer, bool>>>(), null))
            .ReturnsAsync((Customer?)null);
        _customerRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Customer>()))
            .ReturnsAsync(true);

        var result = await _customerService.AddAsync(request, 1);

        Assert.True(result.Success);
        Assert.Equal("Cliente agregado correctamente", result.Message);
    }

    [Fact]
    public async Task AddAsync_ReturnsErrorMessage_WhenCustomerAlreadyExists()
    {
        var request = new CustomerRequest { Name = "ExistingCustomer", Surname = "customer1", CountryStateId  = 1, Email = "existingcustomer@example.com" };
        var existingCustomer = new Customer { Id = 1, Name = "ExistingCustomer", Email = "existingcustomer@example.com", Surname = "customer1", CountryStateId  = 1, IsActive = true };
        _customerRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Customer, bool>>>(), null))
            .ReturnsAsync(existingCustomer);

        var result = await _customerService.AddAsync(request, 1);

        Assert.False(result.Success);
        Assert.Equal("Ya existe un cliente con el email \"existingcustomer@example.com\"", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsSuccessMessage_WhenCustomerIsUpdated()
    {
        var request = new CustomerRequest { Name = "UpdatedCustomer", Email = "updatedcustomer@example.com", Surname = "customer1", CountryStateId  = 1 };
        var customer = new Customer { Id = 1, Name = "OldCustomer", Email = "oldcustomer@example.com", Surname = "customer1", CountryStateId  = 1, IsActive = true };
        _customerRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(customer);
        _customerRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Customer, bool>>>(), null))
            .ReturnsAsync((Customer?)null);
        _customerRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Customer>()))
            .ReturnsAsync(true);

        var result = await _customerService.UpdateAsync(1, request, 1);

        Assert.True(result.Success);
        Assert.Equal("Cliente modificado correctamente", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsErrorMessage_WhenCustomerDoesNotExist()
    {
        var request = new CustomerRequest { Name = "UpdatedCustomer", Email = "updatedcustomer@example.com", Surname = "customer1", CountryStateId  = 1,};
        _customerRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Customer?)null);

        var result = await _customerService.UpdateAsync(1, request, 1);

        Assert.False(result.Success);
        Assert.Equal("Cliente no encontrado", result.Message);
    }

    [Fact]
    public async Task DeactivateAsync_ReturnsSuccessMessage_WhenCustomerIsDeactivated()
    {
        var customer = new Customer { Id = 1, Name = "Customer1", Surname = "customer1", CountryStateId  = 1, IsActive = true };
        _customerRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(customer);
        _customerRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Customer>()))
            .ReturnsAsync(true);

        var result = await _customerService.DeactivateAsync(1, 1);

        Assert.True(result.Success);
        Assert.Equal("Cliente eliminado correctamente", result.Data);
    }

    [Fact]
    public async Task DeactivateAsync_ReturnsErrorMessage_WhenCustomerDoesNotExist()
    {
        _customerRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Customer?)null);

        var result = await _customerService.DeactivateAsync(1, 1);

        Assert.False(result.Success);
        Assert.Equal("Cliente no encontrado", result.Message);
    }
}