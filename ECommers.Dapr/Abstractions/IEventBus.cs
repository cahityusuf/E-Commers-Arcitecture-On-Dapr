using ECommers.Dapr.Events;

public interface IEventBus
{
    Task PublishAsync(IntegrationEvent integrationEvent);
}
