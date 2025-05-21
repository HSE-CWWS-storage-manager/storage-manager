using backend.Services;
using backend.Utils;
using common.Dtos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(Policy = StringConstants.EditorPolicy)]
public class EquipmentController(IEquipmentService equipmentService) : ControllerBase
{
    
    [HttpPost]
    public async Task<IActionResult> Add(AddEquipmentRequest request)
    {
        if (!ModelState.IsValid)
            return Problem();
        
        return Ok(await equipmentService.AddEquipment(request));
    }

    [HttpGet]
    public IActionResult Find([FromQuery] EquipmentFindRequest request)
    {
        if (!ModelState.IsValid)
            return Problem();

        return Ok(equipmentService.FindEquipment(request));
    }
    
    [HttpPatch]
    public async Task<IActionResult> Update(UpdateEquipmentRequest request)
    {
        if (!ModelState.IsValid)
            return Problem();

        return Ok(await equipmentService.UpdateEquipment(request));
    }
    
    [HttpPost("Delete")]
    public async Task<IActionResult> Delete(DeleteEquipmentRequest request)
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