using RoomBazar.Service.DTOs.Auths.RefreshTokens;

namespace RoomBazar.Service.Interfaces.Auths;

public interface IAuthService
{
    Task<TokenForResultDto> AuthenticateAsync(LoginForCreationDto loginDto);
}
