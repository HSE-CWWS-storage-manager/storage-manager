using backend.Services;
using backend.Utils;
using common.Dtos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace backend.Controllers;

/// <summary>
/// Контроллер для генерации карточек оборудования
/// </summary>
/// <param name="cardService"></param>
[Route("[controller]")]
[ApiController]
[Authorize(Policy = StringConstants.EditorPolicy)]
public class EquipmentCardController(IEquipmentCardService cardService) : ControllerBase
{

    private const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    /// <summary>
    /// Сгенерировать карточку для заданного оборудования
    /// </summary>
    /// <param name="request"></param>
    /// <param name="studentId"></param>
    /// <returns></returns>
    [HttpGet]
    [Produces(ExcelContentType)]
    public IActionResult Generate([FromQuery] EquipmentFindRequest request, [FromQuery] Guid studentId)
    {
        if (!ModelState.IsValid)
            return Problem();
        
        var cd = new ContentDispositionHeaderValue("attachment")
        {
            FileNameStar = $"{Guid.NewGuid()}.xlsx"
        };
        
        Response.Headers.Append(HeaderNames.ContentDisposition, cd.ToString());

        return File(cardService.GenerateEquipmentCard(request, studentId), ExcelContentType);
    }
}