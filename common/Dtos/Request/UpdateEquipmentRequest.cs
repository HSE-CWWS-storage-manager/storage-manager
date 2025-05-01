namespace common.Dtos.Request;

public record UpdateEquipmentRequest(
    Guid EquipmentId,
    string? Model,
    string? Name,
    string? SerialNumber,
    string? InventoryNumber
);