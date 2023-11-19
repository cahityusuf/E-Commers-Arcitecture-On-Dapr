using ECommers.Data;
using Order.Domain.AggregatesModel.OrderItemAggregate;
using Order.Infrastructure.DbContexts;

namespace Order.Infrastructure.Repositories
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(OrderDbContext dbContext) : base(dbContext)
        {
        }
    }
}
