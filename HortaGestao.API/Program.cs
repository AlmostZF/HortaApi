using System.Text;
using HortaGestao.API.Middleware;
using HortaGestao.Application.Interfaces;
using HortaGestao.Application.Interfaces.UnitOfWork;
using HortaGestao.Application.UseCases.Authentication;
using HortaGestao.Infrastructure.Database.AppDbContext;
using HortaGestao.Infrastructure.Database.UnitOfWork;
using HortaGestao.Infrastructure.DependecyInjection;
using HortaGestao.Infrastructure.Identity;
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
            policy.WithOrigins("http://localhost:4200", "http://10.0.0.199:4200", "http://10.0.0.166:4200" )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});


builder.Services.AddInfrastructure();
builder.Services.AddApplication();

builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<RegisterUserUseCase>();

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

app.UseMiddleware<JwtAuthenticationMiddleware>(jwtSection["key"]);

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


app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();


