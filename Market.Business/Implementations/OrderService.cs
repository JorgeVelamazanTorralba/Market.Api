using Market.Business.Contracts;
using Market.Business.Models;
using Market.Data.Models;
using System;

namespace Market.Business.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderVendor<VendorOneRequest, VendorOneResponse> _orderVendorOne;
        private readonly IOrderVendor<VendorTwoRequest, VendorTwoResponse> _orderVendorTwo;

        public OrderService(     
            IOrderVendor<VendorOneRequest, VendorOneResponse> orderVendorOne,
            IOrderVendor<VendorTwoRequest, VendorTwoResponse> orderVendorTwo)
        {
            _orderVendorOne = orderVendorOne;
            _orderVendorTwo = orderVendorTwo;
        }

        public Order CreateVendorResponseByCatalog(Catalog product, int? id)
        {
            IVendorResponse vendorResponse = product.VendorId switch
            {
                1 => _orderVendorOne.CreateOrder(new VendorOneRequest { Amount = product.Amount, Sku = product.ExternalId, Vendor = product.ExtraFieldOne }),
                2 => _orderVendorTwo.CreateOrder(new VendorTwoRequest { Amount = product.Amount, Ean = product.ExternalId, Type = product.ExtraFieldOne }),
                _ => throw new ArgumentException($"Invalid vendorId: {product.VendorId}")
            };

            if (vendorResponse is null)
            {
                throw new Exception("Error to processing payment");
            }

            return new()
            {
                CatalogId = id.Value,
                ExternalId = product.VendorId == 1 ? ((VendorOneResponse)vendorResponse).OrderIdentifier : ((VendorTwoResponse)vendorResponse).TxtId,
                Amount = product.Amount
            };
        }
    }
}
