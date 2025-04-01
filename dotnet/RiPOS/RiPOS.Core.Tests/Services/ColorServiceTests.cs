using System.Linq.Expressions;
using AutoMapper;
using Moq;
using RiPOS.Core.MapProfiles;
using RiPOS.Core.Services;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models.Requests;

namespace RiPOS.Core.Tests.Services;

public class ColorServiceTests
{
    private readonly ColorService _colorService;
    private readonly Mock<IColorRepository> _colorRepositoryMock;
    
    public ColorServiceTests()
    {
        var config = new MapperConfiguration(config =>
        {
            config.AddProfile(new ColorProfile());
        });
        
        var mapper = config.CreateMapper();
        _colorRepositoryMock = new Mock<IColorRepository>();
        _colorService = new ColorService(_colorRepositoryMock.Object, mapper);
    }
    
    [Fact]
    public async Task GetAllAsync_ReturnsAllActiveColors_WhenIncludeInactivesIsFalse()
    {
        var colors = new List<Color>
        {
            new Color { Id = 1, Name = "Color1", RgbHex = "#FFFFFF", IsActive = true },
            new Color { Id = 2, Name = "Color2", RgbHex = "#FFFFEE", IsActive = true },
            new Color { Id = 3, Name = "Color3", RgbHex = "#EEFFFF", IsActive = false }
        };
        _colorRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Color, bool>>>(), null, null, false, 0, 0))
            .ReturnsAsync(colors.Where(c => c.IsActive).ToList());

        var result = await _colorService.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.All(result, c => Assert.True(c.IsActive));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllColors_WhenIncludeInactivesIsTrue()
    {
        var colors = new List<Color>
        {
            new Color { Id = 1, Name = "Color1", RgbHex = "#FFFFFF", IsActive = true },
            new Color { Id = 2, Name = "Color2", RgbHex = "#FFFFEE", IsActive = true },
            new Color { Id = 3, Name = "Color3", RgbHex = "#EEFFFF", IsActive = false }
        };
        _colorRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Color, bool>>>(), null, null, false, 0, 0))
            .ReturnsAsync(colors);

        var result = await _colorService.GetAllAsync(true);

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsColor_WhenColorExists()
    {
        var color = new Color { Id = 1, Name = "Color1", RgbHex = "#FFFFFF", IsActive = true };
        _colorRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Color, bool>>>(), null))
            .ReturnsAsync(color);

        var result = await _colorService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenColorDoesNotExist()
    {
        _colorRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Color, bool>>>(), null))
            .ReturnsAsync((Color?)null);

        var result = await _colorService.GetByIdAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task ExistsByIdAsync_ReturnsTrue_WhenColorExists()
    {
        _colorRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Color, bool>>>()))
            .ReturnsAsync(true);

        var result = await _colorService.ExistsByIdAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsByIdAsync_ReturnsFalse_WhenColorDoesNotExist()
    {
        _colorRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Color, bool>>>()))
            .ReturnsAsync(false);

        var result = await _colorService.ExistsByIdAsync(1);

        Assert.False(result);
    }

    [Fact]
    public async Task AddAsync_ReturnsSuccessMessage_WhenColorIsAdded()
    {
        var request = new ColorRequest { Name = "NewColor", RgbHex = "#FFFFFF" };
        _colorRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Color, bool>>>(), null))
            .ReturnsAsync((Color?)null);
        _colorRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Color>()))
            .ReturnsAsync(true);

        var result = await _colorService.AddAsync(request, 1);

        Assert.True(result.Success);
        Assert.Equal("Color agregado correctamente", result.Message);
    }

    [Fact]
    public async Task AddAsync_ReturnsErrorMessage_WhenColorAlreadyExists()
    {
        var request = new ColorRequest { Name = "ExistingColor", RgbHex = "#FFFFFF" };
        var existingColor = new Color { Id = 1, Name = "ExistingColor", RgbHex = "#FFFFFF", IsActive = true };
        _colorRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Color, bool>>>(), null))
            .ReturnsAsync(existingColor);

        var result = await _colorService.AddAsync(request, 1);

        Assert.False(result.Success);
        Assert.Equal("Ya existe un color con el nombre \"ExistingColor\"", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsSuccessMessage_WhenColorIsUpdated()
    {
        var request = new ColorRequest { Name = "UpdatedColor", RgbHex = "#000000" };
        var color = new Color { Id = 1, Name = "OldColor", RgbHex = "#FFFFFF", IsActive = true };
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(color);
        _colorRepositoryMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Color, bool>>>(), null))
            .ReturnsAsync((Color?)null);
        _colorRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Color>()))
            .ReturnsAsync(true);

        var result = await _colorService.UpdateAsync(1, request, 1);

        Assert.True(result.Success);
        Assert.Equal("Color modificado correctamente", result.Message);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsErrorMessage_WhenColorDoesNotExist()
    {
        var request = new ColorRequest { Name = "UpdatedColor", RgbHex = "#000000" };
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Color?)null);

        var result = await _colorService.UpdateAsync(1, request, 1);

        Assert.False(result.Success);
        Assert.Equal("Color no encontrado", result.Message);
    }

    [Fact]
    public async Task DeactivateAsync_ReturnsSuccessMessage_WhenColorIsDeactivated()
    {
        var color = new Color { Id = 1, Name = "Color1", RgbHex = "#FFFFFF", IsActive = true };
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(color);
        _colorRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Color>()))
            .ReturnsAsync(true);

        var result = await _colorService.DeactivateAsync(1, 1);

        Assert.True(result.Success);
        Assert.Equal("Color eliminado correctamente", result.Message);
    }

    [Fact]
    public async Task DeactivateAsync_ReturnsErrorMessage_WhenColorDoesNotExist()
    {
        _colorRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Color?)null);

        var result = await _colorService.DeactivateAsync(1, 1);

        Assert.False(result.Success);
        Assert.Equal("Color no encontrado", result.Message);
    }
}