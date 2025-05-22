using backend.Services;
using backend.Utils;
using common.Dtos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(Policy = StringConstants.EditorPolicy)]
public class StudentController(IStudentService studentService) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> Create(StudentCreateRequest request)
    {
        return !ModelState.IsValid ? Problem() : Ok(await studentService.Create(request));
    }
    
    [HttpGet]
    public async Task<IActionResult> Find([FromQuery] StudentFindRequest request)
    {
        return !ModelState.IsValid ? Problem() : Ok(await studentService.Find(request));
    }
    
    [HttpPatch]
    public async Task<IActionResult> Update(StudentUpdateRequest request)
    {
        return !ModelState.IsValid ? Problem() : Ok(await studentService.Update(request));
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] StudentDeleteRequest request)
    {
        return !ModelState.IsValid ? Problem() : Ok(await studentService.Delete(request));
    }
}