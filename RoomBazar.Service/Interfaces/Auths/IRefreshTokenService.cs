using RoomBazar.Service.DTOs.Auths.RefreshTokens;

namespace RoomBazar.Service.Interfaces.Auths;

public interface IRefreshTokenService
{
    Task<TokenForResultDto> RefreshTokenAsync(RefreshTokenForCreationDto refreshToken);

}
