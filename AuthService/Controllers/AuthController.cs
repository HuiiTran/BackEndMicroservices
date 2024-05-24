using AuthService.Dtos;
using AuthService.Entities;
using JWTAuthenManager;
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
        private readonly JWTTokenHandler jwtTokenHandler;

        public AuthController(JWTTokenHandler jwtTokenHandler)
        {
            this.jwtTokenHandler = jwtTokenHandler;
        }


    }
}
