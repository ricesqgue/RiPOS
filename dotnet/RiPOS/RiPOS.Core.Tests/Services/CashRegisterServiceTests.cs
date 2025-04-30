using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using RiPOS.Core.MapProfiles;
using RiPOS.Core.Services;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Repository.Session;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Session;

namespace RiPOS.Core.Tests.Services;

public class CashRegisterServiceTests
{
    private readonly CashRegisterService _cashRegisterService;
    private readonly Mock<ICashRegisterRepository> _cashRegisterRepositoryMock;

    public CashRegisterServiceTests()
    {
        var config = new MapperConfiguration(config =>
        {
            config.AddProfile(new CashRegisterProfile());
        });
        var mapper = config.CreateMapper();
        
        _cashRegisterRepositoryMock = new Mock<ICashRegisterRepository>();
        var repositorySessionMock = new Mock<IRepositorySession>();
        
        Mock<IRepositorySessionFactory> repositorySessionFactoryMock = new Mock<IRepositorySessionFactory>();
        repositorySessionFactoryMock.Setup(f => f.CreateAsync(It.IsAny<bool>()))
            .ReturnsAsync(repositorySessionMock.Object);
        
        _cashRegisterService = new CashRegisterService(repositorySessionFactoryMock.Object, _cashRegisterRepositoryMock.Object, mapper);
    }
    
