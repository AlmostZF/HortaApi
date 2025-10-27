using DDD_Practice.DDDPractice.Domain.Repositories;
using DDDPractice.Application.DTOs;
using DDDPractice.Application.DTOs.Request.ProductCreateDTO;
using DDDPractice.Application.Interfaces;
using DDDPractice.Application.Mappers;

namespace DDDPractice.Application.Services;

public class SellerService : ISellerService
{
    private readonly ISellerRepository _sellerRepository;

    public SellerService(ISellerRepository sellerRepository)
    {
        _sellerRepository = sellerRepository;
    }

    public async Task<SellerResponseDto> GetByIdAsync(Guid id)
    {
        var sellerEntity = await _sellerRepository.GetByIdAsync(id);
        return SellerMapper.ToDto(sellerEntity);
    }

    public async Task<Guid> AddAsync(SellerCreateDTO sellerCreateDTO)
    {
        var sellerEntity = SellerMapper.ToCreateEntity(sellerCreateDTO);
        await _sellerRepository.AddAsync(sellerEntity);
        return sellerEntity.Id;
    }

    public async Task UpdateAsync(SellerUpdateDTO sellerUpdateDTO)
    { 
        var existingSeller = await _sellerRepository.GetByIdAsync(sellerUpdateDTO.Id);
        if (existingSeller == null)
            throw new InvalidOperationException("Vendedor não encontrado.");
        
        SellerMapper.ToUpdateEntity(existingSeller, sellerUpdateDTO);
        await _sellerRepository.UpdateAsync(existingSeller);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _sellerRepository.DeleteAsync(id);
    }

    public async Task<List<SellerResponseDto>> GetAllAsync()
    {
        var sellerList = await _sellerRepository.GetAllAsync();
        return SellerMapper.ToDtoList(sellerList);
    }
}