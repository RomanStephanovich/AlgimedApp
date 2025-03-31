using AlgimedApp.Data.Models;
using AlgimedApp.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgimedApp.Service.Services.Interfaces
{
    public interface IModeService
    {
        Task<List<ModeDto>> GetAllModesAsync();
        Task<ModeDto> GetModeByIdAsync(int id);
        Task AddModeAsync(ModeDto mode);
        Task UpdateModeAsync(int id, ModeDto mode);
        Task DeleteModeAsync(int id);
    }
}
