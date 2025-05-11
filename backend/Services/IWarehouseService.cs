using common.Dtos;
using common.Dtos.Request;
using common.Dtos.Response;

namespace backend.Services;

public interface IWarehouseService
{
    EquipmentRemainsResponse GetRemains(EquipmentRemainsRequest request);
    Task<EquipmentRemainsResponse> AddEquipmentOnStock(ChangeEquipmentUnitCountRequest request);
    Task<EquipmentRemainsResponse> ExtractEquipmentFromStock(ChangeEquipmentUnitCountRequest request);
    Task<EquipmentRemainsResponse> AddEquipmentOnLoan(ChangeEquipmentUnitCountRequest request);
    Task<EquipmentRemainsResponse> ExtractEquipmentFromLoan(ChangeEquipmentUnitCountRequest request);
    
    Task<WarehouseDto> CreateWarehouse(WarehouseCreateRequest request);
    Task<WarehouseDto> UpdateWarehouse(WarehouseUpdateRequest request);
    Task<WarehouseDto> DeleteWarehouse(WarehouseDeleteRequest request);

    List<WarehouseDto> ListWarehouses(WarehouseListRequest request);
}