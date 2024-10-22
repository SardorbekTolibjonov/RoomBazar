using RoomBazar.Service.DTOs.Auths.Registers;

namespace RoomBazar.Service.Interfaces.Auths;

public interface IRegisterService
{
    Task<RegisterForResultDto> AddRegisterAsync(RegisterDto registerForCreationDto);
    Task<RegisterForResultDto> UpdateRegisterAsync(RegisterDto registerForUpdateDto);
}
