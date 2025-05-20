using backend.Services;
using backend.Utils;
using common.Dtos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(Policy = StringConstants.EditorPolicy)]
public class EquipmentOperationController(IAccountService accountService, IEquipmentOperationService equipmentOperationService) : ControllerBase
{

    [HttpPut]
    public async Task<IActionResult> Transfer(EquipmentTransferRequest request)
    {
        return !ModelState.IsValid ? 
            Problem() :
            Ok(await equipmentOperationService.Transfer(await accountService.GetUserFromPrincipal(User), request));
    }
    
    [HttpDelete]
    public async Task<IActionResult> WriteOff(EquipmentWriteOffRequest request)
    {
        return !ModelState.IsValid ? 
            Problem() :
            Ok(await equipmentOperationService.WriteOff(await accountService.GetUserFromPrincipal(User), request));
    }
    
    [HttpGet]
    public IActionResult Find([FromQuery] EquipmentOperationsFindRequest request)
    {
        return !ModelState.IsValid ? 
            Problem() :
            Ok(equipmentOperationService.Find(request));
    }
}