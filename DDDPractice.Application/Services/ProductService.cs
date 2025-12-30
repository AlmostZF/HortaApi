using DDDPractice.DDDPractice.Domain.Repositories;
using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Interfaces;
using DDDPractice.Application.Mappers;
using DDDPractice.Application.Shared;

namespace DDDPractice.Application.Services;

public class ProductService: IProductService
{

    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductResponseDto> GetByIdAsync(Guid id)
    {
        var productEntity = await _productRepository.GetByIdAsync(id);
        return ProductMapper.ToDto(productEntity);
    }

    public async Task UpdateAsync(ProductUpdateDTO productUpdateDTO)
    {
        var existingProduct = await _productRepository.GetByIdAsync(productUpdateDTO.Id);
        if (existingProduct == null)
        {
            throw new InvalidOperationException("Produto não encontrado.");
        }

        ProductMapper.ToUpdateEntity(existingProduct, productUpdateDTO);    
        await _productRepository.UpdateAsync(existingProduct);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _productRepository.DeleteAsync(id);
    }
    
    public async Task<Guid> AddAsync(ProductCreateDTO productCreateDTO)
    {
        var productEntity = ProductMapper.ToCreateEntity(productCreateDTO);
        await _productRepository.AddAsync(productEntity);
        return productEntity.Id;
    }

    public async Task<List<ProductResponseDto>> GetAllAsync()
    {
        var productEntity = await _productRepository.GetAllAsync();
        return ProductMapper.ToDtoList(productEntity);
    }

    public async Task<PagedResponse<ProductResponseDto>> FilterAsync(ProductFilterDTO productFilterDto)
    {
        var filter = ProductMapper.toFilter(productFilterDto);
        
        var filteredProducts= await _productRepository.FilterAsync(filter);

        var totalItems = await _productRepository.CountAsync(filter);
        var toDtoList = ProductMapper.ToDtoList(filteredProducts);
        
        var itemsPerPage = productFilterDto.MaxItensPerPage ?? 10;
        var totalPages = (int)Math.Ceiling(totalItems / (double)itemsPerPage);
        
        
        var pagedResponse = new PagedResponse<ProductResponseDto>
        {
            Data = toDtoList,
            Pagination = new Pagination
            {
                PageNumber = productFilterDto.PageNumber ?? 1,
                ItemsPerPage = itemsPerPage,
                TotalItems = totalItems,
                TotalPages = totalPages
            }
        };

        return pagedResponse;

    }
}