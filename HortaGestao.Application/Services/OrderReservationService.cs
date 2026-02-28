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

    public async Task FinishOrderAsync(Guid id, Guid sellerId)
    {
        var orderReservation = await _orderReservationRepository.GetByIdAsync(id, sellerId);
        if (orderReservation == null)
            throw new InvalidOperationException("Reserva não encontrada.");
        
        await _orderReservationRepository.UpdateStatusAsync("Confirmada",id);
    }
    
    public async Task CancelOrderAsync(string securityCode, Guid sellerId)
    {   
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var orderReservation = await _orderReservationRepository.GetBySecurityCodeAsync(securityCode, sellerId);
            if (orderReservation == null)
                throw new InvalidOperationException("Reserva não encontrada.");
        
            var productsIds = orderReservation.ListOrderItems
                .Select(x => x.ProductId).Distinct();
            
            var stockEntities = await _stockRepository.GetByProductIdsAsync(productsIds);

            foreach (var item in stockEntities)
            {
                var orderItem = orderReservation.ListOrderItems.
                    FirstOrDefault(x => x.ProductId == item.ProductId);
                if (orderItem != null)
                {
                    item.AddQuantity(orderItem.Quantity);
                }
                
            }
            await _stockRepository.UpdateRangeAsync(stockEntities);
        
            await _orderReservationRepository.UpdateStatusAsync("Cancelada",orderReservation.Id);
            
            await _unitOfWork.CommitAsync(); 
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();
            Console.WriteLine(e);
            throw;
        }

    }
    
    public async Task UpdateAsync(OrderReservationUpdateDto orderReservationUpdateDTO)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {  
            var orderReservation = await _orderReservationRepository.GetBySellerIdAsync(orderReservationUpdateDTO.Id);
            if (orderReservation == null)
                throw new InvalidOperationException("Reserva não encontrada.");
            
            var productsIds = orderReservation.ListOrderItems
                .Select(x => x.ProductId).Distinct();
            
            var productsEntities = await _productRepository.GetManyProducts(productsIds);
            
            var stockEntities = await _stockRepository.GetByProductIdsAsync(productsIds);

            foreach (var item in stockEntities)
            {
                var orderItem = orderReservation.ListOrderItems.
                    FirstOrDefault(x => x.ProductId == item.ProductId);
                if (orderItem != null)
                {
                    item.AddQuantity(orderItem.Quantity);
                }
                
            }
            await _stockRepository.UpdateRangeAsync(stockEntities);
            
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
                        
            await _unitOfWork.CommitAsync();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();
            Console.WriteLine(e);
            throw;
        }
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
                var stockEntities = await _stockRepository.GetByProductIdsAsync(productsIds);

                foreach (var item in stockEntities)
                {
                    var orderItem = orderDetail.listOrderItens.
                        FirstOrDefault(x => x.ProductId == item.ProductId);
                    if (orderItem != null)
                    {
                        if (item.Quantity < orderItem.Quantity)
                            throw new Exception("estoque insuficiente");

                        item.RemoveQuantity(orderItem.Quantity);
                    }
                    
                    await _stockRepository.UpdateQuantityAsync(item);
                    
                }

                
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
    
    public async Task<OrderCalculateResponseDto> CalculateForViewAsync(OrderCalculateDto orderCalculateDTO) 
    {
        var productIds = orderCalculateDTO.listOrderItens.Select(x => x.ProductId)
            .Distinct();
        
        var (products, stockDict) = await GetOrderMetadataAsync(productIds);
        
        var items = await CalculateOrderItemsAsync(products,
            orderCalculateDTO.listOrderItens);
        return OrderReservationMapper.ToCalculatedOrderDTO(items, products, stockDict);
    }


    public async Task<OrderCalculateResponseDto> CalculateForCheckoutAsync(OrderCalculateDto orderCalculateDTO)
    {
        var productIds = orderCalculateDTO.listOrderItens.Select(x => x.ProductId)
            .Distinct();
        
        var (productsEntities, stockDictionary) = await GetOrderMetadataAsync(productIds);
        
        var validatedItems = ValidateStock(orderCalculateDTO.listOrderItens, stockDictionary);
        var items = await CalculateOrderItemsAsync(productsEntities,
            validatedItems);
        
        return OrderReservationMapper.ToCalculatedOrderDTO(items, productsEntities, stockDictionary);
    }

    private List<OrderReservationItemDto> ValidateStock(List<OrderReservationItemDto> listOrderItens,
        Dictionary<Guid,int> stockDictionary)
    {
        var validatedItems = new List<OrderReservationItemDto>();
        
        foreach (var itemDto in listOrderItens)
        {
            stockDictionary.TryGetValue(itemDto.ProductId, out var quantity);
           
            if(itemDto.Quantity > quantity)
                itemDto.Quantity = quantity;
            
            validatedItems.Add(itemDto);
        }
        
        return validatedItems;
    }

    private async Task<(IEnumerable<ProductEntity> products, Dictionary<Guid, int> stock)> GetOrderMetadataAsync(
        IEnumerable<Guid> productIds)
    {
        var productsEntities = await _productRepository.GetManyProducts(productIds);
        var stockEntities = await _stockRepository.GetByProductIdsAsync(productIds);
       
        var stockDictionary = stockEntities.ToDictionary(s => s.ProductId, s => s.Quantity);
    
        return (productsEntities, stockDictionary);
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