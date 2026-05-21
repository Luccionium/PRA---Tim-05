using InfoedukaApi.DTOs;
using InfoedukaApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace InfoedukaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _authService.Login(dto);

            if (token == null)
                return Unauthorized("Pogrešan email ili lozinka.");

            return Ok(new { token });
        }
    }
}