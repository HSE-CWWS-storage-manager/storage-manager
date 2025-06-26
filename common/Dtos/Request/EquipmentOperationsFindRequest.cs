namespace common.Dtos.Request;

public record EquipmentOperationsFindRequest(
    DateTime? StartDate, 
    DateTime? EndDate,
    EquipmentOperationType? Type,
    Guid? WarehouseId,
    Guid? EquipmentId,
    bool? WithoutReturnDate,
    int Page = 1
);