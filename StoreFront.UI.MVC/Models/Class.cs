using StoreFront.DATA.EF.Models;

namespace StoreFront.UI.MVC.Models
{
    public class CartItemViewModel
    {
        public int Qty { get; set; }

        public Product Product { get; set; }

        //The above is an example of a concept called "Containment."
        //This is a use of a complex datatype as a field/property in another class.

        //Complex datatype: Any class with multiple properties (values) -- Product, ContactViewModel, DateTime
        //Primitive datatype: A class that stores a single value -- (int, bool, char, decimal, etc..)

        //Constructor (Ctor)
        public CartItemViewModel(int qty, Product product)
        {
            //Assignment
            Qty = qty;
            Product = product;

        }

    }
}
