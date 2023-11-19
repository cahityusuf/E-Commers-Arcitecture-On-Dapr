using Basket.Application.IntegrationEvents.Events;
using ECommers.Dapr.Abstractions;

namespace Basket.Application.IntegrationEvents.EventHandling;

public class OrderStatusChangedToSubmittedIntegrationEventHandler
    : IIntegrationEventHandler<OrderStatusChangedToSubmittedIntegrationEvent>
{
    private readonly IDaprStateStore _daprStateStore;

    public OrderStatusChangedToSubmittedIntegrationEventHandler(
        IDaprStateStore daprStateStore)
    {
        _daprStateStore= daprStateStore;
    }
    private const string DAPR_PUBSUB_NAME = "pubsub";
    public Task Handle(OrderStatusChangedToSubmittedIntegrationEvent @event) =>
        _daprStateStore.DeleteStateAsync(DAPR_PUBSUB_NAME,@event.BuyerId);
}



