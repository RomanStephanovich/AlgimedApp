using Xunit;
using Moq;
using ClosedXML.Excel;
using AlgimedApp.Service.Services.Implementations;
using AlgimedApp.Service.Services.Interfaces;
using AlgimedApp.Shared.Dtos;
using System.IO;

namespace AlgimedApp.Tests.Services;

public class ExcelImportServiceTests
{
    [Fact]
    public async Task ImportModesFromExcelAsync_Should_Call_AddModeAsync_ForEachRow()
    {
        // Arrange
        var mockModeService = new Mock<IModeService>();
        var mockStepService = new Mock<IStepService>();
        var service = new ExcelImportService(mockModeService.Object, mockStepService.Object);

        var filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");

        using (var workbook = new XLWorkbook())
        {
            var sheet = workbook.Worksheets.Add("Sheet1");
            sheet.Cell(1, 1).Value = "ID";
            sheet.Cell(1, 2).Value = "Name";
            sheet.Cell(1, 3).Value = "MaxBottleNumber";
            sheet.Cell(1, 4).Value = "MaxUsedTips";

            sheet.Cell(2, 2).Value = "TestMode1";
            sheet.Cell(2, 3).Value = 10;
            sheet.Cell(2, 4).Value = 5;

            sheet.Cell(3, 2).Value = "TestMode2";
            sheet.Cell(3, 3).Value = 20;
            sheet.Cell(3, 4).Value = 7;

            workbook.SaveAs(filePath);
        }

        // Act
        await service.ImportModesFromExcelAsync(filePath);

        // Assert
        mockModeService.Verify(x => x.AddModeAsync(It.IsAny<ModeDto>()), Times.Exactly(2));
        File.Delete(filePath);
    }

    [Fact]
    public async Task ImportStepsFromExcelAsync_Should_Call_AddStepAsync_ForEachRow()
    {
        var mockModeService = new Mock<IModeService>();
        var mockStepService = new Mock<IStepService>();
        var service = new ExcelImportService(mockModeService.Object, mockStepService.Object);

        var filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");

        using (var workbook = new XLWorkbook())
        {
            var sheet = workbook.Worksheets.Add("Sheet1");
            sheet.Cell(1, 1).Value = "ModeId";
            sheet.Cell(1, 2).Value = "Timer";
            sheet.Cell(1, 3).Value = "Destination";
            sheet.Cell(1, 4).Value = "Speed";
            sheet.Cell(1, 5).Value = "Type";
            sheet.Cell(1, 6).Value = "Volume";

            sheet.Cell(2, 1).Value = 1;
            sheet.Cell(2, 2).Value = 60;
            sheet.Cell(2, 3).Value = "Well A1";
            sheet.Cell(2, 4).Value = 100;
            sheet.Cell(2, 5).Value = "Dispense";
            sheet.Cell(2, 6).Value = 10;

            workbook.SaveAs(filePath);
        }

        await service.ImportStepsFromExcelAsync(filePath);

        mockStepService.Verify(x => x.AddStepAsync(It.IsAny<StepDto>()), Times.Once);
        File.Delete(filePath);
    }
}
