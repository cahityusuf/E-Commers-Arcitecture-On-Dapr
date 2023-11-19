using ECommers.Abstraction.Model;
using MediatR;
using Product.Application.IntegrationEvents.Events;
using Product.Domain.AggregatesModel.CatalogItemAggregate;

namespace Product.Application.Services.OrderStatusChangedToPaid
{
    public class OrderStatusChangedToPaidCommand : IRequest<Result<List<CatalogItem>>>
    {
        public OrderStatusChangedToPaidCommand(Guid orderId, IEnumerable<OrderStockItem> orderStockItems)
        {
            OrderId = orderId;
            OrderStockItems = orderStockItems;
        }

        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public IEnumerable<OrderStockItem> OrderStockItems { get; set; }
    }
}
