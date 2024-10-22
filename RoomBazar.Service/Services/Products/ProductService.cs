using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoomBazar.Data.IRepositories;
using RoomBazar.Domain.Entites;
using RoomBazar.Service.DTOs.Products;
using RoomBazar.Service.Exceptions;
using RoomBazar.Service.Interfaces.Products;

namespace RoomBazar.Service.Services.Products;

public class ProductService : IProductService
{
    private readonly IRepository<Product> repository;
    private readonly IMapper mapper;

    public ProductService(IRepository<Product> repository,IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }
    public async Task<ProductForResultDto> AddProductAsync(ProductForCreationDto productForCreationDto)
    {
        var product = await this.repository.SelecAll()
            .Where(p => p.Name == productForCreationDto.Name)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if(product != null)
        {
            throw new RoomBazarException(409, "Product is already exist");
        }
        var mappedProduct = this.mapper.Map<Product>(productForCreationDto);
        mappedProduct.CreateAt = DateTime.Now;

        var result = await this.repository.CreateAsync(mappedProduct);

        return this.mapper.Map<ProductForResultDto>(result);
    }

    public async Task<IEnumerable<ProductForResultDto>> GetAllProductsAsync()
    {
        var products = await this.repository.SelecAll()
            .ToListAsync();

        return this.mapper.Map<IEnumerable<ProductForResultDto>>(products);
    }
}
