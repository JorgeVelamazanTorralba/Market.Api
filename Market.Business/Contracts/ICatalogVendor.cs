using System.Collections.Generic;

namespace Market.Business.Contracts
{
    public interface ICatalogVendor<T>
    {
        public List<T> GetCatalog();
    }
}
