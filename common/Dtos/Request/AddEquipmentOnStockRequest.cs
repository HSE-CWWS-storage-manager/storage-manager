using System.ComponentModel.DataAnnotations;

namespace common.Dtos.Request;

public record AddEquipmentOnStockRequest(
    [Required(ErrorMessage = "EquipmentId is required")]
    Guid EquipmentId, 
    [Required(ErrorMessage = "WarehouseId is required")]
    Guid WarehouseId, 
    [Required(ErrorMessage = "AddCount is required")]
    int AddCount
);