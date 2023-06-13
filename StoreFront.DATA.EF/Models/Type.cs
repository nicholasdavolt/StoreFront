using System;
using System.Collections.Generic;

namespace StoreFront.DATA.EF.Models
{
    public partial class Type
    {
        public Type()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Type1 { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
