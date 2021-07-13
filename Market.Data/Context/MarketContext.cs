using Market.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Market.Data.Context
{
    public class MarketContext : DbContext
    {
        public MarketContext(DbContextOptions<MarketContext> options) : base(options)
        {
        }

        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}