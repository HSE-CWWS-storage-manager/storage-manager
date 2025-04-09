using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("[controller]")]
[ApiController]
public class AccountController : ControllerBase
{

    private readonly IAccountService _accountService;
    
    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("Register")]
    public IActionResult Register(UserRegistrationModel model)
    {
        if (!ModelState.IsValid)
            return Problem();
        
        _accountService.Create(model, out var result);
        
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
}