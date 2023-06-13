using System;
using System.Collections.Generic;

namespace StoreFront.DATA.EF.Models
{
    public partial class Builder
    {
        public Builder()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Builder1 { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
