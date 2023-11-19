using Microsoft.EntityFrameworkCore;
using Product.Domain.AggregatesModel.CatalogBrandAggregate;
using Product.Domain.AggregatesModel.CatalogItemAggregate;
using Product.Domain.AggregatesModel.CatalogTypeAggregate;
using System.Reflection;

namespace Product.Infrastructure.DbContexts
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }

        public DbSet<CatalogBrand> CatalogBrand => Set<CatalogBrand>();
        public DbSet<CatalogItem> CatalogItem => Set<CatalogItem>();
        public DbSet<CatalogType> CatalogType => Set<CatalogType>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
