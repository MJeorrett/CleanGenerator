using Blahem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blahem.Infrastructure.Persistence.EntityConfigurations;

public class BlahemEntityTypeConfiguration : IEntityTypeConfiguration<BlaitemEntity>
{
    public void Configure(EntityTypeBuilder<BlaitemEntity> builder)
    {
        builder.ToTable("Blaitem");

        builder.Property(_ => _.Id)
            .HasColumnName("BlaitemId");
    }
}
