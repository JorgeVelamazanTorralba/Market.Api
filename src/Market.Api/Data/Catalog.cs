using Market.Api.Business.Contracts;
using Market.Api.Vendor;
using System.Collections.Generic;

namespace Market.Api.Data
{
    public class Catalog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ExternalId { get; set; }
        public decimal Amount { get; set; }
        public string ExtraFieldOne { get; set; }
        public string ExtraFieldTwo { get; set; }
        public int VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        public void Update<T>(IVendorCatalog vendorCatalog, Data.Vendor vendor)
        {
            VendorId = vendor.Id;
            Amount = vendorCatalog.Amount;

            if (typeof(T) == typeof(VendorOneCatalog))
            {
                var source = (VendorOneCatalog)vendorCatalog;
                ExtraFieldOne = source.Sku;
                ExtraFieldTwo = source.CurrencyCode;
            }
            else
            {
                var source = (VendorTwoCatalog)vendorCatalog;
                ExtraFieldOne = source.Type;
                ExternalId = source.Ean;
            }
        }
    }
}