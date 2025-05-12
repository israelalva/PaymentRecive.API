using Catalog.API.Proto;
using Grpc.Core;
using System.Threading.Tasks;

namespace PaymentRecive.API.Grpc
{
    public interface IPaymentReciveService
    {
        Task<CatalogItemResponse> GetItemById(CatalogItemRequest request, ServerCallContext context);
    }
}