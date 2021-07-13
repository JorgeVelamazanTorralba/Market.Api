using Market.Api.Data;

namespace Market.Api.Business.Contracts
{
    public interface IVendorCatalog
    {
        public decimal Amount { get; set; }
        
        public Catalog CreateCatalog(Data.Vendor vendor);
    }
}
