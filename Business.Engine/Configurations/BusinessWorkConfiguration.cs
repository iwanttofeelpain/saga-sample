using Business.Engine.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Business.Engine.Configurations;

public class BusinessWorkConfiguration : IEntityTypeConfiguration<BusinessWork>
{
    public void Configure(EntityTypeBuilder<BusinessWork> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.StateWorkType).HasColumnType("varchar(128)");
    }
}