namespace common.Dtos.Response;

public record EquipmentRemainData(Guid WarehouseId, int OnStock, int OnLoan);

public record EquipmentRemainsResponse(List<EquipmentRemainData> Remains);