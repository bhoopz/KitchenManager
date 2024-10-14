using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Microsoft.AspNetCore.Authentication;
using Application.Services;
using Application.Interfaces;
using Application.Responses;
using Application.Helpers;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost, Route("register")]
        public async Task<IActionResult> Register(RegisterUserDto userData)
        {
            var result = await _authService.RegisterUser(userData);

            if (result.Succeeded)
            {
                return Ok(result);  
            }
            else
            {
                return BadRequest(result);  
            }
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login(LoginUserDto credentials)
        {
            var result = await _authService.LoginUser(credentials);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            else
            {
               return BadRequest(result);
            }
        }

    }
}
