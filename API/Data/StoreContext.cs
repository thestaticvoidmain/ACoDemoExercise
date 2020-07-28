using System.Reflection;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
  public class StoreContext : DbContext
  {
    public StoreContext(DbContextOptions<StoreContext> options) : base(options)
    {

    }

    public DbSet<Package> Packages { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<DiscountType> DiscountTypes { get; set; }
    public DbSet<Promo> Promos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }


  }
}