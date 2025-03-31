using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgimedApp.Service.Services.Interfaces
{
    public interface IExcelImportService
    {
        Task ImportModesFromExcelAsync(string filePath);
        Task ImportStepsFromExcelAsync(string filePath);
    }
}
