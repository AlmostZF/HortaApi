using System.Text;
using DDD_Practice.DDDPractice.Domain.Repositories;
using DDD_Practice.DDDPractice.Domain.Service;
using DDD_Practice.DDDPractice.Infrastructure;
using DDD_Practice.DDDPractice.Infrastructure.Identity;
using DDD_Practice.DDDPractice.Infrastructure.Repositories;
using DDDPractice.Application.Interfaces;
using DDDPractice.Application.Services;
using DDDPractice.Application.UseCases;
using DDDPractice.Application.UseCases.Auth;
using DDDPractice.Application.UseCases.OrderReservation;
using DDDPractice.Application.UseCases.Product;
using DDDPractice.Application.UseCases.Seller;
using DDDPractice.Application.UseCases.Stock;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<IOrderReservationService, OrderReservationService>();
builder.Services.AddScoped<OrderReservationService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ISellerService, SellerService>();
builder.Services.AddScoped<SellerService>();
builder.Services.AddScoped<IStockService, StockService>();


// Repositories
builder.Services.AddScoped<IOrderReservationRepository, OrderReservationRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISellerRepository, SellerRepository>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IReservationFeeCalculate, ReservationFeeCalculate>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

// Use Case
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<RegisterUserUseCase>();

builder.Services.AddScoped<CreateCustomerUseCase>();
builder.Services.AddScoped<UpdateCustomerUseCase>();
builder.Services.AddScoped<DeleteCustomerUseCase>();
builder.Services.AddScoped<GetAllCustomerUseCase>();
builder.Services.AddScoped<GetCustomerUseCase>();

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


// Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var jwtSection = builder.Configuration.GetSection("jwt");
var key = Encoding.UTF8.GetBytes(jwtSection["key"] ?? string.Empty);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; 
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSection["Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    await IdentitySeeder.SeedRolesAsync(roleManager);
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseCors("AllowSpecificOrigin");

app.Run();


