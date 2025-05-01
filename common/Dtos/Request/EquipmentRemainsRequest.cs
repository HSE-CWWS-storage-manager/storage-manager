namespace common.Dtos.Request;

public record EquipmentRemainsRequest(Guid EquipmentId, Guid? WarehouseId);