using Microsoft.EntityFrameworkCore;

public class dbContext : DbContext
{
    public dbContext(DbContextOptions<dbContext> options)
       : base(options)
    {
    }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }

    public DbSet<Project> Projects { get; set; }
    public DbSet<Manager> Managers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure Employee entity
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Salary).HasColumnType("decimal(18,2)");
        });

        // Configure Department entity
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Name).IsRequired().HasMaxLength(50);
        });

        // Configure relationships
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId);
            
        // Configure Manager inheritance
        modelBuilder.Entity<Manager>()
            .HasBaseType<Employee>();

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            entity.Property(p => p.StartDate).IsRequired();
            entity.Property(p => p.EndDate).IsRequired();
        });
    }
}