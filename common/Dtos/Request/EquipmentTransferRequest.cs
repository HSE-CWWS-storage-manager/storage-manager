using System.ComponentModel.DataAnnotations;

namespace common.Dtos.Request;

public record EquipmentTransferRequest(
    [Required(ErrorMessage = "WarehouseId is required")]
    Guid WarehouseId, 
    [Required(ErrorMessage = "EquipmentId is required")]
    Guid EquipmentId, 
    [Required(ErrorMessage = "RecipientId is required")]
    Guid RecipientId
);