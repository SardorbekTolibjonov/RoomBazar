using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomBazar.Api.Models;
using RoomBazar.Service.DTOs.Auths.Registers;
using RoomBazar.Service.Interfaces.Auths;
using System.Net.NetworkInformation;

namespace RoomBazar.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterCtontroller : ControllerBase
    {
        private readonly IRegisterService registerService;

        public RegisterCtontroller(IRegisterService registerService)
        {
            this.registerService = registerService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(RegisterDto dto)
        {
            var response = new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.registerService.AddRegisterAsync(dto)
            };
            return Ok(response);
        }


        [Authorize]
        [HttpPut]
        public async Task<IActionResult> PutAsync(RegisterDto dto)
        {
            var response = new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.registerService.UpdateRegisterAsync(dto)
            };
            return Ok(response);
        }
        
    }
}
