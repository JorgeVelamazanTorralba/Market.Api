using Market.Api.Business.Contracts;
using Market.Api.Vendor;
using System.Collections.Generic;

namespace Market.Api.Business.Implementations
{
    public class CatalogService : ICatalogService
    {
        private readonly ICatalogVendor<VendorOneCatalog> _catalogVendorOne;
        private readonly ICatalogVendor<VendorTwoCatalog> _catalogVendorTwo;

        public CatalogService(
            ICatalogVendor<VendorOneCatalog> catalogVendorOne,
            ICatalogVendor<VendorTwoCatalog> catalogVendorTwo)
        {
            _catalogVendorOne = catalogVendorOne;
            _catalogVendorTwo = catalogVendorTwo;
        }

        public List<IVendorCatalog> GetCatalog<T>()
        {
            return typeof(T) == typeof(VendorOneCatalog) ?
                _catalogVendorOne.GetCatalog().ConvertAll(item => (IVendorCatalog)item) :
                _catalogVendorTwo.GetCatalog().ConvertAll(item => (IVendorCatalog)item);

        }
    }
}
