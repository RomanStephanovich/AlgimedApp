using AlgimedApp.Data.Models;
using AlgimedApp.Data;
using AlgimedApp.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AlgimedApp.Shared.Dtos;
using AutoMapper;
using System.Windows;

namespace AlgimedApp.Service.Services.Implementations
{
    public class StepService : IStepService
    {
        private readonly AlgimedDbContext _context;
        private readonly IMapper _mapper;

        public StepService(AlgimedDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<StepDto>> GetAllStepsAsync()
        {
            var steps = await _context.Steps.ToListAsync();
            return _mapper.Map<List<StepDto>>(steps);
        }

        public async Task<List<StepDto>> GetStepsByModeIdAsync(int modeId)
        {
            var steps = await _context.Steps
                .Where(s => s.ModeId == modeId)
                .ToListAsync();

            return _mapper.Map<List<StepDto>>(steps);
        }

        public async Task<StepDto> GetStepByIdAsync(int id)
        {
            var step = await _context.Steps.FindAsync(id);
            return _mapper.Map<StepDto>(step);
        }

        public async Task AddStepAsync(StepDto dto)
        {
            bool modeExists = await _context.Modes.AnyAsync(m => m.ID == dto.ModeId);
            if (!modeExists)
                throw new InvalidOperationException($"Mode with ID {dto.ModeId} does not exist.");

            try
            {
                var step = _mapper.Map<Step>(dto);
                _context.Steps.Add(step);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save step.", ex);
            }
        }

        public async Task UpdateStepAsync(int id, StepDto dto)
        {
            var step = await _context.Steps.FindAsync(id);
            if (step != null)
            {
                // Безопасный ручной маппинг
                step.Timer = dto.Timer;
                step.Destination = dto.Destination;
                step.Speed = dto.Speed;
                step.Type = dto.Type;
                step.Volume = dto.Volume;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteStepAsync(int id)
        {
            var step = await _context.Steps.FindAsync(id);
            if (step != null)
            {
                _context.Steps.Remove(step);
                await _context.SaveChangesAsync();
            }
        }
    }
}