using ECommers.Data;
using Product.Domain.AggregatesModel.CatalogBrandAggregate;
using Product.Infrastructure.DbContexts;

namespace Product.Infrastructure.Repositories
{
    public class CatalogBrandRepository :Repository<CatalogBrand>, ICatalogBrandRepository
    {
        public CatalogBrandRepository(ProductDbContext dbContext) : base(dbContext)
        {
        }
    }
}
