using ECommers.Data;
using Order.Domain.AggregatesModel.OrderAggregate;
using Order.Infrastructure.DbContexts;

namespace Order.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Domain.AggregatesModel.OrderAggregate.Order>, IOrderRepository
    {
        public OrderRepository(OrderDbContext dbContext) : base(dbContext)
        {
        }
    }
}
