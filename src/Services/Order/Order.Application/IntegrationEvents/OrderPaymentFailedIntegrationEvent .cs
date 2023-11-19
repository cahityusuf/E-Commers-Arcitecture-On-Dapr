using ECommers.Dapr.Events;

namespace Order.Application.IntegrationEvents;

public record OrderPaymentFailedIntegrationEvent(Guid OrderId) : IntegrationEvent;
