using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RoomBazar.Data.DbContexts;
using RoomBazar.Data.IRepositories;
using RoomBazar.Domain.Entites.Auths;
using RoomBazar.Service.DTOs.Auths.RefreshTokens;
using RoomBazar.Service.Exceptions;
using RoomBazar.Service.Interfaces.Auths;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RoomBazar.Service.Services.Auths;

public class AuthService : IAuthService
{
    private readonly IRepository<Register> registerRepository;
    private readonly IConfiguration configuration;
    private readonly IRepository<RefreshToken> refreshTokenRepository;
    public AuthService(IConfiguration configuration,
        IRepository<Register> registerRepository,
        IRepository<RefreshToken> refreshTokenRepository)
    {
        this.configuration = configuration;
        this.registerRepository = registerRepository;
        this.refreshTokenRepository = refreshTokenRepository;
    }
    public async Task<TokenForResultDto> AuthenticateAsync(LoginForCreationDto loginDto)
    {
        var user = await this.registerRepository.SelecAll().Where(u => u.UserName == loginDto.Username)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (user == null || user.Password != loginDto.Password)
        {
            throw new RoomBazarException(400, "Invalid username or password");
        }

        var accessToken = GenerateToken(user);

        var refreshToken = GenerateRefreshToken();

        var tokenEntry = new RefreshToken
        {
            Token = refreshToken,
            ExpiryDate = DateTime.UtcNow.AddMinutes(2), // 2 minut davomida amal qiladi
            RegisterId = user.Id // Refresh tokenni foydalanuvchi bilan bog'lash
        };

        await this.refreshTokenRepository.CreateAsync(tokenEntry);

        return new TokenForResultDto
        { 
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

    }

    private string GenerateToken(Register user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                 new Claim("Id", user.Id.ToString()),
                 new Claim("Username",user.UserName)
            }),
            Audience = configuration["JWT:Audience"],
            Issuer = configuration["JWT:Issuer"],
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddSeconds(double.Parse(configuration["JWT:Expire"])),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }
        return Convert.ToBase64String(randomNumber); // Tasodifiy 64-bayt uzunlikdagi refresh token
    }
}
