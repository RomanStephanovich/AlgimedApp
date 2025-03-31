using AlgimedApp.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgimedApp.Service.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> AuthenticateAsync(LoginDto dto);
        Task<string> RegisterAsync(LoginDto dto);
    }
}
