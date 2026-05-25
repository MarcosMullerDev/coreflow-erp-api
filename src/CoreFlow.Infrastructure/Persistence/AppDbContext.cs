using CoreFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreFlow.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Company> Companies => Set<Company>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<SaleItem> SaleItems => Set<SaleItem>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<VehicleImage> VehicleImages => Set<VehicleImage>();
    public DbSet<Lead> Leads => Set<Lead>();
    public DbSet<VehicleOptional> VehicleOptionals => Set<VehicleOptional>();
    public DbSet<CompanySettings> CompanySettings => Set<CompanySettings>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(c => c.Document)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(c => c.Phone)
                .HasMaxLength(30);

            entity.Property(c => c.CreatedAt)
                .IsRequired();

            entity.Property(c => c.IsActive)
                .IsRequired();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(u => u.PasswordHash)
                .IsRequired();

            entity.Property(u => u.Role)
                .IsRequired();

            entity.Property(u => u.IsActive)
                .IsRequired();

            entity.Property(u => u.CreatedAt)
                .IsRequired();

            entity.Property(u => u.UpdatedAt);

            entity.Property(u => u.IsDeleted)
                .IsRequired();

            entity.HasOne(u => u.Company)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(p => p.Description)
                .HasMaxLength(500);

            entity.Property(p => p.Sku)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(p => p.CostPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(p => p.SalePrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(p => p.StockQuantity)
                .IsRequired();

            entity.Property(p => p.IsActive)
                .IsRequired();

            entity.HasOne(p => p.Company)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(s => s.Id);

            entity.Property(s => s.TotalAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(s => s.Status)
                .IsRequired();

            entity.Property(s => s.CreatedAt)
                .IsRequired();

            entity.Property(s => s.UpdatedAt);

            entity.Property(s => s.IsDeleted)
                .IsRequired();

            entity.HasOne(s => s.Company)
                .WithMany(c => c.Sales)
                .HasForeignKey(s => s.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.User)
                .WithMany(u => u.Sales)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(s => s.Items)
                .WithOne(i => i.Sale)
                .HasForeignKey(i => i.SaleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SaleItem>(entity =>
        {
            entity.HasKey(i => i.Id);

            entity.Property(i => i.Quantity)
                .IsRequired();

            entity.Property(i => i.UnitPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(i => i.TotalPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(i => i.CreatedAt)
                .IsRequired();

            entity.Property(i => i.UpdatedAt);

            entity.Property(i => i.IsDeleted)
                .IsRequired();

            entity.HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(v => v.Id);

            entity.Property(v => v.Brand)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(v => v.Model)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(v => v.Year)
                .IsRequired();

            entity.Property(v => v.ModelYear)
                .IsRequired();

            entity.Property(v => v.Mileage)
                .IsRequired();

            entity.Property(v => v.Color)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(v => v.Plate)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(v => v.Chassis)
                .HasMaxLength(50);

            entity.Property(v => v.FuelType)
                .IsRequired();

            entity.Property(v => v.TransmissionType)
                .IsRequired();

            entity.Property(v => v.PurchasePrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(v => v.SalePrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(v => v.Status)
                .IsRequired();

            entity.Property(v => v.CreatedAt)
                .IsRequired();

            entity.Property(v => v.UpdatedAt);

            entity.Property(v => v.IsDeleted)
                .IsRequired();

            entity.HasOne(v => v.Company)
                .WithMany(c => c.Vehicles)
                .HasForeignKey(v => v.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.Property(v => v.IsFeatured)
                .IsRequired();
        });
        modelBuilder.Entity<VehicleImage>(entity =>
        {
            entity.HasKey(i => i.Id);

            entity.Property(i => i.FileName)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(i => i.IsPrimary)
                .IsRequired();

            entity.HasOne(i => i.Vehicle)
                .WithMany(v => v.Images)
                .HasForeignKey(i => i.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<Lead>(entity =>
        {
            entity.HasKey(l => l.Id);

            entity.Property(l => l.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(l => l.Phone)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(l => l.Email)
                .HasMaxLength(150);

            entity.Property(l => l.Message)
                .HasMaxLength(1000);

            entity.Property(l => l.Status)
                .IsRequired();

            entity.Property(l => l.CreatedAt)
                .IsRequired();

            entity.Property(l => l.UpdatedAt);

            entity.Property(l => l.IsDeleted)
                .IsRequired();

            entity.HasOne(l => l.Company)
                .WithMany(c => c.Leads)
                .HasForeignKey(l => l.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(l => l.Vehicle)
                .WithMany(v => v.Leads)
                .HasForeignKey(l => l.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<VehicleOptional>(entity =>
        {
            entity.HasKey(o => o.Id);

            entity.Property(o => o.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasOne(o => o.Vehicle)
                .WithMany(v => v.Optionals)
                .HasForeignKey(o => o.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<CompanySettings>(entity =>
        {
            entity.HasKey(s => s.Id);

            entity.Property(s => s.StoreName)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(s => s.Slug)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(s => s.Slug)
                .IsUnique();

            entity.Property(s => s.LogoUrl)
                .HasMaxLength(500);

            entity.Property(s => s.BannerUrl)
                .HasMaxLength(500);

            entity.Property(s => s.PrimaryColor)
                .HasMaxLength(20);

            entity.Property(s => s.SecondaryColor)
                .HasMaxLength(20);

            entity.Property(s => s.Whatsapp)
                .HasMaxLength(30);

            entity.Property(s => s.Instagram)
                .HasMaxLength(150);

            entity.Property(s => s.Address)
                .HasMaxLength(250);

            entity.Property(s => s.HeroTitle)
                .HasMaxLength(150);

            entity.Property(s => s.HeroSubtitle)
                .HasMaxLength(300);

            entity.HasOne(s => s.Company)
                .WithOne(c => c.Settings)
                .HasForeignKey<CompanySettings>(s => s.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}