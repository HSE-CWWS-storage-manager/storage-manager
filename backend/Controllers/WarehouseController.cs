using backend.Services;
using backend.Utils;
using common.Dtos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(Policy = StringConstants.EditorPolicy)]
public class WarehouseController(IWarehouseService service) : ControllerBase
{

    [HttpGet("GetRemains")]
    public IActionResult GetRemains([FromQuery] EquipmentRemainsRequest request)
    {
        return !ModelState.IsValid ? Problem() : Ok(service.GetRemains(request));
    }
    
    [HttpPost("AddOnStock")]
    public async Task<IActionResult> AddOnStock(ChangeEquipmentUnitCountRequest request)
    {
        return !ModelState.IsValid ? Problem() : Ok(await service.AddEquipmentOnStock(request));
    }
    
    [HttpPost("ExtractFromStock")]
    public async Task<IActionResult> ExtractFromStock(ChangeEquipmentUnitCountRequest request)
    {
        return !ModelState.IsValid ? Problem() : Ok(await service.ExtractEquipmentFromStock(request));
    }
    
    [HttpPost("AddOnLoan")]
    public async Task<IActionResult> AddOnLoan(ChangeEquipmentUnitCountRequest request)
    {
        return !ModelState.IsValid ? Problem() : Ok(await service.AddEquipmentOnLoan(request));
    }
    
    [HttpPost("ExtractFromLoan")]
    public async Task<IActionResult> ExtractFromLoan(ChangeEquipmentUnitCountRequest request)
    {
        return !ModelState.IsValid ? Problem() : Ok(await service.ExtractEquipmentFromLoan(request));
    }
}