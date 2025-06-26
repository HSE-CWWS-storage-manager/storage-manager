using backend.Services;
using backend.Utils;
using common.Dtos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;


/// <summary>
/// Контроллер с методами проведения различных операций с оборудованием
/// </summary>
/// <param name="accountService"></param>
/// <param name="equipmentOperationService"></param>
[Route("[controller]")]
[ApiController]
[Authorize(Policy = StringConstants.EditorPolicy)]
[Produces("application/json")]
public class EquipmentOperationController(IAccountService accountService, IEquipmentOperationService equipmentOperationService) : ControllerBase
{

    /// <summary>
    /// Передача оборудования студенту со склада
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> Transfer(EquipmentTransferRequest request)
    {
        return !ModelState.IsValid ? 
            Problem() :
            Ok(await equipmentOperationService.Transfer(await accountService.GetUserFromPrincipal(User), request));
    }
    
    /// <summary>
    /// Списание оборудования со склада
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> WriteOff(EquipmentWriteOffRequest request)
    {
        return !ModelState.IsValid ? 
            Problem() :
            Ok(await equipmentOperationService.WriteOff(await accountService.GetUserFromPrincipal(User), request));
    }
    
    /// <summary>
    /// Возврат оборудования
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Return(EquipmentReturnRequest request)
    {
        return !ModelState.IsValid ? 
            Problem() :
            Ok(await equipmentOperationService.Return(request));
    }
    
    /// <summary>
    /// Поиск операций в заданном диапазоне дат
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Find([FromQuery] EquipmentOperationsFindRequest request)
    {
        return !ModelState.IsValid ? 
            Problem() :
            Ok(equipmentOperationService.Find(request));
    }
}