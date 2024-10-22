using Microsoft.AspNetCore.Mvc;
using RoomBazar.Api.Models;
using RoomBazar.Service.DTOs.Auths.RefreshTokens;
using RoomBazar.Service.Interfaces.Auths;

namespace RoomBazar.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> PostAsync(LoginForCreationDto loginDto)
        {
            var response = new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.authService.AuthenticateAsync(loginDto)
            };

            return Ok(response);
        }
    }
}
