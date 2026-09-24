using FullStackPoc.Api.Data;
using FullStackPoc.Api.DTOs;
using FullStackPoc.Api.Models;
using FullStackPoc.Api.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace FullStackPoc.Api.Controllers;
[ApiController, Route("api/auth")]
public sealed class AuthController(MongoDbContext db, JwtService jwt, ILogger<AuthController> logger) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        if (await db.Users.Find(x => x.Email == request.Email).AnyAsync()) return Conflict(new { message = "Email already registered." });
        var user = new User { Name = request.Name, Email = request.Email, PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password) };
        await db.Users.InsertOneAsync(user);
        logger.LogInformation("User registered: {Email}", user.Email);
        return Ok(new AuthResponse(jwt.CreateToken(user), user.Name, user.Email));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var user = await db.Users.Find(x => x.Email == request.Email).FirstOrDefaultAsync();
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) return Unauthorized(new { message = "Invalid email or password." });
        logger.LogInformation("User login successful: {Email}", user.Email);
        return Ok(new AuthResponse(jwt.CreateToken(user), user.Name, user.Email));
    }
}
