using AuthService.Dtos;
using AuthService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServicesCommon;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("authen")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> logger;
        private readonly IRepository<AllUser> repository;

        public AuthController(IRepository<AllUser> repository, ILogger<AuthController> logger)
        {
            this.logger = logger;
            this.repository = repository;
        }

        [HttpGet]
        public IActionResult Get(string UserName, string PassWord)
        {
            /*var users = (await repository.GetAllAsync())
                .Single(user => user.UserName == UserName && user.Password == PassWord);

            if (users == null)
            {
                return NotFound();
            }*/

            if (UserName == "huy" &&  PassWord == "123")
            {
                var now = DateTime.UtcNow;

                var claims = new Claim[]
                {
                new Claim("UserName", UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
                };

                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("This is key is my test private key"));
                var tokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = key,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
                var jwt = new JwtSecurityToken(
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(TimeSpan.FromMinutes(5)),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                    );

                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                var responseJson = new
                {
                    access_token = encodedJwt,
                    expires_in = (int)TimeSpan.FromMinutes(5).TotalSeconds
                };

                return Json(responseJson);
                
            }

            return Json("");
            
        }
    }
}