    [Fact]
    public async Task GetAllAsync_ReturnsAllActiveCashRegisters_WhenIncludeInactivesIsFalse()
    {
        var cashRegisters = new List<CashRegister>
        {
            new CashRegister { Id = 1, Name = "Register1", IsActive = true, StoreId = 1 },
            new CashRegister { Id = 2, Name = "Register2", IsActive = true, StoreId = 1 },
            new CashRegister { Id = 3, Name = "Register3", IsActive = false, StoreId = 1 },
            new CashRegister { Id = 4, Name = "Register4", IsActive = true, StoreId = 2 }
        };
        
        _cashRegisterRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<CashRegister, bool>>>(), null, null, false, 0, 0))
            .ReturnsAsync((Expression<Func<CashRegister, bool>> filter, Func<IQueryable<CashRegister>, IIncludableQueryable<CashRegister, object>>? _, Expression<Func<CashRegister, object>>? _, bool _, int _, int _)
                => cashRegisters.AsQueryable().Where(filter).ToList());

        var result = await _cashRegisterService.GetAllAsync(1);

        Assert.Equal(2, result.Count);
        Assert.All(result, cr => Assert.Equal(1, cr.StoreId));
        Assert.Contains(result, cr => cr.Name == "Register1");
        Assert.Contains(result, cr => cr.Name == "Register2");
        Assert.DoesNotContain(result, cr => cr.Name == "Register3");
        Assert.DoesNotContain(result, cr => cr.StoreId == 2);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllCashRegistersIncludingInactives_WhenIncludeInactivesIsTrue()
    {
        var cashRegisters = new List<CashRegister>
        {
            new CashRegister { Id = 1, Name = "Register1", IsActive = true, StoreId = 1 },
            new CashRegister { Id = 2, Name = "Register2", IsActive = true, StoreId = 1},
            new CashRegister { Id = 3, Name = "Register3", IsActive = false, StoreId = 1},
            new CashRegister { Id = 4, Name = "Register3", IsActive = false, StoreId = 2},
        };
        _cashRegisterRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<CashRegister, bool>>>(), null, null, false, 0, 0))
            .ReturnsAsync((Expression<Func<CashRegister, bool>> filter, Func<IQueryable<CashRegister>, IIncludableQueryable<CashRegister, object>>? _, Expression<Func<CashRegister, object>>? _, bool _, int _, int _)
                => cashRegisters.AsQueryable().Where(filter).ToList());

        var result = await _cashRegisterService.GetAllAsync(1, true);

        Assert.Equal(3, result.Count);
        Assert.Contains(result, cr => cr.Name == "Register1");
        Assert.Contains(result, cr => cr.Name == "Register2");
        Assert.Contains(result, cr => cr.Name == "Register3");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCashRegister_WhenCashRegisterExists()
    {
        var cashRegister = new CashRegister { Id = 1, Name = "Register1", IsActive = true, StoreId = 1};
        _cashRegisterRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<CashRegister, bool>>>(), null))
            .ReturnsAsync(cashRegister);

        var result = await _cashRegisterService.GetByIdAsync(1, 1);

        Assert.NotNull(result);
        Assert.Equal("Register1", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenCashRegisterDoesNotExist()
    {
        _cashRegisterRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<CashRegister, bool>>>(), null))
            .ReturnsAsync((CashRegister?)null);

        var result = await _cashRegisterService.GetByIdAsync(1, 1);

        Assert.Null(result);
    }

    [Fact]
    public async Task ExistsByIdAsync_ReturnsTrue_WhenCashRegisterExists()
    {
        _cashRegisterRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<CashRegister, bool>>>()))
            .ReturnsAsync(true);

        var result = await _cashRegisterService.ExistsByIdAsync(1, 1);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsByIdAsync_ReturnsFalse_WhenCashRegisterDoesNotExist()
    {
        _cashRegisterRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<CashRegister, bool>>>()))
            .ReturnsAsync(false);

        var result = await _cashRegisterService.ExistsByIdAsync(1, 1);

        Assert.False(result);
    }

    [Fact]
    public async Task AddAsync_ReturnsSuccessMessage_WhenCashRegisterIsAdded()
    {
        var request = new CashRegisterRequest { Name = "NewRegister" };
        _cashRegisterRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<CashRegister, bool>>>()))
            .ReturnsAsync(false);
        
        var result = await _cashRegisterService.AddAsync(request, new UserSession() { UserId = 1, StoreId = 1});

        Assert.True(result.Success);
        Assert.Equal("Caja agregada correctamente", result.Message);
    }

    [Fact]
    public async Task AddAsync_ReturnsFailureMessage_WhenCashRegisterAlreadyExists()
    {
        var request = new CashRegisterRequest { Name = "ExistingRegister" };
        _cashRegisterRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<CashRegister, bool>>>()))
            .ReturnsAsync(true);

        var result = await _cashRegisterService.AddAsync(request, new UserSession() { UserId = 1, StoreId = 1});

        Assert.False(result.Success);
        Assert.Equal("Ya existe una caja con el nombre \"ExistingRegister\"", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsSuccessMessage_WhenCashRegisterIsUpdated()
    {
        var request = new CashRegisterRequest { Name = "UpdatedRegister" };
        var cashRegister = new CashRegister { Id = 1, Name = "OldRegister", IsActive = true };
        _cashRegisterRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(cashRegister);
        _cashRegisterRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<CashRegister, bool>>>()))
            .ReturnsAsync(false);
        
        var result = await _cashRegisterService.UpdateAsync(1, request, new UserSession() { UserId = 1, StoreId = 1});

        Assert.True(result.Success);
        Assert.Equal("Caja modificada correctamente", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsFailureMessage_WhenCashRegisterDoesNotExist()
    {
        var request = new CashRegisterRequest { Name = "UpdatedRegister" };
        _cashRegisterRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((CashRegister?)null);

        var result = await _cashRegisterService.UpdateAsync(1, request, new UserSession() { UserId = 1, StoreId = 1});

        Assert.False(result.Success);
        Assert.Equal("Caja no encontrada", result.Message);
    }

    [Fact]
    public async Task DeactivateAsync_ReturnsSuccessMessage_WhenCashRegisterIsDeactivated()
    {
        var cashRegister = new CashRegister { Id = 1, Name = "RegisterToDeactivate", IsActive = true };
        _cashRegisterRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(cashRegister);
        
        var result = await _cashRegisterService.DeactivateAsync(1, 1);

        Assert.True(result.Success);
        Assert.Equal("Caja eliminada correctamente", result.Data);
    }

    [Fact]
    public async Task DeactivateAsync_ReturnsFailureMessage_WhenCashRegisterDoesNotExist()
    {
        _cashRegisterRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((CashRegister?)null);

        var result = await _cashRegisterService.DeactivateAsync(1, 1);

        Assert.False(result.Success);
        Assert.Equal("Caja no encontrada", result.Message);
    }
}