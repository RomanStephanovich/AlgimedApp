using Xunit;
using AlgimedApp.Data;
using AlgimedApp.Service.Services.Implementations;
using AlgimedApp.Shared.AutoMapper;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using AlgimedApp.Shared.Dtos;

namespace AlgimedApp.Tests.Services;

public class ModeServiceTests
{
    private readonly AlgimedDbContext _context;
    private readonly IMapper _mapper;
    private readonly ModeService _service;

    public ModeServiceTests()
    {
        var options = new DbContextOptionsBuilder<AlgimedDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // каждый тест = чистая БД
            .Options;

        _context = new AlgimedDbContext(options);

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        _mapper = mapperConfig.CreateMapper();
        _service = new ModeService(_context, _mapper);
    }

    [Fact]
    public async Task AddMode_Should_SaveToDatabase()
    {
        // Arrange
        var dto = new ModeDto
        {
            Name = "TestMode",
            MaxBottleNumber = 10,
            MaxUsedTips = 5
        };

        // Act
        await _service.AddModeAsync(dto);

        // Assert
        var allModes = await _service.GetAllModesAsync();
        allModes.Should().ContainSingle(m => m.Name == "TestMode");
    }
}
