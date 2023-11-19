using ECommers.Data;
using Product.Domain.AggregatesModel.CatalogItemAggregate;
using Product.Infrastructure.DbContexts;

namespace Product.Infrastructure.Repositories
{
    public class CatalogItemRepository : Repository<CatalogItem>, ICatalogItemRepository
    {
        public CatalogItemRepository(ProductDbContext dbContext) : base(dbContext)
        {
        }
    }
}
