using AutoMapper;
using ECommers.Abstraction.Model;
using MediatR;
using Product.Domain.AggregatesModel.CatalogItemAggregate;

namespace Product.Application.Services.OrderStatusChangedToPaid
{
    public class OrderStatusChangedToPaidCommandHandler : IRequestHandler<OrderStatusChangedToPaidCommand, Result<List<CatalogItem>>>
    {
        private readonly ICatalogItemRepository _context;
        private static readonly SemaphoreSlim _updateLock = new SemaphoreSlim(1, 1);
        // readonly ILogger<OrderStatusChangedToPaidCommandHandler> _logger;
        private readonly IMapper _mapper;
        public OrderStatusChangedToPaidCommandHandler(ICatalogItemRepository context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public virtual async Task<Result<List<CatalogItem>>> Handle(OrderStatusChangedToPaidCommand request, CancellationToken cancellationToken)
        {
            await _updateLock.WaitAsync();
            try
            {
                var updateModel = new List<CatalogItem>();

                foreach (var orderStockItem in request.OrderStockItems)
                {
                    var catalogItem = await _context.GetByIdAsync(orderStockItem.ProductId);

                    if (catalogItem != null)
                    {
                        catalogItem.AvailableStock -= orderStockItem.Units;

                        updateModel.Add(catalogItem);
                    }
                }

                await _context.UpdateAsync(updateModel);

                return new SuccessResult<List<CatalogItem>>(updateModel);

            }
            catch (Exception ex)
            {

                throw;
            }
            finally { _updateLock.Release(); }


        }



    }
}
