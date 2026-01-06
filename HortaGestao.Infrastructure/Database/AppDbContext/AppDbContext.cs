using HortaGestao.Domain.Entities;
using HortaGestao.Domain.ValueObjects;
using HortaGestao.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HortaGestao.Infrastructure.Database.AppDbContext;

public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public DbSet<ProductEntity> Product { get; set; }
    public DbSet<SellerEntity> Seller { get; set; }
    public DbSet<CustomerEntity> Customer { get; set; }
    public DbSet<StockEntity> Stock { get; set; }
    public DbSet<RefreshTokenEntity> RefreshToken { get; set; }
    
    public DbSet<OrderReservationItemEntity> OrderReservationItem { get; set; }
    public DbSet<OrderReservationEntity> OrderReservation { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SystemUserEntity>().UseTpcMappingStrategy();
        
        modelBuilder.Entity<ApplicationUser>()
            .HasOne(a => a.SystemUser)
            .WithOne()
            .HasForeignKey<ApplicationUser>(a => a.SystemUserId)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<CustomerEntity>(entity =>
        {
            entity.Property(u => u.SecurityCode)
                .HasConversion(
                    v => v != null ? v.Value : null,
                    v => v != null ? new SecurityCode(v) : null
                )
                .HasColumnName("SecurityCode")
                .IsRequired();
        });
        
        modelBuilder.Entity<SellerEntity>()
            .HasMany(s => s.PickupLocations)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PickupLocationEntity>()
            .OwnsOne(p => p.Address);

        modelBuilder.Entity<PickupLocationEntity>()
            .OwnsMany(p => p.AvailablePickupDays);

        modelBuilder.Entity<OrderReservationEntity>(entity =>
        {
            entity.Property(o => o.SecurityCode)
                .HasConversion(
                    v => v != null ? v.Value : null,
                    v => v != null ? new SecurityCode(v) : null
                )
                .HasColumnName("SecurityCode")
                .IsRequired(false);
        });

    }
}
