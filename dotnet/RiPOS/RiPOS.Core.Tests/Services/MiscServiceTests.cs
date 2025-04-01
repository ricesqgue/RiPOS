using AutoMapper;
using Moq;
using RiPOS.Core.MapProfiles;
using RiPOS.Core.Services;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;

namespace RiPOS.Core.Tests.Services;

public class MiscServiceTests
{
    private readonly MiscService _miscService;
    private readonly Mock<IMiscRepository> _miscRepositoryMock;

    public MiscServiceTests()
    {
        var config = new MapperConfiguration(config =>
        {
            config.AddProfile(new MiscProfile());
        });
        
        var mapper = config.CreateMapper();
        _miscRepositoryMock = new Mock<IMiscRepository>();
        _miscService = new MiscService(_miscRepositoryMock.Object, mapper);
    }
    
    [Fact]
    public async Task GetAllCountryStatesAsync_ReturnsAllCountryStates()
    {
        var countryStates = new List<CountryState>
        {
            new CountryState { Id = 1, Name = "State1", ShortName = "S1" },
            new CountryState { Id = 2, Name = "State2", ShortName = "S1" }
        };
        _miscRepositoryMock.Setup(repo => repo.GetAllCountryStatesAsync())
            .ReturnsAsync(countryStates);

        var result = await _miscService.GetAllCountryStatesAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal("State1", result.First().Name);
        Assert.Equal("State2", result.Last().Name);
    }

    [Fact]
    public async Task GetAllCountryStatesAsync_ReturnsEmptyList_WhenNoCountryStatesExist()
    {
        _miscRepositoryMock.Setup(repo => repo.GetAllCountryStatesAsync())
            .ReturnsAsync(new List<CountryState>());

        var result = await _miscService.GetAllCountryStatesAsync();

        Assert.Empty(result);
    }
}