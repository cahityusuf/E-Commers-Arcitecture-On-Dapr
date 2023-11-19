using ECommers.Data;
using Order.Domain.AggregatesModel.AddressAggregate;
using Order.Infrastructure.DbContexts;

namespace Order.Infrastructure.Repositories
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(OrderDbContext dbContext) : base(dbContext)
        {
        }
    }
}
