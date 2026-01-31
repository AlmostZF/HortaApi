using DDDPractice.DDDPractice.Domain.Enums;
using HortaGestao.Application.DTOs.Request;
using HortaGestao.Application.DTOs.Response;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Interfaces.UnitOfWork;
using HortaGestao.Application.Mappers;
using HortaGestao.Domain.DomainService;
using HortaGestao.Domain.Entities;
using HortaGestao.Domain.IRepositories;

namespace HortaGestao.Application.Services;

public class OrderReservationService : IOrderReservationService
{

    private readonly IOrderReservationRepository _orderReservationRepository;
    private readonly IProductRepository _productRepository;
    private readonly IPickupLocationRespository _pickupLocationRepository;
    private readonly IReservationFeeCalculate _calculate;
    private readonly IStockRepository _stockRepository;
    private readonly IUnitOfWork _unitOfWork;
        
    public OrderReservationService(
        IOrderReservationRepository orderReservationRepository,
        IReservationFeeCalculate calculate,
        IProductRepository productRepository,
        IPickupLocationRespository pickupLocationRepository,
        IStockRepository stockRepository,
        IUnitOfWork unitOfWork
    )
    {
        _orderReservationRepository = orderReservationRepository;
        _calculate = calculate;
        _productRepository = productRepository;
        _pickupLocationRepository = pickupLocationRepository;
        _stockRepository = stockRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderReservationResponseDto?> GetBySecurityCodeAsync(string securityCode,Guid sellerId)
    {
        var listOrder = await _orderReservationRepository
            .GetBySecurityCodeAsync(securityCode,sellerId);

        if (listOrder == null)
            return null;
        

        return OrderReservationMapper.ToDto(listOrder);
    }

    public async Task<OrderReservationResponseDto> GetBySellerIdAsync(Guid id)
    {
        var orderReservation = await _orderReservationRepository.GetBySellerIdAsync(id);
        
        return OrderReservationMapper.ToDto(orderReservation);
    }

    public async Task FinishOrder(Guid guid, Guid sellerId)
    {
        var orderReservation = await _orderReservationRepository.GetByIdAsync(guid, sellerId);
        if (orderReservation == null)
            throw new InvalidOperationException("Reserva não encontrada.");
        
        await _orderReservationRepository.UpdateStatusAsync("Confirmada",guid);
    }
    
    public async Task UpdateAsync(OrderReservationUpdateDto orderReservationUpdateDTO)
    {
        var orderReservation = await _orderReservationRepository.GetBySellerIdAsync(orderReservationUpdateDTO.Id);
        if (orderReservation == null)
            throw new InvalidOperationException("Reserva não encontrada.");
        
        var productsIds = orderReservation.ListOrderItems
            .Select(x => x.ProductId).Distinct();
        var productsEntities = await _productRepository.GetManyProducts(productsIds);

        var items = await CalculateOrderItemsAsync(productsEntities,
            orderReservationUpdateDTO.listOrderItens);
        
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

    public async Task<List<CreateOrderReservationResponseDto>> AddAsync(OrderReservationCreateDto orderReservationCreateDto)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {   
            var responses = new List<CreateOrderReservationResponseDto>();
            foreach (var orderDetail in orderReservationCreateDto.OrderDetails)
            {
                var productsIds = orderDetail.listOrderItens
                    .Select(x => x.ProductId).Distinct();
                
                var productsEntities = await _productRepository.GetManyProducts(productsIds);

                var items = await CalculateOrderItemsAsync(productsEntities,
                    orderDetail.listOrderItens);
            
                var totalValue = items.Sum(i => i.UnitPrice * i.Quantity);
                var fee = _calculate.CalculateFeeCalculate(totalValue);
                
                var pickupLocation = await _pickupLocationRepository.GetByIdAsync(
                    orderDetail.PickupLocation.Id.Value, orderDetail.SellerId);

                if (pickupLocation == null) throw new Exception("Ponto de retirada não encontrado");
                 
                var orderReservationEntity = OrderReservationMapper.ToCreateEntity(
                    orderReservationCreateDto, orderDetail, pickupLocation, totalValue, orderDetail.SellerId);
            
                foreach (var item in items)
                {
                    orderReservationEntity.AddItem(item.ProductId, item.SellerId, item.Quantity, item.UnitPrice);
                }
            
                await _orderReservationRepository.AddAsync(orderReservationEntity);
                responses.Add(new CreateOrderReservationResponseDto 
                { 
                    SellerName = orderReservationEntity.Seller.Name,
                    SecurityCode = orderReservationEntity.SecurityCode.Value 
                });
            }   
            
            await _unitOfWork.CommitAsync();
            return responses;
        }
        catch (Exception e)
        {
           await _unitOfWork.RollbackAsync();
            Console.WriteLine(e);
            throw;
        }
        
    }

    public async Task<List<OrderReservationResponseDto>> GetByStatusAsync(StatusOrder status)
    {
        var orderReservationEntities = await _orderReservationRepository
            .GetByStatusAsync(status);
        return OrderReservationMapper.ToDtoList(orderReservationEntities);
    }

    public async Task<List<OrderReservationResponseDto>> GetAllAsync()
    {
        var orderReservationEntities = await _orderReservationRepository.GetAllAsync();
        return OrderReservationMapper.ToDtoList(orderReservationEntities);
    }

    public async Task<OrderCalculateResponseDto> CalculateAsync(OrderCalculateDto orderCalculateDTO)
    {
       var productsIds = orderCalculateDTO.listOrderItens
           .Select(x => x.ProductId).Distinct();

       var productsEntities = await _productRepository.GetManyProducts(productsIds);
       var stockEntities = await _stockRepository.GetByProductIdsAsync(productsIds);
       
       var stockDict = stockEntities.ToDictionary(s => s.ProductId, s => s.Quantity);
       var validatedItems = new List<OrderReservationItemDto>();
       
       foreach (var itemDto in orderCalculateDTO.listOrderItens)
       {
            stockDict.TryGetValue(itemDto.ProductId, out var quantity);
            
            if(itemDto.Quantity > quantity)
                itemDto.Quantity = quantity;
            
            validatedItems.Add(itemDto);
       }
       
       

       var items = await CalculateOrderItemsAsync(productsEntities,
           validatedItems);
        
       return OrderReservationMapper.ToCalculatedOrderDTO(items, productsEntities, stockDict);
       
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