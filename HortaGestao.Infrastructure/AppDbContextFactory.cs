using HortaGestao.Infrastructure.Database.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HortaGestao.Infrastructure
{
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseMySql(configuration.GetConnectionString("DefaultConnection"),
            ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection")));


        return new AppDbContext(optionsBuilder.Options);
    }
}

}