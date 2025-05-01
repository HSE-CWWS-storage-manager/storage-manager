namespace common.Dtos.Request;

public record AddEquipmentOnStockRequest(Guid EquipmentId, Guid WarehouseId, int AddCount);