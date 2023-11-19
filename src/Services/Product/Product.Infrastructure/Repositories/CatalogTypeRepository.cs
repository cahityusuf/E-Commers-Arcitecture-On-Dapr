using ECommers.Data;
using Product.Domain.AggregatesModel.CatalogTypeAggregate;
using Product.Infrastructure.DbContexts;

namespace Product.Infrastructure.Repositories
{
    public class CatalogTypeRepository : Repository<CatalogType>, ICatalogTypeRepository
    {
        public CatalogTypeRepository(ProductDbContext dbContext) : base(dbContext)
        {
        }
    }
}
