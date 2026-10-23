using LeadLane.Domain.Identity.Models;
using LeadLane.Domain.Leads.Models;
using Microsoft.EntityFrameworkCore;

namespace LeadLane.Infrastructure.Persistence;

public class LeadLaneDbContext(DbContextOptions<LeadLaneDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Lead> Leads => Set<Lead>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LeadLaneDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
