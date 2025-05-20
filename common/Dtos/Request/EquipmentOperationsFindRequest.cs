namespace common.Dtos.Request;

public record EquipmentOperationsFindRequest(
    DateTime? StartDate, 
    DateTime? EndDate,
    EquipmentOperationType? Type,
    int Page = 1
);