using backend.Services;
using backend.Utils;
using common.Dtos.Request;
using common.Dtos.Response;
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
    private const string JsonContentType = "application/json";

    /// <summary>
    /// Сгенерировать карточку для заданного оборудования
    /// </summary>
    /// <param name="request"></param>
    /// <param name="studentId"></param>
    /// <returns></returns>
    /// <response code="200">Возвращает карточку оборудования в формате XLSX</response>
    /// <response code="400">Возвращает детализацию ошибки в запросе</response>
    /// <response code="404">Возвращает детализацию ошибки поиска (например, студент или оборудование с указанным идентификатором не существует)</response>
    /// <response code="500">Произошла непредвиденная ошибка</response>
    [HttpGet]
    [Produces(ExcelContentType, JsonContentType)]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(HttpErrorMessageResponse), StatusCodes.Status404NotFound, contentType: JsonContentType)]
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