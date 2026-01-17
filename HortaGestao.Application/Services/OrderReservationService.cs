
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
        
        var productsIds = orderReservation.ListOrderItems.Select(x => x.ProductId).Distinct();
        var productsEntities = await _productRepository.GetManyProducts(productsIds);

        var items = await CalculateOrderItemsAsync(productsEntities, orderReservationUpdateDTO.listOrderItens);
        
        var totalValue = items.Sum(i => i.UnitPrice * i.Quantity);
        var fee = _calculate.CalculateFeeCalculate(totalValue);
        
        if (!orderReservation.ListOrderItems.SequenceEqual(items))
        {
            foreach (var item in items)
            {
                orderReservation.AddItem(item.ProductId, item.SellerId, item.Quantity, item.UnitPrice);
            }
        }
        
        OrderReservationMapper.ToUpdateEntity(orderReservation, orderReservationUpdateDTO, items, fee);
        await _orderReservationRepository.UpdateAsync(orderReservation);
    }

    public async Task DeleteAsync(Guid id)
    {
       await _orderReservationRepository.DeleteAsync(id);
    }

    public async Task AddAsync(OrderReservationCreateDto orderReservationCreateDto)
    {
        var productsIds = orderReservationCreateDto.listOrderItens.Select(x => x.ProductId).Distinct();
        var productsEntities = await _productRepository.GetManyProducts(productsIds);

        var items = await CalculateOrderItemsAsync(productsEntities, orderReservationCreateDto.listOrderItens);
        
        var totalValue = items.Sum(i => i.UnitPrice * i.Quantity);
        var fee = _calculate.CalculateFeeCalculate(totalValue);
        
        var orderReservationEntity = OrderReservationMapper.ToCreateEntity(orderReservationCreateDto,fee, totalValue, orderReservationCreateDto.SellerId);
        
        foreach (var item in items)
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
       var productsIds = orderCalculateDTO.listOrderItens.Select(x => x.ProductId).Distinct();
       
       var productsEntities = await _productRepository.GetManyProducts(productsIds);

       var items = await CalculateOrderItemsAsync(productsEntities, orderCalculateDTO.listOrderItens);
        
       return OrderReservationMapper.ToCalculatedOrderDTO(items, productsEntities);
       
    }
    
    private async Task<ICollection<OrderReservationItemEntity>> CalculateOrderItemsAsync(
        IEnumerable<ProductEntity> productEntities, 
        IEnumerable<OrderReservationItemDto> itemDtos)
    {
        var items = new List<OrderReservationItemEntity>();
        foreach (var dto in itemDtos)
        {
            var product = productEntities.FirstOrDefault(p => p.Id == dto.ProductId);
            if (product != null)
            {
                var entity = new OrderReservationItemEntity(
                    Guid.NewGuid(), 
                    dto.ProductId, 
                    dto.SellerId, 
                    dto.Quantity, 
                    product.UnitPrice
                );
                items.Add(entity);
            }
        }
        return items;
    }
}