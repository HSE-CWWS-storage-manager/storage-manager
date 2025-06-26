using System.ComponentModel.DataAnnotations;

namespace common.Dtos.Request;

public record AddEquipmentRequest(
    string? Model, 
    [Required(ErrorMessage = "Name is required")]
    string Name, 
    string? SerialNumber, 
    string? InventoryNumber,
    string? BalanceCost,
    DateTime? AcceptDate
);