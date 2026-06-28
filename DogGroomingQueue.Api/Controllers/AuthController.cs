using DogGroomingQueue.Api.Data;
using DogGroomingQueue.Api.DTOs;
using DogGroomingQueue.Api.Helpers;
using DogGroomingQueue.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DogGroomingQueue.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly JwtHelper _jwtHelper;

    public AuthController(ApplicationDbContext context, JwtHelper jwtHelper)
    {
        _context = context;
        _jwtHelper = jwtHelper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var userExists = await _context.Users
            .AnyAsync(u => u.Username_Vch == request.Username);

        if (userExists)
        {
            return BadRequest("שם המשתמש כבר קיים");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Username_Vch = request.Username,
            PasswordHash_Vch = passwordHash,
            FirstName_Vch = request.FirstName,
            CreatedAt_Dat = DateTime.Now
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("המשתמש נרשם בהצלחה");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username_Vch == request.Username);

        if (user == null)
        {
            return Unauthorized("שם משתמש או סיסמה שגויים");
        }

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(
            request.Password,
            user.PasswordHash_Vch);

        if (!isPasswordValid)
        {
            return Unauthorized("שם משתמש או סיסמה שגויים");
        }

        return Ok(new AuthResponse
        {
            UserId = user.UserId_Int,
            FirstName = user.FirstName_Vch,
            Token = _jwtHelper.GenerateToken(user)
        });
    }
}