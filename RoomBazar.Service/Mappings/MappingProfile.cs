using AutoMapper;
using RoomBazar.Domain.Entites;
using RoomBazar.Domain.Entites.Auths;
using RoomBazar.Service.DTOs.Auths.RefreshTokens;
using RoomBazar.Service.DTOs.Auths.Registers;
using RoomBazar.Service.DTOs.Products;

namespace RoomBazar.Service.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Register, RegisterDto>().ReverseMap();
        CreateMap<Register, RegisterForResultDto>().ReverseMap();

        CreateMap<RefreshToken,RefreshTokenForCreationDto>().ReverseMap();
        CreateMap<RefreshToken,TokenForResultDto>().ReverseMap();

        CreateMap<Product,ProductForCreationDto>().ReverseMap();
        CreateMap<Product, ProductForResultDto>().ReverseMap();
    }
}
