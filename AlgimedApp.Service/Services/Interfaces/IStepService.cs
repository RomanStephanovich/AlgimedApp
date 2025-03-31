using AlgimedApp.Data.Models;
using AlgimedApp.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgimedApp.Service.Services.Interfaces
{
    public interface IStepService
    {
        Task<List<StepDto>> GetAllStepsAsync();
        Task<List<StepDto>> GetStepsByModeIdAsync(int modeId);
        Task<StepDto> GetStepByIdAsync(int id);
        Task AddStepAsync(StepDto step);
        Task UpdateStepAsync(int id, StepDto step);
        Task DeleteStepAsync(int id);
    }
}
