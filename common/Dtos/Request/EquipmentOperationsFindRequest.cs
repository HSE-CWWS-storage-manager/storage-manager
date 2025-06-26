namespace common.Dtos.Request;

public record EquipmentOperationsFindRequest(
    DateTime? StartDate, 
    DateTime? EndDate,
    EquipmentOperationType? Type,
    Guid? WarehouseId,
    Guid? EquipmentId,
    bool WithoutReturnDate = false,
    int Page = 1
);