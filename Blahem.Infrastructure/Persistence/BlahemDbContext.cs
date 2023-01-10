using Blahem.Application.Common.Interfaces;
using Blahem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Blahem.Infrastructure.Persistence;

public class BlahemDbContext : DbContext, IBlahemDbContext
{
    public DbSet<BlahemEntity> Blahems { get; init; }

    public BlahemDbContext(DbContextOptions<BlahemDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
