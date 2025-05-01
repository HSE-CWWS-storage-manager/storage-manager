using System.ComponentModel.DataAnnotations;

namespace common.Dtos.Request;

public record AddEquipmentRequest(
    [Required(ErrorMessage = "Model is required")]
    string Model, 
    [Required(ErrorMessage = "Name is required")]
    string Name, 
    [Required(ErrorMessage = "SerialNumber is required")]
    string SerialNumber, 
    [Required(ErrorMessage = "InventoryNumber is required")]
    string InventoryNumber
);