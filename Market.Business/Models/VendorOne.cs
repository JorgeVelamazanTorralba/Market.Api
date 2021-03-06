using Market.Business.Contracts;
using System;
using System.Collections.Generic;

namespace Market.Business.Models
{
    public class VendorOne : ICatalogVendor<VendorOneCatalog>, IOrderVendor<VendorOneRequest, VendorOneResponse>
    {
        public List<VendorOneCatalog> GetCatalog()
        {
            #region [DO NOT TOUCH THIS]

            Random rnd = new Random();
            var catalog = new List<VendorOneCatalog>();
            for (int i = 1; i <= 5; i++)
            {
                catalog.Add(new VendorOneCatalog
                {
                    Amount = rnd.Next(50, 100),
                    CurrencyCode = "EUR",
                    Name = $"Product {i}",
                    Sku = i.ToString(),
                    Vendor = "Verdor 1"
                });
            }

            #endregion [DO NOT TOUCH THIS]

            return catalog;
        }

        public VendorOneResponse CreateOrder(VendorOneRequest request)
        {
            #region [DO NOT TOUCH THIS]

            if (string.IsNullOrWhiteSpace(request.Sku))
            {
                throw new Exception("Sku is null");
            }
            if (string.IsNullOrWhiteSpace(request.Vendor))
            {
                throw new Exception("Vendor is null");
            }
            return new VendorOneResponse { Sku = request.Sku, Amount = request.Amount, OrderIdentifier = Guid.NewGuid().ToString() };

            #endregion [DO NOT TOUCH THIS]
        }
    }

    public class VendorOneRequest : IVendorRequest
    {
        public string Sku { get; set; }
        public string Vendor { get; set; }
        public decimal Amount { get; set; }
    }

    public class VendorOneResponse : IVendorResponse
    {
        public string OrderIdentifier { get; set; }
        public string Sku { get; set; }
        public decimal Amount { get; set; }
    }

    public class VendorOneCatalog : IVendorCatalog
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public string Vendor { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }
}