using System.Collections.Generic;

namespace Market.Api.Business.Contracts
{
    public interface ICatalogService
    {
        public List<IVendorCatalog> GetCatalog<T>();
    }
}
