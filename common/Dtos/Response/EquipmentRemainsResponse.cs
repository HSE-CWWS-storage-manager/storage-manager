namespace common.Dtos.Response;

public record EquipmentRemainData(Guid WarehouseId, int OnStock, int OnLoan);

public record EquipmentRemainDto(Guid EquipmentId, List<EquipmentRemainData> Remains);

public record EquipmentRemainsResponse(List<EquipmentRemainDto> Remains);