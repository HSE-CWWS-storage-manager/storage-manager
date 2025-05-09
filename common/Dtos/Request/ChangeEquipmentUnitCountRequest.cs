using System.ComponentModel.DataAnnotations;

namespace common.Dtos.Request;

public record ChangeEquipmentUnitCountRequest(
    [Required(ErrorMessage = "EquipmentId is required")]
    Guid EquipmentId, 
    [Required(ErrorMessage = "WarehouseId is required")]
    Guid WarehouseId, 
    [Range(0, Int32.MaxValue)]
    [Required(ErrorMessage = "Count is required")]
    int Count
);