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
    
    [HttpPost]
    public async Task<IActionResult> Create(WarehouseCreateRequest request)
    {
        return !ModelState.IsValid ? Problem() : Ok(await service.CreateWarehouse(request));
    }
    
    [HttpPut]
    public async Task<IActionResult> Update(WarehouseUpdateRequest request)
    {
        return !ModelState.IsValid ? Problem() : Ok(await service.UpdateWarehouse(request));
    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(WarehouseDeleteRequest request)
    {
        return !ModelState.IsValid ? Problem() : Ok(await service.DeleteWarehouse(request));
    }
    
    [HttpGet]
    public IActionResult List([FromQuery] WarehouseListRequest request)
    {
        return !ModelState.IsValid ? Problem() : Ok(service.ListWarehouses(request));
    }
}