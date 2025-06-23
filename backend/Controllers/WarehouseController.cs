using backend.Services;
using backend.Utils;
using common.Dtos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

/// <summary>
/// Контроллер управления складами
/// </summary>
/// <param name="service"></param>
[Route("[controller]")]
[ApiController]
[Authorize(Policy = StringConstants.EditorPolicy)]
[Produces("application/json")]
public class WarehouseController(IWarehouseService service) : ControllerBase
{

    /// <summary>
    /// Получить остатки оборудования на складе и на руках у студентов
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet("GetRemains")]
    public IActionResult GetRemains([FromQuery] EquipmentRemainsRequest request)
    {
        return !ModelState.IsValid ? Problem() : Ok(service.GetRemains(request));
    }
    
    /// <summary>
    /// Создать запись о складе
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create(WarehouseCreateRequest request)
    {
        return !ModelState.IsValid ? Problem() : Ok(await service.CreateWarehouse(request));
    }
    
    /// <summary>
    /// Обновить информацию о складе
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> Update(WarehouseUpdateRequest request)
    {
        return !ModelState.IsValid ? Problem() : Ok(await service.UpdateWarehouse(request));
    }
    
    /// <summary>
    /// Удалить запись о складе
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] WarehouseDeleteRequest request)
    {
        return !ModelState.IsValid ? Problem() : Ok(await service.DeleteWarehouse(request));
    }
    
    /// <summary>
    /// Получить список складов
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult List([FromQuery] WarehouseListRequest request)
    {
        return !ModelState.IsValid ? Problem() : Ok(service.ListWarehouses(request));
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