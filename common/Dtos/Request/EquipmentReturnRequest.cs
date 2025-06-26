namespace common.Dtos.Request;

public record EquipmentReturnRequest(
    Guid OperationId, 
    DateTime? ReturnDate = null
);