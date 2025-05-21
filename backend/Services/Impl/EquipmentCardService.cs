using System.Net;
using backend.Exceptions;
using common.Dtos.Request;
using common.Dtos.Response;
using NanoXLSX;

namespace backend.Services.Impl;

public class EquipmentCardService(IEquipmentService equipmentService) : IEquipmentCardService
{

    private const string StudentFormat = "{0}, студент, группа {1}";
    
    private const string EquipmentCardFileName = "equipment_card.xlsx";
    private const string CellName = "H6";
    private const string CellModel = "H8";
    private const string CellSerial = "H10";
    private const string CellInventoryNumber = "H12";
    private const string CellStudent = "H14";
    private const string CellDate = "N18";
    
    public byte[] GenerateEquipmentCard(EquipmentFindRequest request)
    {
        var equipments = equipmentService.FindEquipment(request);
        var equipment = equipments.Equipments.FirstOrDefault() ?? throw new HttpResponseException(
            (int) HttpStatusCode.NotFound,
            new HttpErrorMessageResponse("Equipment not found")
        );
        
        var workbook = Workbook.Load(EquipmentCardFileName);

        workbook.CurrentWorksheet.Cells[CellName].Value = equipment.Name;
        workbook.CurrentWorksheet.Cells[CellModel].Value = equipment.Model;
        workbook.CurrentWorksheet.Cells[CellSerial].Value = equipment.SerialNumber;
        workbook.CurrentWorksheet.Cells[CellInventoryNumber].Value = equipment.InventoryNumber;
        workbook.CurrentWorksheet.Cells[CellDate].Value = DateTime.Now;

        using var memoryStream = new MemoryStream();
        workbook.SaveAsStream(memoryStream, true);
        
        return memoryStream.ToArray();
    }
}