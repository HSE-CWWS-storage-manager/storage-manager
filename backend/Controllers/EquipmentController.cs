using backend.Services;
using backend.Utils;
using common.Dtos;
using common.Dtos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

/// <summary>
/// Контроллер для работы с видами оборудования
/// </summary>
/// <param name="equipmentService"></param>
[Route("[controller]")]
[ApiController]
[Authorize(Policy = StringConstants.EditorPolicy)]
[Produces("application/json")]
public class EquipmentController(IEquipmentService equipmentService) : ControllerBase
{
    
    /// <summary>
    /// Добавить новый вид оборудования
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200">Возвращает информацию о новом виде оборудования</response>
    [HttpPost]
    [ProducesResponseType(typeof(EquipmentDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Add(AddEquipmentRequest request)
    {
        if (!ModelState.IsValid)
            return Problem();
        
        return Ok(await equipmentService.AddEquipment(request));
    }

    /// <summary>
    /// Поиск оборудования по различным параметрам
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Find([FromQuery] EquipmentFindRequest request)
    {
        if (!ModelState.IsValid)
            return Problem();

        return Ok(equipmentService.FindEquipment(request));
    }
    
    /// <summary>
    /// Изменение существующего вида оборудования
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPatch]
    public async Task<IActionResult> Update(UpdateEquipmentRequest request)
    {
        if (!ModelState.IsValid)
            return Problem();

        return Ok(await equipmentService.UpdateEquipment(request));
    }
    
    
    /// <summary>
    /// Удаление существующего вида оборудования
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] DeleteEquipmentRequest request)
    {
        if (!ModelState.IsValid)
            return Problem();

        await equipmentService.DeleteEquipment(request);
        
        return Ok(new
        {
            Status = "ok"
        });
    }
}