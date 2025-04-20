namespace common.Dtos;

public record EquipmentWriteOffDto(Guid Id, Guid EquipmentId, Guid InitiatorId, int Quantity, DateTime Date);