using RoomBazar.Service.DTOs.Products;

namespace RoomBazar.Service.Interfaces.Products;

public interface IProductService
{
    Task<ProductForResultDto> AddProductAsync(ProductForCreationDto productForCreationDto);
    Task<IEnumerable<ProductForResultDto>> GetAllProductsAsync();
}
