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
    }
}