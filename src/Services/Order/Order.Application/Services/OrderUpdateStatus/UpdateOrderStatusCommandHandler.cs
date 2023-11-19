using AutoMapper;
using ECommers.Abstraction.Model;
using MediatR;
using Order.Application.Models;
using Order.Domain.AggregatesModel.OrderAggregate;

namespace Order.Application.Services.OrderUpdateStatus
{
    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, Result<OrderDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public UpdateOrderStatusCommandHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public async Task<Result<OrderDto>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.Id);

            if (order is not null)
            {
                order.OrderStatus = request.OrderStatus;
                order.Description = request.Description;

                await _orderRepository.UpdateAsync(order);

                return new SuccessResult<OrderDto>(_mapper.Map<OrderDto>(order))
                {
                    Messages = new List<string>
                    {
                        "Sipariş kabul edildi, sipariş işlemi kaydedildi"
                    }
                };
            }

            return new NoContentResult<OrderDto>();
        }
    }
}
