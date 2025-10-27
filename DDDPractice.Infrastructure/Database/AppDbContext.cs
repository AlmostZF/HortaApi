
using DDD_Practice.DDDPractice.Domain;
using DDD_Practice.DDDPractice.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DDD_Practice.DDDPractice.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<ProductEntity> Product { get; set; }
    public DbSet<SellerEntity> Seller { get; set; }
    public DbSet<StockEntity> Stock { get; set; }
    public DbSet<UserEntity> User { get; set; }
    
    public DbSet<OrderReservationItemEntity> OrderReservationItem { get; set; }
    public DbSet<OrderReservationEntity> OrderReservation { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.OwnsOne(u => u.SecurityCode, sb =>
            {
                sb.Property(sc => sc.Value)
                    .HasColumnName("SecurityCode")
                    .IsRequired();
            });
            
        });
        
        modelBuilder.Entity<SellerEntity>()
            .OwnsOne(o => o.PickupLocation);
        
        modelBuilder.Entity<OrderReservationEntity>()
            .OwnsOne(o => o.PickupLocation);

    }
}
