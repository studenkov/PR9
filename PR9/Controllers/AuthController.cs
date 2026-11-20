using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PR9.Dtos;
using PR9.Services;

namespace PR9.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public AuthController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        try
        {
            var user = await _userService.RegisterAsync(dto.Login, dto.Password);
            var accessToken = _tokenService.CreateAccessToken(user);
            return Ok(new LoginResponseDto
            {
                AccessToken = accessToken,
                ExpiresInMinutes = 15
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [EnableRateLimiting("LoginPolicy")]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _userService.ValidateCredentialsAsync(dto.Login, dto.Password);
        if (user is null)
        {
            return Unauthorized(new { error = "Неверный логин или пароль." });
        }

        var accessToken = _tokenService.CreateAccessToken(user);
        return Ok(new LoginResponseDto
        {
            AccessToken = accessToken,
            ExpiresInMinutes = 15
        });
    }
}
