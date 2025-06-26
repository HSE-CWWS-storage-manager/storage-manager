using common.Dtos;
using common.Dtos.Request;
using common.Dtos.Response;
using Microsoft.AspNetCore.Identity;

namespace backend.Services;

public interface IEquipmentOperationService
{
    Task<EquipmentOperationDto> Transfer(IdentityUser initiator, EquipmentTransferRequest request);
    Task<EquipmentOperationDto> WriteOff(IdentityUser initiator, EquipmentWriteOffRequest request);
    Task<EquipmentOperationDto> Return(EquipmentReturnRequest request);
    EquipmentOperationsFindResponse Find(EquipmentOperationsFindRequest request);
    
}