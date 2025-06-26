using System.ComponentModel.DataAnnotations;

namespace common.Dtos.Request;

public record UpdateEquipmentRequest(
    [Required(ErrorMessage = "EquipmentId is required")]
    Guid EquipmentId,
    string? Model,
    string? Name,
    string? SerialNumber,
    string? InventoryNumber,
    string? BalanceCost,
    DateTime? AcceptDate
);