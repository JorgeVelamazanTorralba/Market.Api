using System;
using System.Collections.Generic;

namespace Market.Business.Contracts
{
    public interface ICatalogService
    {
        public List<IVendorCatalog> GetCatalog<T>();
    }
}
