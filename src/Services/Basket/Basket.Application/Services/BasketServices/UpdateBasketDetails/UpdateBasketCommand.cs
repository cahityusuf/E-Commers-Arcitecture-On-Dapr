using Basket.Abstraction.Dtos;
using ECommers.Abstraction.Model;
using MediatR;

namespace Basket.Application.Services.BasketServices.UpdateBasketDetails
{
    public class UpdateBasketCommand : IRequest<Result<BasketDto>>
    {

        public UpdateBasketCommand(string buyerId, List<BasketItemDto> items)
        {
            BuyerId = buyerId;
            Items = items;
        }

        public Guid Id => Guid.NewGuid();

        public string BuyerId { get; set; } = "";
        public List<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();

    }
}
