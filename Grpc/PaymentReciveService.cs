using PaymentRecive.API.Infrastructure;
using PaymentRecive.API.Model;
using PaymentRecive.API.Proto;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentRecive.API.Grpc
{
    public class PaymentReciveService : PaymentRecive.API.Proto.PaymentReciveGrpc.CatalogGrpcBase, IPaymentReciveService
    {
        private readonly ILogger<PaymentReciveService> _logger;
        private readonly PaymentReciveContext _paymentReciveContext;

        public PaymentReciveService(
            ILogger<PaymentReciveService> logger,
            PaymentReciveContext paymentlogContext)
        {
            _logger = logger;
            _paymentReciveContext = paymentlogContext;
        }

        public override async Task<CatalogItemResponse> GetItemById(CatalogItemRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call CatalogService.ProductId for product id {Id}", request.ProductId);
            if (request.ProductId <= 0)
            {
                context.Status = new Status(StatusCode.FailedPrecondition, $"Id must be > 0 (received {request.ProductId})");
                return null;
            }

            var _product = await _paymentReciveContext.Product.Include(x => x.ProductInStocks).FirstOrDefaultAsync(x => x.ProductId == request.ProductId);

            if (_product != null)
            {
                return new CatalogItemResponse()
                {
                    ProductId = _product.ProductId,
                    Description = _product.Description,
                    Name = _product.Name,
                    Price = _product.ProductId,
                    AvailableStock = _product.ProductInStocks.First().Stock
                };
            }

            context.Status = new Status(StatusCode.NotFound, $"Client with id {request.ProductId} do not exist");
            return null;
            //return base.GetItemById(request, context);
        }

        public override async Task<PaginatedItemsResponse> GetItemsByIds(CatalogItemsRequest request, ServerCallContext context)
        {
            if (!string.IsNullOrEmpty(request.Ids))
            {
                var items = await GetItemsByIdsAsync(request.Ids);

                context.Status = !items.Any() ?
                    new Status(StatusCode.NotFound, $"ids value invalid. Must be comma-separated list of numbers") :
                    new Status(StatusCode.OK, string.Empty);

                return this.MapToResponse(items);
            }

            var totalItems = await _paymentReciveContext.AgreementPayment.LongCountAsync();

            var itemsOnPage = await _paymentReciveContext.AgreementPayment
                .OrderBy(c => c.AgreementId)
                .Skip(request.PageSize * request.PageIndex)
                .Take(request.PageSize)
                .ToListAsync();

            var model = this.MapToResponse(itemsOnPage, totalItems, request.PageIndex, request.PageSize);
            context.Status = new Status(StatusCode.OK, string.Empty);

            return model;
        }

        private async Task<List<AgreementPayments>> GetItemsByIdsAsync(string ids)
        {
            var numIds = ids.Split(',').Select(id => (Ok: int.TryParse(id, out int x), Value: x));

            if (!numIds.All(nid => nid.Ok))
            {
                return new List<AgreementPayments>();
            }

            var idsToSelect = numIds.Select(id => id.Value);

            var items = await _catalogContext.AgreementPayments.Include(x => x.ProductInStocks).Where(ci => idsToSelect.Contains(ci.ProductId)).ToListAsync();

            return items;
        }

        private PaginatedItemsResponse MapToResponse(List<Product> items)
        {
            return this.MapToResponse(items, items.Count, 1, items.Count);
        }

        private PaginatedItemsResponse MapToResponse(List<Product> items, long count, int pageIndex, int pageSize)
        {
            var result = new PaginatedItemsResponse()
            {
                Count = count,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };

            items.ForEach(i =>
            {
                result.Data.Add( new CatalogItemResponse()
                {
                    ProductId = i.ProductId,
                    Description = i.Description,
                    Name = i.Name,
                    Price = i.ProductId,
                    AvailableStock = i.ProductInStocks.First().Stock
                });
            });

            return result;
        }


    }
}
