using HortaGestao.Application.DTOs;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Mappers;
using HortaGestao.Application.Shared;
using HortaGestao.Domain.Repositories;

namespace HortaGestao.Application.Services;

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

        if (productEntity == null) throw new InvalidOperationException("Produto não encontrado.");
        
        if(! productEntity.IsActive) return null;
        
        return ProductMapper.ToDto(productEntity);
    }

    public async Task UpdateAsync(ProductUpdateDto productUpdateDTO)
    {
        var existingProduct = await _productRepository.GetByIdAsync(productUpdateDTO.Id);
        if (existingProduct == null)
        {
            throw new InvalidOperationException("Produto não encontrado.");
        }

        ProductMapper.ToUpdateEntity(existingProduct, productUpdateDTO);    
        await _productRepository.UpdateAsync(existingProduct);
    }

    public async Task UpdateStatusAsync(ProductUpdateStatusDto productUpdateStatusDto)
    {
        var existingProduct = await _productRepository.GetByIdAsync(productUpdateStatusDto.Id);
        if (existingProduct == null)
        {
            throw new InvalidOperationException("Produto não encontrado.");
        }

        ProductMapper.ToUpdateStatus(existingProduct, productUpdateStatusDto.IsActive);    
        await _productRepository.UpdateAsync(existingProduct);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _productRepository.DeleteAsync(id);
    }
    
    public async Task<Guid> AddAsync(ProductCreateDto productCreateDTO)
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

    public async Task<PagedResponse<ProductResponseDto>> FilterAsync(ProductFilterDto productFilterDto)
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