using Blahem.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blahem.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<BlahemEntity> Blahems { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
