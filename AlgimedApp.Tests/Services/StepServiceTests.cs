using Xunit;
using AlgimedApp.Data;
using AlgimedApp.Data.Models;
using AlgimedApp.Service.Services.Implementations;
using AlgimedApp.Shared.AutoMapper;
using AlgimedApp.Shared.Dtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

namespace AlgimedApp.Tests.Services;

public class StepServiceTests
{
    private readonly AlgimedDbContext _context;
    private readonly StepService _service;
    private readonly IMapper _mapper;

    public StepServiceTests()
    {
        var options = new DbContextOptionsBuilder<AlgimedDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AlgimedDbContext(options);
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
        _service = new StepService(_context, _mapper);

        // Добавим тестовый Mode, чтобы FK не падал
        _context.Modes.Add(new Mode { ID = 1, Name = "Mode1", MaxBottleNumber = 10, MaxUsedTips = 5 });
        _context.SaveChanges();
    }

    [Fact]
    public async Task AddStep_Should_SaveToDatabase()
    {
        var step = new StepDto
        {
            ModeId = 1,
            Timer = 60,
            Destination = "Test",
            Speed = 100,
            Type = "Dispense",
            Volume = 5
        };

        await _service.AddStepAsync(step);
        var all = await _service.GetAllStepsAsync();

        all.Should().ContainSingle(s => s.Destination == "Test");
    }
}
