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
        
        modelBuilder.Entity<ProductEntity>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.ProductType).HasConversion<string>();
        });
        
        modelBuilder.Entity<StockEntity>(entity =>
        {
            entity.HasKey(s => s.Id);
            
        });
        
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
        
        modelBuilder.Entity<SellerEntity>(entity =>
        {
            var navigation = entity.Metadata.FindNavigation(nameof(SellerEntity.PickupLocations));
            navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
            
            entity.HasMany(s => s.PickupLocations)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PickupLocationEntity>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.OwnsOne(p => p.Address);
            entity.OwnsMany(p => p.AvailablePickupDays);
        });
        

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
        
        modelBuilder.Entity<OrderReservationEntity>(entity =>
        {
            entity.HasKey(o => o.Id);
            
            var navigation = entity.Metadata.FindNavigation(nameof(OrderReservationEntity.ListOrderItems));
            navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
            
            entity.Property(o => o.SecurityCode)
                .HasConversion(v => v != null ? v.Value : null, v => v != null ? new SecurityCode(v) : null)
                .HasColumnName("SecurityCode");


            entity.OwnsOne(o => o.GuessCustomer, guest =>
            {
                guest.Property(g => g.FullName).HasColumnName("GuestFullName");
                guest.Property(g => g.Email).HasColumnName("GuestEmail");
                guest.Property(g => g.PhoneNumber).HasColumnName("GuestPhoneNumber");
            });
            
            entity.HasMany(o => o.ListOrderItems)
                .WithOne()
                .HasForeignKey(i => i.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.Property(o => o.OrderStatus).HasConversion<string>();
        });
        
        
        modelBuilder.Entity<OrderReservationItemEntity>(entity =>
        {
            entity.HasOne(i => i.Product)
                .WithMany() 
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(i => i.Seller)
                .WithMany()
                .HasForeignKey(i => i.SellerId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
