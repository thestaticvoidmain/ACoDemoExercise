using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data.Config
{
  public class PackageConfiguration : IEntityTypeConfiguration<Package>
  {

    //For modifying default data types auto-associated by EF to the entities.
    public void Configure(EntityTypeBuilder<Package> builder)
    {
      builder.Property(p => p.Name).IsRequired().HasMaxLength(256);
      builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
    }
  }
}