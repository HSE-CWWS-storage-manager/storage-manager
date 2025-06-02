namespace common.Dtos.Request;

public record EquipmentReportRequest(Guid WarehouseId, int MaxCount = 10);