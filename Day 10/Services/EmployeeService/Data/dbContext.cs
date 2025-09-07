using Microsoft.EntityFrameworkCore;

public class dbContext : DbContext
{
    public dbContext(DbContextOptions<dbContext> options)
       : base(options)
    {
    }
    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure Employee entity
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Salary).HasColumnType("decimal(18,2)");
        });
    }
}