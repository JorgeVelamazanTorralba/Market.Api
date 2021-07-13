using Market.Data.Models;

namespace Market.Business.Contracts
{
    public interface IOrderService
    {
        public Order CreateVendorResponseByCatalog(Catalog product, int? id);
    }
}
