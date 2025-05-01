namespace common.Dtos.Request;

public record EquipmentRemainsRequest(Guid EquipmentId, Guid? WarehouseId, int Page = 1);