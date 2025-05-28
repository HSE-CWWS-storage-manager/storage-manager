using System.Net;
using backend.Services;
using common.Dtos;
using common.Dtos.Request;
using common.Dtos.Response;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

/// <summary>
/// Контроллер для работы с аккаунтами пользователей
/// </summary>
/// <param name="accountService">Сервис для взаимодействия с аккаунтами пользователей</param>
[Route("[controller]")]
[ApiController]
[Produces("application/json")]
public class AccountController(IAccountService accountService) : ControllerBase
{
    
    /// <summary>
    /// Регистрация нового пользователя
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Информация о новом пользователе</returns>
    /// <response code="200">Возвращает информацию о новом пользователе</response>
    /// <response code="400">Возвращает детализацию ошибки в запросе</response>
    /// <response code="500">Возвращает детализацию ошибки при регистрации (например, конфликт Email)</response>
    [HttpPost("Register")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    /// <summary>
    /// Аутентификация существующего пользователя
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Токен доступа</returns>
    /// <response code="200">Возвращает токен доступа</response>
    /// <response code="400">Возвращает детализацию ошибки в запросе</response>
    /// <response code="403">Возвращает детализацию ошибки авторизации (например, неправильный Email или пароль)</response>
    [HttpPost("Login")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(HttpErrorMessageResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Login(UserAuthenticationRequest request)
    {
        if (!ModelState.IsValid)
            return Problem();

        return Ok(await accountService.Login(request));
    }
}