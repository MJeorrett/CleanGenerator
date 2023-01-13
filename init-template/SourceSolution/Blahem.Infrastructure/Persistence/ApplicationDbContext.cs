using Blahem.Application.Common.Interfaces;
using Blahem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Blahem.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<BlaitemEntity> Blaitems { get; init; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
