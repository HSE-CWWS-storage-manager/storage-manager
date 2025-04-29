using common.Dtos;
using common.Dtos.Request;

namespace backend.Services;

public interface IEquipmentService
{
    EquipmentDto AddEquipment(AddEquipmentRequest request);
    EquipmentDto UpdateEquipment(UpdateEquipmentRequest request);
    void DeleteEquipment(DeleteEquipmentRequest request);
}