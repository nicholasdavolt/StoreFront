using System;
using System.Collections.Generic;

namespace StoreFront.DATA.EF.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

        public int Id { get; set; }
        public string ProductName { get; set; } = null!;
        public string? ProductDesc { get; set; }
        public short? UnitsInStock { get; set; }
        public decimal? SellPrice { get; set; }
        public decimal? PurchasePrice { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public int? BuilderId { get; set; }
        public string? ProductImage { get; set; }

        public virtual Builder? Builder { get; set; }
        public virtual Status Status { get; set; } = null!;
        public virtual Type Type { get; set; } = null!;
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
