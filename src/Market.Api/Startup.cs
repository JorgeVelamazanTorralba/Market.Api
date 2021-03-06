using Market.Api.Business.Contracts;
using Market.Api.Business.Implementations;
using Market.Api.Vendor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Market.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Market.Api", Version = "v1" });
            });

            services.AddTransient<IOrderVendor<VendorOneRequest, VendorOneResponse>, VendorOne>();
            services.AddTransient<IOrderVendor<VendorTwoRequest, VendorTwoResponse>, VendorTwo>();
            services.AddTransient<ICatalogVendor<VendorOneCatalog>, VendorOne>();
            services.AddTransient<ICatalogVendor<VendorTwoCatalog>, VendorTwo>();            
            services.AddSingleton<IOrderService, OrderService>();
            services.AddSingleton<ICatalogService, CatalogService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Market.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}