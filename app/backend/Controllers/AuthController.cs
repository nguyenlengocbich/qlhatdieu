using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CashewCms.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CashewCms.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(AppDbContext db, IConfiguration config) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var email = (request.Email ?? "").Trim().ToLowerInvariant();
        var password = request.Password ?? "";

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            return BadRequest(new
            {
                success = false,
                message = "Vui lòng nhập đầy đủ email và mật khẩu."
            });
        }

        var admin = await db.AdminUsers
            .SingleOrDefaultAsync(x => x.Email == email);

        if (admin is null || admin.Status != "active")
        {
            return Unauthorized(new
            {
                success = false,
                message = "Email hoặc mật khẩu không chính xác."
            });
        }

        bool valid;

        try
        {
            valid = BCrypt.Net.BCrypt.Verify(
                password,
                admin.PasswordHash
            );
        }
        catch
        {
            valid = false;
        }

        if (!valid)
        {
            return Unauthorized(new
            {
                success = false,
                message = "Email hoặc mật khẩu không chính xác."
            });
        }

        var keyText = config["Jwt:Key"];

        if (string.IsNullOrWhiteSpace(keyText) ||
            keyText.Length < 32)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "JWT key chưa được cấu hình đúng."
            });
        }

        var claims = new[]
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                admin.Id.ToString()
            ),

            new Claim(
                ClaimTypes.Email,
                admin.Email
            ),

            new Claim(
                ClaimTypes.Name,
                admin.Name
            ),

            new Claim(
                ClaimTypes.Role,
                "admin"
            )
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(keyText)
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var expires = DateTime.UtcNow.AddHours(
            double.TryParse(
                config["Jwt:ExpiresHours"],
                out var hours
            )
                ? hours
                : 8
        );

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );

        return Ok(new
        {
            success = true,
            token = new JwtSecurityTokenHandler()
                .WriteToken(token),
            expiresAt = expires,
            user = new
            {
                id = admin.Id,
                email = admin.Email,
                name = admin.Name
            }
        });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var idText =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(idText, out var id))
            return Unauthorized();

        var admin = await db.AdminUsers
            .Where(x =>
                x.Id == id &&
                x.Status == "active")
            .Select(x => new
            {
                id = x.Id,
                email = x.Email,
                name = x.Name,
                status = x.Status
            })
            .SingleOrDefaultAsync();

        return admin is null
            ? Unauthorized()
            : Ok(admin);
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return Ok(new
        {
            success = true,
            message = "Đăng xuất thành công."
        });
    }

    public record LoginRequest(
        string? Email,
        string? Password
    );
}