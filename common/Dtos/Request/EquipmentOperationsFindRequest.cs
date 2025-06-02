namespace common.Dtos.Request;

public record EquipmentOperationsFindRequest(
    DateTime? StartDate, 
    DateTime? EndDate,
    EquipmentOperationType? Type,
    Guid? WarehouseId,
    Guid? EquipmentId,
    int Page = 1
);