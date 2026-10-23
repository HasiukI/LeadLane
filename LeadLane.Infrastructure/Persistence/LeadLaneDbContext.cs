using LeadLane.Domain.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace LeadLane.Infrastructure.Persistence;

public class LeadLaneDbContext(DbContextOptions<LeadLaneDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LeadLaneDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
