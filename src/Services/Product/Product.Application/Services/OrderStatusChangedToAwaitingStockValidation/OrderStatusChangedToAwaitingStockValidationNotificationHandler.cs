using ECommers.Dapr.Events;
using MediatR;
using Product.Application.IntegrationEvents.Events;
using Product.Domain.AggregatesModel.CatalogItemAggregate;

namespace Product.Application.Services.OrderStatusChangedToAwaitingStockValidation
{
    public class OrderStatusChangedToAwaitingStockValidationNotificationHandler : INotificationHandler<OrderStatusChangedToAwaitingStockValidationNotification>
    {
        private readonly ICatalogItemRepository _context;
        private readonly IEventBus _eventBus;
        public OrderStatusChangedToAwaitingStockValidationNotificationHandler(ICatalogItemRepository context, IEventBus eventBus)
        {
            _context = context;
            _eventBus = eventBus;
        }
        public async Task Handle(OrderStatusChangedToAwaitingStockValidationNotification notification, CancellationToken cancellationToken)
        {
            var confirmedOrderStockItems = new List<ConfirmedOrderStockItem>();

            foreach (var orderStockItem in notification.OrderStockItems)
            {

                var catalogItem = await _context.GetByIdAsync(orderStockItem.ProductId);

                if (catalogItem != null)
                {
                    var hasStock = catalogItem.AvailableStock >= orderStockItem.Units;
                    var confirmedOrderStockItem = new ConfirmedOrderStockItem(catalogItem.Id, hasStock);

                    confirmedOrderStockItems.Add(confirmedOrderStockItem);
                }
            }

            var confirmedIntegrationEvent = confirmedOrderStockItems.Any(c => !c.HasStock)
                ? (IntegrationEvent)new OrderStockRejectedIntegrationEvent(notification.OrderId, confirmedOrderStockItems)
                : new OrderStockConfirmedIntegrationEvent(notification.OrderId);

            await _eventBus.PublishAsync(confirmedIntegrationEvent);
        }
    }
}
