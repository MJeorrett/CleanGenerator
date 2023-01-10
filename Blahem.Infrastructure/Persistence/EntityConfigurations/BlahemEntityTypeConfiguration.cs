using Blahem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blahem.Infrastructure.Persistence.EntityConfigurations;

public class BlahemEntityTypeConfiguration : IEntityTypeConfiguration<BlahemEntity>
{
    public void Configure(EntityTypeBuilder<BlahemEntity> builder)
    {
        builder.ToTable("Blahem");

        builder.Property(_ => _.Id)
            .HasColumnName("BlahemId");
    }
}
