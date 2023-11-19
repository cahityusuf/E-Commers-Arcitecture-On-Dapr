

using Basket.Abstraction.Dtos;
using ECommers.Abstraction.Model;
using MediatR;

namespace Basket.Application.Services.BasketServices.GetBasketDetails
{
    public class GetBasketDetailsQuery : IRequest<Result<BasketDto>>
    {
        public GetBasketDetailsQuery(string customerId)
        {
            CustomerId = customerId;
        }

        public string CustomerId { get; }
    }
}