using DDD_Practice.DDDPractice.Domain.Repositories;
using DDD_Practice.DDDPractice.Domain.Service;
using DDD_Practice.DDDPractice.Infrastructure;
using DDD_Practice.DDDPractice.Infrastructure.Repositories;
using DDDPractice.Application.Interfaces;
using DDDPractice.Application.Services;
using DDDPractice.Application.UseCases;
using DDDPractice.Application.UseCases.OrderReservation;
using DDDPractice.Application.UseCases.Product;
using DDDPractice.Application.UseCases.Seller;
using DDDPractice.Application.UseCases.Stock;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IOrderReservationService, OrderReservationService>();
builder.Services.AddScoped<OrderReservationService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ISellerService, SellerService>();
builder.Services.AddScoped<SellerService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Repositories
builder.Services.AddScoped<IOrderReservationRepository, OrderReservationRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISellerRepository, SellerRepository>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IReservationFeeCalculate, ReservationFeeCalculate>();

// Use Case
builder.Services.AddScoped<CreateUserUseCase>();
builder.Services.AddScoped<UpdateUserUseCase>();
builder.Services.AddScoped<DeleteUserUseCase>();
builder.Services.AddScoped<GetAllUserUseCase>();
builder.Services.AddScoped<GetUserUserCase>();

builder.Services.AddScoped<CreateStockUseCase>();
builder.Services.AddScoped<UpdateQuantityUseCase>();
builder.Services.AddScoped<GetAllStockUseCase>();
builder.Services.AddScoped<GetProductStockUseCase>();
builder.Services.AddScoped<GetStockByProductIdUseCase>();

builder.Services.AddScoped<DeleteSellerUseCase>();
builder.Services.AddScoped<CreateSellerUseCase>();
builder.Services.AddScoped<GetAllSellerUseCase>();
builder.Services.AddScoped<GetSellerUseCase>();
builder.Services.AddScoped<UpdateSellerUseCase>();

builder.Services.AddScoped<CreateProductUseCase>();
builder.Services.AddScoped<UpdateProductUseCase>();
builder.Services.AddScoped<GetAllProductUseCase>();
builder.Services.AddScoped<GetProductUseCase>();
builder.Services.AddScoped<DeleteProductUseCase>();
builder.Services.AddScoped<FilterProductsUseCase>();

builder.Services.AddScoped<CreateOrderUseCase>();
builder.Services.AddScoped<DeleteOrderUseCase>();
builder.Services.AddScoped<GetAllOrderUseCase>();
builder.Services.AddScoped<GetOrderBySecurityCodeUseCase>();
builder.Services.AddScoped<GetOrderByStatusUseCase>();
builder.Services.AddScoped<GetOrderUseCase>();
builder.Services.AddScoped<UpdateOrderUseCase>();
builder.Services.AddScoped<CalculateOrderUseCase>();


builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseCors("AllowSpecificOrigin");

app.Run();

// dotnet ef migrations add InitialCreate --project DDDPractice.Infrastructure --startup-project DDDPractice.API