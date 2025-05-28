using backend.Services;
using backend.Utils;
using common.Dtos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;


/// <summary>
/// Контроллер для управления студентами
/// </summary>
/// <param name="studentService"></param>
[Route("[controller]")]
[ApiController]
[Authorize(Policy = StringConstants.EditorPolicy)]
[Produces("application/json")]
public class StudentController(IStudentService studentService) : ControllerBase
{

    /// <summary>
    /// Создать запись о студенте в системе
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create(StudentCreateRequest request)
    {
        return !ModelState.IsValid ? Problem() : Ok(await studentService.Create(request));
    }
    
    /// <summary>
    /// Найти студента по его данным
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Find([FromQuery] StudentFindRequest request)
    {
        return !ModelState.IsValid ? Problem() : Ok(await studentService.Find(request));
    }
    
    /// <summary>
    /// Обновить запись о студенте
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPatch]
    public async Task<IActionResult> Update(StudentUpdateRequest request)
    {
        return !ModelState.IsValid ? Problem() : Ok(await studentService.Update(request));
    }
    
    /// <summary>
    /// Удалить запись о студенте
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] StudentDeleteRequest request)
    {
        return !ModelState.IsValid ? Problem() : Ok(await studentService.Delete(request));
    }
}