using AlgimedApp.Data;
using AlgimedApp.Data.Models;
using AlgimedApp.Service.Services.Interfaces;
using AlgimedApp.Shared.Dtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgimedApp.Service.Services.Implementations
{
    public class ModeService : IModeService
    {
        private readonly AlgimedDbContext _context;
        private readonly IMapper _mapper;

        public ModeService(AlgimedDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ModeDto>> GetAllModesAsync()
        {
            var modes = await _context.Modes.ToListAsync();
            return _mapper.Map<List<ModeDto>>(modes);
        }

        public async Task<ModeDto> GetModeByIdAsync(int id)
        {
            var mode = await _context.Modes.FindAsync(id);
            return _mapper.Map<ModeDto>(mode);
        }

        public async Task AddModeAsync(ModeDto modeDto)
        {
            var mode = _mapper.Map<Mode>(modeDto);
            _context.Modes.Add(mode);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateModeAsync(int id, ModeDto modeDto)
        {
            var mode = await _context.Modes.FindAsync(id);
            if (mode != null)
            {
                _mapper.Map(modeDto, mode);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteModeAsync(int id)
        {
            var mode = await _context.Modes.FindAsync(id);
            if (mode != null)
            {
                _context.Modes.Remove(mode);
                await _context.SaveChangesAsync();
            }
        }
    }
}