using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Restaurants.Domain.Request;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Restaurant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
          //check for the valid user using hard coded credentials

          if(request.Password!="password" || request.Username != "admin")
          {
                return Unauthorized();
          }
          // claims generation

          var claims = new []
          {
            new Claim(ClaimTypes.Name, request.Username),
            new Claim(ClaimTypes.Role,"Admin")
          };
            // generate token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this-is-a-very-strong-secret-key-12345"));
            
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var token = new JwtSecurityToken(
            claims:claims,
            expires:DateTime.UtcNow.AddMinutes(30),
            signingCredentials:creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            //return the token
            return Ok(new {Token= tokenString});
        }
    }
}
