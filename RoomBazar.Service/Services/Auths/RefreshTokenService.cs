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

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IConfiguration configuration;
    private readonly IRepository<RefreshToken> repostory;
    private readonly AppDbContext appDbContext;
    public RefreshTokenService(IRepository<RefreshToken> repository,
        IConfiguration configuration,
        AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
        this.repostory = repository;
        this.configuration = configuration;
    }
    public async Task<TokenForResultDto> RefreshTokenAsync(RefreshTokenForCreationDto refreshToken)
    {
        var storedToken = await this.repostory.SelecAll()
            .Include(rt => rt.Register)
            .Where(t => t.Token == refreshToken.RefreshToken)
            .FirstOrDefaultAsync();

        if (storedToken == null || storedToken.ExpiryDate < DateTime.UtcNow)
        {
            throw new RoomBazarException(401, "Invalid or expired refresh token");
        }
        // yangi access token
        var newAccessToken = GenerateToken(storedToken.Register);

        // yangi refresh token
        var newRefreshToken = GenerateRefreshToken();
        storedToken.Token = newRefreshToken;
        storedToken.ExpiryDate = DateTime.UtcNow.AddMinutes(2);


        
        await this.appDbContext.SaveChangesAsync();

        return new TokenForResultDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
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
                    new Claim("Username", user.UserName)
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

    // Yangi refresh token yaratish uchun metod
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }
        return Convert.ToBase64String(randomNumber);
    }
}
