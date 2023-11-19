using ECommers.Dapr.Events;
namespace Order.Application.IntegrationEvents;

public record OrderStockRejectedIntegrationEvent(
    Guid OrderId,
    List<ConfirmedOrderStockItem> OrderStockItems)
    : IntegrationEvent;
