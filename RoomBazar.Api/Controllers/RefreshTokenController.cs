using Microsoft.AspNetCore.Mvc;
using RoomBazar.Api.Models;
using RoomBazar.Service.DTOs.Auths.RefreshTokens;
using RoomBazar.Service.Interfaces.Auths;

namespace RoomBazar.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RefreshTokenController : ControllerBase
    {
        private readonly IRefreshTokenService refreshTokenService;

        public RefreshTokenController(IRefreshTokenService refreshTokenService)
        {
            this.refreshTokenService = refreshTokenService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(RefreshTokenForCreationDto refreshToken)
        {
            var response = new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.refreshTokenService.RefreshTokenAsync(refreshToken)
            };

            return Ok(response);
        }
    }
}
