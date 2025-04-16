using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("[controller]")]
[ApiController]
public class AccountController(IAccountService accountService) : ControllerBase
{
    [HttpPost("Register")]
    public IActionResult Register(UserRegistrationModel model)
    {
        if (!ModelState.IsValid)
            return Problem();
        
        accountService.Create(model, out var result);
        
        if (result.Succeeded)
        {
            return Ok(model);
        }

        var dict = new Dictionary<string, object?>();

        foreach (var error in result.Errors)
        {
            dict.Add(error.Code, error.Description);
        }
        
        return Problem(extensions: dict);
    }

    [HttpPost("Login")]
    public IActionResult Login(UserAuthenticationModel model)
    {
        if (!ModelState.IsValid)
            return Problem();

        return Ok(accountService.Login(model));
    }
    
    [HttpGet("Test")]
    [Authorize]
    public IActionResult Test()
    {
        return Ok("test");
    }
}