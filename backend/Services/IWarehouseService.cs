using common.Dtos.Request;
using common.Dtos.Response;

namespace backend.Services;

public interface IWarehouseService
{
    Task<EquipmentRemainsResponse> GetRemains(EquipmentRemainsRequest request);
    Task<EquipmentRemainsResponse> AddEquipmentOnStock(Guid equipmentId, Guid warehouseId, int addCount);
    Task<EquipmentRemainsResponse> ExtractEquipmentFromStock(Guid equipmentId, Guid warehouseId, int extractCount);
    Task<EquipmentRemainsResponse> AddEquipmentOnLoan(Guid equipmentId, Guid warehouseId, int addCount);
    Task<EquipmentRemainsResponse> ExtractEquipmentFromLoan(Guid equipmentId, Guid warehouseId, int extractCount);
}