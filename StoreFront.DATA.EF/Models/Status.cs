using System;
using System.Collections.Generic;

namespace StoreFront.DATA.EF.Models
{
    public partial class Status
    {
        public Status()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string? Status1 { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
