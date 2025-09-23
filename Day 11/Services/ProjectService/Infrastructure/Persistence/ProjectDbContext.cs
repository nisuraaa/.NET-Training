using Microsoft.EntityFrameworkCore;
using ProjectService.Domain.Entities;

namespace ProjectService.Infrastructure.Persistence;

public class ProjectDbContext : DbContext
{
    public ProjectDbContext(DbContextOptions<ProjectDbContext> options)
       : base(options)
    {
    }
    public DbSet<Project> Projects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Name)
                  .IsRequired()
                  .HasMaxLength(50)
                  .UseCollation("NOCASE"); // SQLite: make comparisons case-insensitive

            entity.HasIndex(d => d.Name)
                  .IsUnique();
        });

    }
}