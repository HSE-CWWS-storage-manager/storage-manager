using common.Dtos.Request;

namespace backend.Services;

public interface IEquipmentCardService
{

    byte[] GenerateEquipmentCard(EquipmentFindRequest request, Guid studentId);
}