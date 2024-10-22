using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RoomBazar.Data.IRepositories;
using RoomBazar.Domain.Entites.Auths;
using RoomBazar.Service.DTOs.Auths.Registers;
using RoomBazar.Service.Exceptions;
using RoomBazar.Service.Interfaces.Auths;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace RoomBazar.Service.Services.Auths;

public class RegisterService : IRegisterService
{
    private readonly IRepository<Register> repository;
    private readonly IMapper mapper;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IConfiguration configuration;
    public RegisterService(IRepository<Register> repository, 
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.configuration = configuration;
        this.httpContextAccessor = httpContextAccessor;
    }
    public async Task<RegisterForResultDto> AddRegisterAsync(RegisterDto registerForCreationDto)
    {
        var existUser = await this.repository.SelecAll().Where(u => u.UserName == registerForCreationDto.UserName &&
                                u.PhoneNumber == registerForCreationDto.PhoneNumber)
                                .AsNoTracking()
                                .FirstOrDefaultAsync();
        if (existUser != null)
            throw new RoomBazarException(409, "User already exists");

        var register = this.mapper.Map<Register>(registerForCreationDto);
        register.CreateAt = DateTime.UtcNow;
        var result = await this.repository.CreateAsync(register);

        return this.mapper.Map<RegisterForResultDto>(result);
    }

    public async Task<RegisterForResultDto> UpdateRegisterAsync(RegisterDto registerDto)
    {
        var userId = GetRegisterIdFromToken();

        var userCheck = await this.repository.SelecAll()
            .Where(u => u.Id == int.Parse(userId))
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (userCheck is null)
            throw new RoomBazarException(404, "User is not found");

        // Foydalanuvchini DTO dan mapping qiling
        var mappedUser = this.mapper.Map(registerDto, userCheck);
        mappedUser.UpdateAt = DateTime.UtcNow;

        // Yangilangan foydalanuvchini bazaga yozing va natijani kuting
        var updatedUser = await this.repository.UpdateAsync(mappedUser);

        // Yangilangan foydalanuvchini DTO formatiga mapping qiling
        return this.mapper.Map<RegisterForResultDto>(updatedUser);
    }

    private string GetRegisterIdFromToken()
    {
        var accessToken = this.httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (string.IsNullOrEmpty(accessToken))
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key  = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);

        try
        {
            // Tokenni validatsiya qilish
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken validatedToken);

            // Claim dan user ID ni olish
            var userIdClaim = principal.FindFirst("Id");

            return userIdClaim?.Value;
        }
        catch (RoomBazarException ex)
        {
            throw new RoomBazarException(401, "Access token expired or error");
        }
    }
}
