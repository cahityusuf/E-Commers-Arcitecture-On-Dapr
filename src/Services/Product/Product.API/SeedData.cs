using Product.Domain.AggregatesModel.CatalogBrandAggregate;
using Product.Domain.AggregatesModel.CatalogItemAggregate;
using Product.Domain.AggregatesModel.CatalogTypeAggregate;
using Product.Infrastructure.DbContexts;

namespace Product.API
{
    public class SeedData
    {
        public static void InitializeDatabase(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ProductDbContext>();

                context.Database.EnsureCreated();

                SeedCatalogBrands(context);
                SeedCatalogTypes(context);
                SeedCatalogItems(context);

                context.SaveChanges();
            }
        }

        private static void SeedCatalogBrands(ProductDbContext context)
        {
            // Benzer şekilde, CatalogBrand için seed datayı kontrol edip ekle
            // Örnek:
            if (!context.CatalogBrand.Any())
            {
                context.CatalogBrand.AddRange(
                    new CatalogBrand { Id = Guid.Parse("eea0eee4-519f-4d04-8040-27ad3be3957b"), Name = "Teknoloji Dünyası" },
                    new CatalogBrand { Id = Guid.Parse("b7d397e0-092f-4372-8189-b4562ea507f8"), Name = "Ev ve Yaşam" },
                    new CatalogBrand { Id = Guid.Parse("4c406591-6646-4af0-9822-913bf6a7f71c"), Name = "Sporcu Eşyaları" }
            );
            }
        }

        private static void SeedCatalogTypes(ProductDbContext context)
        {
            // Benzer şekilde, CatalogType için seed datayı kontrol edip ekle
            // Örnek:
            if (!context.CatalogType.Any())
            {
                context.CatalogType.AddRange(
                    new CatalogType { Id = Guid.Parse("99035157-e1fd-4cb1-8b50-d5a9d08123b2"), Name = "Elektronik" },
                    new CatalogType { Id = Guid.Parse("d5bb70b6-929e-4826-9592-97f79af6dde5"), Name = "Mobilya" },
                    new CatalogType { Id = Guid.Parse("b53b7be8-952a-43aa-962b-a98b4bf52a16"), Name = "Spor Giyim" }
                );
            }
        }

        private static void SeedCatalogItems(ProductDbContext context)
        {
            // Benzer şekilde, CatalogType için seed datayı kontrol edip ekle
            // Örnek:
            if (!context.CatalogItem.Any())
            {
                context.CatalogItem.AddRange(
                    new CatalogItem { 
                        Id = Guid.Parse("54c53ef8-f722-443c-972a-2f1cf7770607"), 
                        Name = "Akıllı Telefon", 
                        Price = 4999.99M, 
                        PictureFileName = "smartphone.png", 
                        CatalogTypeId = Guid.Parse("99035157-e1fd-4cb1-8b50-d5a9d08123b2"), 
                        CatalogBrandId = Guid.Parse("eea0eee4-519f-4d04-8040-27ad3be3957b"), 
                        AvailableStock = 100 },
                    new CatalogItem { 
                        Id = Guid.Parse("d1840a42-c08f-4ca6-bb21-a436f1474692"), 
                        Name = "Kanepe", 
                        Price = 2999.99M, 
                        PictureFileName = "sofa.png", 
                        CatalogTypeId = Guid.Parse("d5bb70b6-929e-4826-9592-97f79af6dde5"), 
                        CatalogBrandId = Guid.Parse("b7d397e0-092f-4372-8189-b4562ea507f8"), 
                        AvailableStock = 50 },
                    new CatalogItem { 
                        Id = Guid.Parse("024b5c81-caa2-42c1-9d00-61be131107f1"), 
                        Name = "Koşu Ayakkabısı", 
                        Price = 749.99M, 
                        PictureFileName = "running_shoes.png", 
                        CatalogTypeId = Guid.Parse("b53b7be8-952a-43aa-962b-a98b4bf52a16"), 
                        CatalogBrandId = Guid.Parse("4c406591-6646-4af0-9822-913bf6a7f71c"), 
                        AvailableStock = 200 }
                );
            }
        }
    }
}
