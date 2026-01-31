using HortaGestao.Application.Interfaces.Repositories;
using HortaGestao.Application.Interfaces.Services;
using HortaGestao.Application.Services;
using HortaGestao.Application.UseCases.Authentication;
using HortaGestao.Application.UseCases.Customer;
using HortaGestao.Application.UseCases.Dashboard;
using HortaGestao.Application.UseCases.OrderReservation;
using HortaGestao.Application.UseCases.PickupLocation;
using HortaGestao.Application.UseCases.Product;
using HortaGestao.Application.UseCases.Seller;
using HortaGestao.Application.UseCases.Stock;
using HortaGestao.Application.UseCases.Storage;
using HortaGestao.Domain.DomainService;
using HortaGestao.Domain.IRepositories;
using HortaGestao.Infrastructure.Interfaces;
using HortaGestao.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HortaGestao.Infrastructure.DependecyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IOrderReservationRepository, OrderReservationRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ISellerRepository, SellerRepository>();
        services.AddScoped<IStockRepository, StockRepository>();
        services.AddScoped<IReservationFeeCalculate, ReservationFeeCalculate>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IRefreshTokenRespository, RefreshTokenRespository>();
        services.AddScoped<IPickupLocationRespository, PickupLocationRespository>();
        services.AddScoped<IDashboardQueries, DashboardRepository>();
        
        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<CustomerService>();
        services.AddScoped<IOrderReservationService, OrderReservationService>();
        services.AddScoped<OrderReservationService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ProductService>();
        services.AddScoped<ISellerService, SellerService>();
        services.AddScoped<SellerService>();
        services.AddScoped<IStockService, StockService>();
        services.AddScoped<IPickupLocationService, PickupLocationService>();
        services.AddScoped<IStorageService, StorageService>();
        services.AddScoped<IDashboardService, DashboardService>();

        services.AddScoped<LoginUseCase>();
        services.AddScoped<LogoutUseCase>();
        
        services.AddScoped<CreateCustomerUseCase>();
        services.AddScoped<UpdateCustomerUseCase>();
        services.AddScoped<DeleteCustomerUseCase>();
        services.AddScoped<GetAllCustomerUseCase>();
        services.AddScoped<GetCustomerUseCase>();

        services.AddScoped<CreateStockUseCase>();
        services.AddScoped<UpdateQuantityUseCase>();
        services.AddScoped<GetAllStockUseCase>();
        services.AddScoped<GetProductStockUseCase>();
        services.AddScoped<GetStockByProductIdUseCase>();

        services.AddScoped<DeleteSellerUseCase>();
        services.AddScoped<CreateSellerUseCase>();
        services.AddScoped<GetAllSellerUseCase>();
        services.AddScoped<GetSellerUseCase>();
        services.AddScoped<UpdateSellerUseCase>();

        services.AddScoped<CreateProductUseCase>();
        services.AddScoped<UpdateProductUseCase>();
        services.AddScoped<GetAllProductUseCase>();
        services.AddScoped<GetProductUseCase>();
        services.AddScoped<DeleteProductUseCase>();
        services.AddScoped<FilterProductsUseCase>();
        services.AddScoped<UpdateProductStatusUseCase>();

        services.AddScoped<CreateOrderUseCase>();
        services.AddScoped<DeleteOrderUseCase>();
        services.AddScoped<GetAllOrderUseCase>();
        services.AddScoped<GetOrderBySecurityCodeUseCase>();
        services.AddScoped<GetOrderByStatusUseCase>();
        services.AddScoped<GetOrderUseCase>();
        services.AddScoped<UpdateOrderUseCase>();
        services.AddScoped<CalculateOrderUseCase>();
        services.AddScoped<RefreshUseCase>();
        services.AddScoped<FinishOrderUserCase>();
        
        services.AddScoped<CreatePickupLocationUseCase>();
        services.AddScoped<UpdatePickupLocationUseCase>();
        services.AddScoped<DeletePickupLocationUseCase>();
        services.AddScoped<GetByIdPickupLocationUseCase>();
        
        services.AddScoped<GetDashboardOverviewUseCase>();

        services.AddScoped<GetImageUseCase>();
        
        return services;
    }
}