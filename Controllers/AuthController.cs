using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
namespace ControllerAPI.Controllers;

// Model for the AuthController
public class UserLogin
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class AuthController: ControllerBase
{
    // สร้างตัวแปร _key เพื่อเก็บ secret key
    private readonly string _key;

    // สร้าง constructor รับค่า key มา
    public AuthController() {
        // กำหนดค่า secret key ให้กับ _key
        _key = "uw03u3uou3ioui3u33oiu3uyioeu93u8yiy83eu893";
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLogin user)
    {
        // ตรวจสอบว่า username และ password ถูกต้องหรือไม่
        if (user.Username == "admin" && user.Password == "1234")
        {
            // สร้าง token โดยใช้ secret key ที่กำหนดไว้
            var token = GenerateJwtToken(user.Username);
            // ส่ง token กลับไปให้ client
            return Ok(new { token });
        }
        // ถ้า username หรือ password ไม่ถูกต้อง
        return Unauthorized(); // status code 401
    }

    // สร้าง method สำหรับสร้าง generate token
    private string GenerateJwtToken(string username){
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: "http://localhost:5285", // ให้ token ออกมาจาก url นี้
            audience: "*", // ให้ใช้งานได้ทุกที่
            claims, // ข้อมูลที่จะเก็บใน token
            expires: DateTime.Now.AddMinutes(60), // หมดอายุใน 60 นาที
            signingCredentials: credentials // ใช้ secret key ที่กำหนดไว้
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}