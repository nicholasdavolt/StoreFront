using Microsoft.AspNetCore.Mvc;
using StoreFront.DATA.EF.Models;
using Microsoft.AspNetCore.Identity;
using StoreFront.UI.MVC.Models;
using Newtonsoft.Json;

namespace StoreFront.UI.MVC.Controllers
{
    public class ShoppingCartController : Controller
    {

        private readonly GuitarShopContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ShoppingCartController(GuitarShopContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            //Retrives existing cart from the Session

            var sessionCart = HttpContext.Session.GetString("cart");

            //Create local cart shell

            Dictionary<int, CartItemViewModel> shoppingCart = null;

            //Check to see if the cart is null or empty

            if (sessionCart == null || sessionCart.Count() == 0)
            {
                //Sets cart to an empty Dictionary
                shoppingCart = new Dictionary<int, CartItemViewModel>();
                ViewBag.Message = "There are no items in your cart.";
            }
            else
            {
                ViewBag.Message = null;

                //Deserialize the session cart and add to the shoppingCart dictionary

                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int,CartItemViewModel>>(sessionCart);

            }
            return View(shoppingCart);
        }


        public IActionResult AddToCart (int id)
        {
            //Creates shell dictionary for the local cart

            Dictionary<int, CartItemViewModel> shoppingCart = null;

            //pull any existing cart info from the Session

            var sessionCart = HttpContext.Session.GetString("cart");

            //Confirm if the Session cart contains anything. If it doesn't instantiate a new dictionary
            if (string.IsNullOrEmpty(sessionCart))
            {
                shoppingCart = new Dictionary<int, CartItemViewModel>();
            }
            else
            {
                //If the Session cart did contain cart info, deserialize and add to the shoppingCart dictionary

                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int,CartItemViewModel>>(sessionCart);
            }


            //Pull selected product from the database

            Product product = _context.Products.Find(id);

            //Create and Instantiate a CartItemViewModel object to be added to the cart
            CartItemViewModel civm = new CartItemViewModel(1, product);

            //Check to see if the product was already in the cart, if true iterate the qty by 1

            if(shoppingCart.ContainsKey(product.Id))
            {
                shoppingCart[product.Id].Qty++;
            }
            else
            {
                shoppingCart.Add(product.Id, civm);
            }

            
            //Updates the Session with the updated cart info

            string jsonCart = JsonConvert.SerializeObject(shoppingCart);

            HttpContext.Session.SetString("cart", jsonCart);

            return RedirectToAction("TiledProducts" , "Products");
        }

        public IActionResult RemoveFromCart (int id)
        {
            //Pulls cart info from the Session

            var sessionCart = HttpContext.Session.GetString("cart");

            //Since we know cart info exists, deserialize and add to the Dictionary shoppingCart

            Dictionary<int, CartItemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            //Remove the item

            shoppingCart.Remove(id);

            //Check for remaining items in the cart, if there are none then remove the cart from the session

            if (shoppingCart.Count == 0)
            {
                HttpContext.Session.Remove("cart");
            }
            else
            {
                //Since items remain, serialize the dictionary to JSON and update the session

                string jsonCart = JsonConvert.SerializeObject(shoppingCart);

                HttpContext.Session.SetString("cart", jsonCart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult UpdateCart (int productId, int qty)
        {
            //Pull the cart from the session
            var sessionCart = HttpContext.Session.GetString("cart");

            //Since we know cart info exists, deserialize and add to the Dictionary shoppingCart
            Dictionary<int, CartItemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            //Update the qty for the specified item in the local cart

            shoppingCart[productId].Qty = qty;

            //Update the Session cart
            string jsonCart = JsonConvert.SerializeObject (shoppingCart);
            HttpContext.Session.SetString("cart", jsonCart);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SubmitOrder()
        {
            //Retrieve the User's ID

            string? userId = (await _userManager.GetUserAsync(HttpContext.User))?.Id;

            //Using the above Id, collect the user's details
            UserDetail ud = _context.UserDetails.Find(userId);

            //Creates the order object

            Order o = new Order()
            {
                OrderDate = DateTime.Now,
                UserId = userId,
                ShipToName = ud.FullName,
                ShipCity = ud.City,
                ShipState = ud.State,
                ShipZip = ud.Zip
            };

            //Add the new order item to the context
            _context.Orders.Add(o);

            //Retrieve the cart from the session
            var sessionCart = HttpContext.Session.GetString("cart");

            //Convert from JSON to Dictionary
            Dictionary<int, CartItemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            //Create the OrderProduct object for each item in the cart and add to the existing context
            foreach (var item in shoppingCart)
            {
                OrderProduct op = new OrderProduct()
                {
                    OrderId = o.Id,
                    ProductId = item.Key,
                    ProductPrice = (decimal)item.Value.Product.SellPrice,
                    Quantity = (short)item.Value.Qty


                };
                
                o.OrderProducts.Add(op);
            }
            //Commits changes to the database
            _context.SaveChanges();
            //Clear the Cart
            HttpContext.Session.Remove("cart");
            //Redirect the user to the Orders Index
            return RedirectToAction("OrderByUser", "Orders",new {id = userId});


        }


    }
}
