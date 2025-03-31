using AlgimedApp.Data;
using AlgimedApp.Data.Models;
using AlgimedApp.Service.Services.Interfaces;
using AlgimedApp.Shared.Dtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AlgimedApp.Service.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly AlgimedDbContext _context;
        private readonly IMapper _mapper;

        public AuthService(AlgimedDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> AuthenticateAsync(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.PasswordHash, user.PasswordHash))
            {
                return "Invalid username or password";
            }

            return "Login successful";
        }

        public async Task<string> RegisterAsync(LoginDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return "Username already exists";

            if (!IsValidPassword(dto.PasswordHash))
                return "Password must contain at least 6 characters, including letters and digits";

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = hashedPassword
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "User registered successfully";
        }

        private bool IsValidPassword(string password)
        {
            return password.Length >= 6 &&
                   password.Any(char.IsLetter) &&
                   password.Any(char.IsDigit);
        }
    }
}