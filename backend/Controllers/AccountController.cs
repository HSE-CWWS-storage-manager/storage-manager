using backend.Services;
using common.Dtos.Request;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("[controller]")]
[ApiController]
public class AccountController(IAccountService accountService) : ControllerBase
{
    [HttpPost("Register")]
    public async Task<IActionResult> Register(UserRegistrationRequest request)
    {
        if (!ModelState.IsValid)
            return Problem();

        var tuple = await accountService.Create(request);
        var user = tuple.Item1;
        var result = tuple.Item2;

        if (result.Succeeded)
            return Ok(user);

        var dict = new Dictionary<string, object?>();

        foreach (var error in result.Errors) dict.Add(error.Code, error.Description);

        return Problem(extensions: dict);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(UserAuthenticationRequest request)
    {
        if (!ModelState.IsValid)
            return Problem();

        return Ok(await accountService.Login(request));
    }
}