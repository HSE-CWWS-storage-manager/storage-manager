using common.Dtos;
using common.Dtos.Request;
using common.Dtos.Response;

namespace backend.Services;

public interface IEquipmentService
{
    Task<EquipmentDto> AddEquipment(AddEquipmentRequest request);
    Task<EquipmentDto> UpdateEquipment(UpdateEquipmentRequest request);
    EquipmentFindResponse FindEquipment(EquipmentFindRequest request);
    Task DeleteEquipment(DeleteEquipmentRequest request);
}