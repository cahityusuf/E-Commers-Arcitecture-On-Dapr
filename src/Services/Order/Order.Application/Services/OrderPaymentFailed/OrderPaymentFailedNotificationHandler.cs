using Dapr.Actors;
using Dapr.Actors.Client;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Application.Actors;
using Order.Application.Services.OrderStockConfirmed;

namespace Order.Application.Services.OrderPaymentFailed
{
    public class OrderPaymentFailedNotificationHandler : INotificationHandler<OrderPaymentFailedNotification>
    {
        private readonly IActorProxyFactory _actorProxyFactory;
        private readonly ILogger<OrderStockConfirmedNotificationHandler> _logger;
        public OrderPaymentFailedNotificationHandler(
            IActorProxyFactory actorProxyFactory, 
            ILogger<OrderStockConfirmedNotificationHandler> logger)
        {
            _actorProxyFactory = actorProxyFactory;
            _logger = logger;
        }
        public async Task Handle(OrderPaymentFailedNotification notification, CancellationToken cancellationToken)
        {
            var actorId = new ActorId(notification.OrderId.ToString());
            var process = _actorProxyFactory.CreateActorProxy<IOrderingProcessActor>(
                actorId,
                nameof(OrderingProcessActor));

            await process.PaymentFailedSimulatedWorkDoneAsync();
        }
    }
}
