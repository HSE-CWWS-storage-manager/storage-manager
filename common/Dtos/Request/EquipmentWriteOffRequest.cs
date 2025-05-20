using System.ComponentModel.DataAnnotations;

namespace common.Dtos.Request;

public record EquipmentWriteOffRequest(
    [Required(ErrorMessage = "WarehouseId is required")]
    Guid WarehouseId, 
    [Required(ErrorMessage = "EquipmentId is required")]
    Guid EquipmentId, 
    [Required(ErrorMessage = "Quantity is required")]
    int Quantity
);