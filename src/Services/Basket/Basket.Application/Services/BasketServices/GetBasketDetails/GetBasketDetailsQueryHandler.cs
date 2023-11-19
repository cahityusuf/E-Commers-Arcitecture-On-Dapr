using Basket.Abstraction.Dtos;
using ECommers.Abstraction.Model;
using ECommers.Dapr.Abstractions;
using MediatR;

namespace Basket.Application.Services.BasketServices.GetBasketDetails
{
    public class GetBasketDetailsQueryHandler : IRequestHandler<GetBasketDetailsQuery, Result<BasketDto>>
    {
        private readonly IDaprStateStore _daprStateStore;
        public GetBasketDetailsQueryHandler(IDaprStateStore daprStateStore)
        {
            _daprStateStore = daprStateStore;
        }
        private const string STATE_STORE_NAME = "statestore";
        public async Task<Result<BasketDto>> Handle(GetBasketDetailsQuery request, CancellationToken cancellationToken)
        {

            return new SuccessResult<BasketDto>(
                await _daprStateStore.GetStateAsync<BasketDto>(STATE_STORE_NAME, request.CustomerId))
            {
                Messages = new List<string>
                    {
                        "Ürün sepete kaydedildi"
                    }
            };
        }
    }
}