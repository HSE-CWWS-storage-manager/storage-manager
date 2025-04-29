using common.Dtos.Request;
using common.Dtos.Response;

namespace backend.Services;

public interface IWarehouseService
{
    EquipmentRemainsResponse GetRemains(EquipmentRemainsRequest request);
    EquipmentRemainsResponse AddEquipmentOnStock(Guid equipmentId, Guid warehouseId, int addCount);
    EquipmentRemainsResponse ExtractEquipmentFromStock(Guid equipmentId, Guid warehouseId, int extractCount);
    EquipmentRemainsResponse AddEquipmentOnLoan(Guid equipmentId, Guid warehouseId, int addCount);
    EquipmentRemainsResponse ExtractEquipmentFromLoan(Guid equipmentId, Guid warehouseId, int extractCount);
}