namespace common.Dtos;

public enum EquipmentOperationType : ushort
{
    WriteOff = 1,
    Transfer
}

public record EquipmentOperationDto(
    Guid Id,
    Guid WarehouseId,
    Guid EquipmentId,
    Guid InitiatorId,
    DateTime Date,
    EquipmentOperationType Type,
    Guid? RecipientId = null,
    int? Quantity = null
);