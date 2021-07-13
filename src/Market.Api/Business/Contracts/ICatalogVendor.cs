using System.Collections.Generic;

namespace Market.Api.Business.Contracts
{
    public interface ICatalogVendor<T>
    {
        public List<T> GetCatalog();
    }
}
