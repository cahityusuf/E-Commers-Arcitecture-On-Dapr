using ECommers.Dapr.Events;

namespace Order.Application.IntegrationEvents;

public record GracePeriodConfirmedIntegrationEvent(int OrderId) : IntegrationEvent;
