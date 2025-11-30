using Microsoft.AspNetCore.Mvc;
using washit.dtos;
using washit.repository;
using washit.services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtService _jwtService;
    public AuthController(IUserService userService, IJwtService jwtService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _userService.GetUserAsync(dto.UserName, dto.Password);

        if (user == null) return Unauthorized(new { Message = "Invalid Credentials" });

        var token = _jwtService.GenerateToken(user);

        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {


        int newUserId = await _userService.RegisterUserAsync(registerDto);

        return Ok(new
        {
            Message = "User Register successfully",
            UserId = newUserId
        });
    }
}