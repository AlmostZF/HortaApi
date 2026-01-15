
using DDDPractice.DDDPractice.Domain.Enums;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Mappers;
using HortaGestao.Domain.DomainService;
using HortaGestao.Domain.Entities;
using HortaGestao.Domain.IRepositories;

namespace HortaGestao.Application.Services;

public class OrderReservationService : IOrderReservationService
{

    private readonly IOrderReservationRepository _orderReservationRepository;
    private readonly IProductRepository _productRepository;
    private readonly IReservationFeeCalculate _calculate;
        
    public OrderReservationService(
        IOrderReservationRepository orderReservationRepository,
        IReservationFeeCalculate calculate,
        IProductRepository productRepository
    )
    {
        _orderReservationRepository = orderReservationRepository;
        _calculate = calculate;
        _productRepository = productRepository;
    }

    public async Task<List<OrderReservationResponseDto>> GetBySecurityCodeAsync(string securityCode)
    {
        var listOrder= await _orderReservationRepository.GetBySecurityCodeAsync(securityCode);
        return OrderReservationMapper.ToDtoList(listOrder);
    }

    public async Task<OrderReservationResponseDto> GetByIdAsync(Guid id)
    {
        var orderReservation = await _orderReservationRepository.GetByIdAsync(id);
        
        return OrderReservationMapper.ToDto(orderReservation);
    }

    public async Task UpdateStatusAsync(OrderReservationResponseDto orderReservationResponseDto)
    {
        var orderReservation = await _orderReservationRepository.GetByIdAsync(orderReservationResponseDto.Id);
        if (orderReservation == null)
            throw new InvalidOperationException("Reserva não encontrada.");
        
        await _orderReservationRepository.UpdateStatusAsync(orderReservationResponseDto.OrderStatus,orderReservationResponseDto.Id);
    }
    
    public async Task UpdateAsync(OrderReservationUpdateDto orderReservationUpdateDTO)
    {
        var orderReservation = await _orderReservationRepository.GetByIdAsync(orderReservationUpdateDTO.Id);
        if (orderReservation == null)
            throw new InvalidOperationException("Reserva não encontrada.");
        
        var (itens, totalValue) = await BuildNewOrderItemsAsync(orderReservationUpdateDTO);
        
        var fee = _calculate.CalculateFeeCalculate(totalValue);
        
        if (!orderReservation.ListOrderItems.SequenceEqual(itens))
        {
            foreach (var item in itens)
            {
                orderReservation.AddItem(item.ProductId, item.SellerId, item.Quantity, item.UnitPrice);
            }
        }
        
        OrderReservationMapper.ToUpdateEntity(orderReservation, orderReservationUpdateDTO, itens, fee);
        
        // OrderReservationMapper.ToUpdateEntity(orderReservation,orderReservationUpdateDTO);
        await _orderReservationRepository.UpdateAsync(orderReservation);
    }

    public async Task DeleteAsync(Guid id)
    {
       await _orderReservationRepository.DeleteAsync(id);
    }

    public async Task AddAsync(OrderReservationCreateDto orderReservationCreateDto)
    {
        var (itens, totalValue) = await BuildOrderItemsAsync(orderReservationCreateDto);

        var fee = _calculate.CalculateFeeCalculate(totalValue);
        
        var orderReservationEntity = OrderReservationMapper.ToCreateEntity(orderReservationCreateDto,fee, totalValue, orderReservationCreateDto.SellerId);
        
        foreach (var item in itens)
        {
            orderReservationEntity.AddItem(item.ProductId, item.SellerId, item.Quantity, item.UnitPrice);
        }
   
        await _orderReservationRepository.AddAsync(orderReservationEntity);
    }

    public async Task<List<OrderReservationResponseDto>> GetByStatusAsync(StatusOrder status)
    {
        var orderReservationEntities = await _orderReservationRepository.GetByStatusAsync(status);
        return OrderReservationMapper.ToDtoList(orderReservationEntities);
    }

    public async Task<List<OrderReservationResponseDto>> GetAllAsync()
    {
        var orderReservationEntities = await _orderReservationRepository.GetAllAsync();
        return OrderReservationMapper.ToDtoList(orderReservationEntities);
    }

    public async Task<OrderCalculateResponseDto> CalculateAsync(OrderCalculateDto orderCalculateDTO)
    {
        var (itens, totalValue) = await CalculateOrderItemsAsync(orderCalculateDTO);
        
        var fee = _calculate.CalculateFeeCalculate(totalValue);
        
        var orderReservationEntity = OrderReservationMapper.ToCalculatedOrderDTO(itens, fee, totalValue);

        return orderReservationEntity;

    }

    private async Task<(List<OrderReservationItemEntity> items, decimal total)> BuildOrderItemsAsync(
        OrderReservationCreateDto itemDtos)
    {
        var items = new List<OrderReservationItemEntity>();
        var list = itemDtos.listOrderItens;
        decimal total = 0;

        foreach (var dto in list)
        {
            var product = await _productRepository.GetByIdAsync(dto.ProductId);
            if (product == null)
                throw new Exception($"Produto {dto.ProductId} não encontrado.");

            var unitPrice = product.UnitPrice;
            
            items.Add(new OrderReservationItemEntity(Guid.NewGuid(), dto.ProductId, dto.SellerId, dto.Quantity, unitPrice));
        }
        return (items, total);
    }
    
    private async Task<(ICollection<OrderReservationItemEntity> items, decimal total)> BuildNewOrderItemsAsync(
        OrderReservationUpdateDto itemDtos)
    {
        var items = new List<OrderReservationItemEntity>();
        var list = itemDtos.listOrderItens;
        decimal total = 0;

        foreach (var dto in list)
        {
            var product = await _productRepository.GetByIdAsync(dto.ProductId);
            if (product == null)
                throw new Exception($"Produto {dto.ProductId} não encontrado.");

            var unitPrice = product.UnitPrice;
            
            items.Add(new OrderReservationItemEntity(Guid.NewGuid(), dto.ProductId, dto.SellerId, dto.Quantity, unitPrice));
        }
        return (items, total);
    }
    
    private async Task<(ICollection<OrderReservationItemEntity> items, decimal total)> CalculateOrderItemsAsync(
        OrderCalculateDto itemDtos)
    {
        var items = new List<OrderReservationItemEntity>();
        var list = itemDtos.listOrderItens;
        decimal total = 0;

        foreach (var dto in list)
        {
            var product = await _productRepository.GetByIdAsync(dto.ProductId);
            if (product == null)
                throw new Exception($"Produto {dto.ProductId} não encontrado.");

            var unitPrice = product.UnitPrice;
            
            items.Add(new OrderReservationItemEntity(Guid.NewGuid(), dto.ProductId, dto.SellerId, dto.Quantity, unitPrice));
        }
        return (items, total);
    }
}