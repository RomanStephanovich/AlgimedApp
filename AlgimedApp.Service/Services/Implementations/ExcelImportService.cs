
using AlgimedApp.Service.Services.Interfaces;
using AlgimedApp.Shared.Dtos;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlgimedApp.Service.Services.Implementations
{
    public class ExcelImportService : IExcelImportService
    {
        private readonly IModeService _modeService;
        private readonly IStepService _stepService;

        public ExcelImportService(IModeService modeService, IStepService stepService)
        {
            _modeService = modeService;
            _stepService = stepService;
        }

        public async Task ImportModesFromExcelAsync(string filePath)
        {
            using var workbook = new XLWorkbook(filePath);
            var worksheet = workbook.Worksheet(1);
            var rows = worksheet.RowsUsed().Skip(1); 

            foreach (var row in rows)
            {
                try
                {
                    var dto = new ModeDto
                    {
                        Name = row.Cell(2).GetString(),                  
                        MaxBottleNumber = row.Cell(3).GetValue<int>(),   
                        MaxUsedTips = row.Cell(4).GetValue<int>()        
                    };

                    await _modeService.AddModeAsync(dto);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ImportModes] Row {row.RowNumber()}: {ex.Message}");
                }
            }
        }
        public async Task ImportStepsFromExcelAsync(string filePath)
        {
            using var workbook = new XLWorkbook(filePath);
            var worksheet = workbook.Worksheet(1);
            var rows = worksheet.RowsUsed().Skip(1);

            foreach (var row in rows)
            {
                try
                {
                    var dto = new StepDto
                    {
                        ModeId = row.Cell(1).GetValue<int>(),
                        Timer = row.Cell(2).GetValue<int>(),
                        Destination = row.Cell(3).GetString(),
                        Speed = row.Cell(4).GetValue<int>(),
                        Type = row.Cell(5).GetString(),
                        Volume = row.Cell(6).GetValue<int>()
                    };

                    await _stepService.AddStepAsync(dto);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ImportSteps] Row {row.RowNumber()}: {ex.Message}");
                }
            }
        }
    }
}
