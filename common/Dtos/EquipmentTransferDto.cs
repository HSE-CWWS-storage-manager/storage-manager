namespace common.Dtos;

public record EquipmentTransferDto(Guid Id, Guid EquipmentId, Guid RecipientId, Guid InitiatorId, DateTime IssueDate);