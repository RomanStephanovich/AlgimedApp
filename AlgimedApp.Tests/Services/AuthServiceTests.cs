using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AlgimedApp.Data;
using AlgimedApp.Service.Services.Implementations;
using AlgimedApp.Shared.AutoMapper;
using AlgimedApp.Shared.Dtos;
using System.Threading.Tasks;

namespace AlgimedApp.Tests.Services;

public class AuthServiceTests
{
    private readonly AlgimedDbContext _context;
    private readonly IMapper _mapper;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        var options = new DbContextOptionsBuilder<AlgimedDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AlgimedDbContext(options);

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        _mapper = config.CreateMapper();
        _service = new AuthService(_context, _mapper);
    }

    [Fact]
    public async Task RegisterAsync_Should_Register_User_And_Hash_Password()
    {
        // Act
        var result = await _service.RegisterAsync(new LoginDto
        {
            Username = "testuser",
            PasswordHash = "Test123"
        });

        result.Should().Be("User registered successfully");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == "testuser");
        user.Should().NotBeNull();
        user!.PasswordHash.Should().NotBe("Test123"); 
    }

    [Fact]
    public async Task RegisterAsync_Should_Return_Error_When_Username_Exists()
    {
        await _service.RegisterAsync(new LoginDto
        {
            Username = "admin",
            PasswordHash = "SomePass1"
        });

        var result = await _service.RegisterAsync(new LoginDto
        {
            Username = "admin",
            PasswordHash = "OtherPass2"
        });

        result.Should().Be("Username already exists");
    }

    [Fact]
    public async Task RegisterAsync_Should_Reject_Weak_Password()
    {
        var result = await _service.RegisterAsync(new LoginDto
        {
            Username = "weak",
            PasswordHash = "abc" 
        });

        result.Should().Be("Password must contain at least 6 characters, including letters and digits");
    }

    [Fact]
    public async Task AuthenticateAsync_Should_Return_Success_When_Credentials_Valid()
    {
        await _service.RegisterAsync(new LoginDto
        {
            Username = "john",
            PasswordHash = "StrongPass1"
        });

        var result = await _service.AuthenticateAsync(new LoginDto
        {
            Username = "john",
            PasswordHash = "StrongPass1"
        });

        result.Should().Be("Login successful");
    }

    [Fact]
    public async Task AuthenticateAsync_Should_Return_Error_When_Password_Invalid()
    {
        await _service.RegisterAsync(new LoginDto
        {
            Username = "jack",
            PasswordHash = "Correct123"
        });

        var result = await _service.AuthenticateAsync(new LoginDto
        {
            Username = "jack",
            PasswordHash = "WrongPass"
        });

        result.Should().Be("Invalid username or password");
    }
}

