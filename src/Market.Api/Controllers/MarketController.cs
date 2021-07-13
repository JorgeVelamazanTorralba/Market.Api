using Market.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Linq;
using Market.Api.Data;
using Market.Api.Business.Contracts;
using Market.Api.Vendor;

namespace Market.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketController : ControllerBase
    {
        private MarketContext _marketContext;
        private readonly IOrderService _orderService;
        private readonly ICatalogService _catalogService;

        public MarketController(MarketContext marketContext,
            IOrderService orderService,
            ICatalogService catalogService)            
        {
            _marketContext = marketContext;
            _orderService = orderService;
            _catalogService = catalogService;
        }

        [HttpGet]
        [Route("products")]
        public async Task<IActionResult> GetProduct()
        {
            var result = await _marketContext.Catalogs.ToListAsync();
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        [Route("sync-catalog")]
        public async Task<IActionResult> SyncCatalog()
        {
            var vendorTwoCatalog = _catalogService.GetCatalog<VendorTwoCatalog>().ConvertAll(item => (VendorTwoCatalog)item); 
            var vendorOneCatalog = _catalogService.GetCatalog<VendorOneCatalog>().ConvertAll(item => (VendorOneCatalog)item);

            var products = await _marketContext.Catalogs.ToListAsync();
            #region [Update current products]
            foreach (var product in products)
            {
                var verdonOneProduct = vendorOneCatalog.FirstOrDefault(w => w.Name.ToLower() == product.Name.ToLower());
                var verdonTwoProduct = vendorTwoCatalog.FirstOrDefault(w => w.Name.ToLower() == product.Name.ToLower());
                if (verdonOneProduct is not null && verdonTwoProduct is not null)
                {
                    if (verdonOneProduct.Amount < verdonTwoProduct.Amount)
                    {
                        var vendor = _marketContext.Vendors.Find(1);
                        product.Update<VendorOneCatalog>(verdonOneProduct, vendor);
                        await _marketContext.SaveChangesAsync();
                    }
                    else
                    {
                        var vendor = _marketContext.Vendors.Find(2);
                        product.Update<VendorTwoCatalog>(verdonTwoProduct, vendor);
                        await _marketContext.SaveChangesAsync();
                    }
                    vendorOneCatalog.Remove(verdonOneProduct);
                    vendorTwoCatalog.Remove(verdonTwoProduct);
                }
                else if (verdonOneProduct is not null)
                {
                    var vendor = _marketContext.Vendors.Find(1);
                    product.Update<VendorOneCatalog>(verdonOneProduct, vendor);
                    await _marketContext.SaveChangesAsync();
                    vendorOneCatalog.Remove(verdonOneProduct);
                }
                else if (verdonTwoProduct is not null)
                {
                    var vendor = _marketContext.Vendors.Find(2);
                    product.Update<VendorTwoCatalog>(verdonTwoProduct, vendor);
                    await _marketContext.SaveChangesAsync();
                    vendorTwoCatalog.Remove(verdonTwoProduct);
                }
            }
            #endregion

            #region [New products]
            var sameProducts = vendorOneCatalog.Where(w => vendorTwoCatalog.Select(s => s.Name.ToUpper()).Contains(w.Name.ToUpper())).ToList();
            if (sameProducts is not null)
            {
                foreach (var sameProduct in sameProducts)
                {
                    var verdonOneProduct = vendorOneCatalog.FirstOrDefault(w => w.Name.ToLower() == sameProduct.Name.ToLower());
                    var verdonTwoProduct = vendorTwoCatalog.FirstOrDefault(w => w.Name.ToLower() == sameProduct.Name.ToLower());
                    if (verdonOneProduct.Amount < verdonTwoProduct.Amount)
                    {
                        var vendor = _marketContext.Vendors.Find(1);
                        _marketContext.Catalogs.Add(verdonOneProduct.CreateCatalog(vendor));
                        await _marketContext.SaveChangesAsync();
                    }
                    else
                    {
                        var vendor = _marketContext.Vendors.Find(2);
                        _marketContext.Catalogs.Add(verdonTwoProduct.CreateCatalog(vendor));
                        await _marketContext.SaveChangesAsync();
                    }
                    vendorOneCatalog.Remove(verdonOneProduct);
                    vendorTwoCatalog.Remove(verdonTwoProduct);
                }
                if (vendorOneCatalog.Any())
                {
                    var vendor = _marketContext.Vendors.Find(1);
                    _marketContext.Catalogs.AddRange(
                        vendorOneCatalog.Select(s => s.CreateCatalog(vendor)));
                    await _marketContext.SaveChangesAsync();
                }
                if (vendorTwoCatalog.Any())
                {
                    var vendor = _marketContext.Vendors.Find(2);
                    _marketContext.Catalogs.AddRange(
                        vendorTwoCatalog.Select(s => s.CreateCatalog(vendor)));
                    await _marketContext.SaveChangesAsync();
                }
            }
            #endregion
            return Ok();
        }        

        [HttpPost]
        [Route("create-order")]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest createOrderViewModel)
        {
            CreateOrderResponse response;
            
            if (!createOrderViewModel.Id.HasValue) 
            {
                throw new Exception("Id is null");
            }
            var product = await _marketContext.Catalogs.FirstOrDefaultAsync(w => w.Id == createOrderViewModel.Id);
            if (product is null) 
            {
                return NotFound();
            }

            Order order = _orderService.CreateVendorResponseByCatalog(product, createOrderViewModel.Id.Value);
            _marketContext.Orders.Add(order);
            await _marketContext.SaveChangesAsync();
            response = new CreateOrderResponse { Id = order.Id, Amount = order.Amount };

            return Ok(response);
        }
    }
}