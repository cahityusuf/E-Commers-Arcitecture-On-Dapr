using AutoMapper;
using Dapr.Actors;
using Dapr.Actors.Client;
using ECommers.Abstraction.Model;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Application.Actors;
using Order.Application.Models;
using Order.Domain.AggregatesModel.AddressAggregate;
using Order.Domain.AggregatesModel.OrderAggregate;
using Order.Domain.AggregatesModel.OrderItemAggregate;

namespace Order.Application.Services.OrderStatusChanged
{
    public class OrderStatusChangedCommandHandler : IRequestHandler<OrderStatusChangedCommand, Result<OrderDto>>
    {
        private readonly IActorProxyFactory _actorProxyFactory;
        private readonly ILogger<OrderStatusChangedCommandHandler> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEventBus _eventBus;
        public OrderStatusChangedCommandHandler(
            IActorProxyFactory actorProxyFactory,
            ILogger<OrderStatusChangedCommandHandler> logger,
            IMapper mapper,
            IEventBus eventBus,
            IOrderRepository orderRepository)
        {
            _actorProxyFactory = actorProxyFactory;
            _logger = logger;
            _mapper = mapper;
            _eventBus = eventBus;
            _orderRepository = orderRepository;
        }

        public async Task<Result<OrderDto>> Handle(OrderStatusChangedCommand request, CancellationToken cancellationToken)
        {

            var actorId = new ActorId(request.OrderId.ToString());
            var orderingProcess = _actorProxyFactory.CreateActorProxy<IOrderingProcessActor>(
                actorId,
                nameof(OrderingProcessActor));

            var state = await orderingProcess.GetOrderDetails();

            var readModelOrder = new Domain.AggregatesModel.OrderAggregate.Order()
            {
                Id = request.OrderId,
                OrderDate = state.OrderDate,
                OrderStatus = state.OrderStatus.Name,
                BuyerId = state.BuyerId,
                BuyerEmail = state.BuyerEmail,
                Description = state.Description,
                Address = new Address()
                {
                    Id = Guid.NewGuid(),
                    Street = state.Address.Street,
                    City = state.Address.City,
                    State = state.Address.State,
                    Country = state.Address.Country
                },
                OrderItems = state.OrderItems
                    .Select(itemState => new OrderItem()
                    {
                        Id = Guid.NewGuid(),
                        OrderId = request.OrderId,
                        PictureFileName = itemState.PictureFileName,
                        ProductId = itemState.ProductId,
                        ProductName = itemState.ProductName,
                        UnitPrice = itemState.UnitPrice,
                        Units = itemState.Units
                    })
                    .ToList()
            };

            try
            {
                await _orderRepository.AddAsync(readModelOrder);

                await orderingProcess.OrderStatusChangedToSubmittedAsync();
            }
            catch (Exception ex)
            {

                throw;
            }


            return new SuccessResult<OrderDto>(_mapper.Map<OrderDto>(readModelOrder))
            {
                Messages = new List<string>
                    {
                        "Sipariş kabul edildi, sipariş işlemi kaydedildi"
                    }
            };

        }
    }
}
