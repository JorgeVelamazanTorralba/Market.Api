using System.Collections.Generic;

namespace Market.Data.Models
{
    public class Vendor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Catalog> Catalogs { get; set; }
    }
}