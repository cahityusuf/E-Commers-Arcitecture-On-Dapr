using Microsoft.EntityFrameworkCore;
using Product.Domain.AggregatesModel.CatalogBrandAggregate;
using Product.Domain.AggregatesModel.CatalogItemAggregate;
using Product.Domain.AggregatesModel.CatalogTypeAggregate;

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
            modelBuilder.Entity<CatalogItem>()
                    .Property(item => item.Price)
                    .HasPrecision(18, 2); // Hassasiyet ve ölçek belirleme

            var brands = new List<CatalogBrand>
            {
                new CatalogBrand { Id = Guid.Parse("eea0eee4-519f-4d04-8040-27ad3be3957b"), Name = "Teknoloji Dünyası" },
                new CatalogBrand { Id = Guid.Parse("b7d397e0-092f-4372-8189-b4562ea507f8"), Name = "Ev ve Yaşam" },
                new CatalogBrand { Id = Guid.Parse("4c406591-6646-4af0-9822-913bf6a7f71c"), Name = "Sporcu Eşyaları" }
            };
            modelBuilder.Entity<CatalogBrand>().HasData(brands);

            // Seed data for CatalogType
            var types = new List<CatalogType>
            {
                new CatalogType { Id = Guid.Parse("99035157-e1fd-4cb1-8b50-d5a9d08123b2"), Name = "Elektronik" },
                new CatalogType { Id = Guid.Parse("d5bb70b6-929e-4826-9592-97f79af6dde5"), Name = "Mobilya" },
                new CatalogType { Id = Guid.Parse("b53b7be8-952a-43aa-962b-a98b4bf52a16"), Name = "Spor Giyim" }
            };
            modelBuilder.Entity<CatalogType>().HasData(types);

            // Seed data for CatalogItem
            var items = new List<CatalogItem>
            {
                new CatalogItem { Id = Guid.Parse("54c53ef8-f722-443c-972a-2f1cf7770607"), Name = "Akıllı Telefon", Price = 4999.99M, PictureFileName = "smartphone.png", CatalogTypeId = types[0].Id, CatalogBrandId = brands[0].Id, AvailableStock = 100 },
                new CatalogItem { Id = Guid.Parse("d1840a42-c08f-4ca6-bb21-a436f1474692"), Name = "Kanepe", Price = 2999.99M, PictureFileName = "sofa.png", CatalogTypeId = types[1].Id, CatalogBrandId = brands[1].Id, AvailableStock = 50 },
                new CatalogItem { Id = Guid.Parse("024b5c81-caa2-42c1-9d00-61be131107f1"), Name = "Koşu Ayakkabısı", Price = 749.99M, PictureFileName = "running_shoes.png", CatalogTypeId = types[2].Id, CatalogBrandId = brands[2].Id, AvailableStock = 200 },
            };
            modelBuilder.Entity<CatalogItem>().HasData(items);


            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
