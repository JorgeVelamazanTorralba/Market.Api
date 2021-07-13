using Market.Api.Data;

namespace Market.Api.Business.Contracts
{
    public interface IOrderService
    {
        public Order CreateVendorResponseByCatalog(Catalog product, int? id);
    }
}
