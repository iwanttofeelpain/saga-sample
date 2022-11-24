using Business.Engine.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business.Engine.DbContexts;

public class BusinessContext : DbContext
{
    public BusinessContext(DbContextOptions<BusinessContext> options) : base(options)
    {
    }

    public DbSet<BusinessWork> BusinessWorks { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("business");
        modelBuilder.ApplyConfigurationsFromAssembly(assembly: typeof(BusinessContext).Assembly);
    }
}