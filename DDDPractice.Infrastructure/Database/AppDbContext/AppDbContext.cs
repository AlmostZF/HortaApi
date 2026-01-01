using DDDPractice.DDDPractice.Domain;
using DDDPractice.DDDPractice.Domain.Entities;
using DDDPractice.DDDPractice.Domain.ValueObjects;
using DDDPractice.DDDPractice.Infrastructure.Identity;
using DDDPractice.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DDDPractice.DDDPractice.Infrastructure;

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
            entity.OwnsOne(u => u.SecurityCode, sb =>
            {
                sb.Property(sc => sc.Value)
                    .HasColumnName("SecurityCode")
                    .IsRequired();
            });
        });
        
        
        modelBuilder.Entity<SellerEntity>()
            .OwnsOne(o => o.PickupLocation);

        modelBuilder.Entity<OrderReservationEntity>(entity =>
        {
            entity.Property(o => o.SecurityCode)
                .HasConversion(
                    v => v != null ? v.Value : null,
                    v => v != null ? new SecurityCode(v) : null
                )
                .HasColumnName("SecurityCode")
                .IsRequired(false);
            
            entity.OwnsOne(o => o.PickupLocation);
        });

    }
}
