using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Lesson6.Models
{
    public partial class ShoppingCart
    {
        private WebshopRepository _context;

        public ShoppingCart(WebshopRepository context)
        {

            _context = context;
        }


        string shoppingCartId { get; set; }
       

        public static ShoppingCart GetCart(WebshopRepository repo,HttpContext context)
        {                       
            var cart = new ShoppingCart(repo);          

            cart.shoppingCartId = cart.GetCartId(context);

            return cart;
        }

        public string GetCartId(HttpContext context)
        {
            string cartid;
             
            if (context.Request.Cookies["cartId"] == null)
            {
                cartid = Guid.NewGuid().ToString();
                context.Response.Cookies.Append(

                    "cartId",
                    cartid,
                    new Microsoft.AspNetCore.Http.CookieOptions()
                    {
                        Path = "/",
                        HttpOnly = false,
                        Secure = false,
                        Expires = DateTime.Now.AddMonths(6)

                    });                
            }
            else {

                cartid = context.Request.Cookies["cartId"];
                cartid = HtmlEncoder.Default.Encode(cartid);
          
            }
            return cartid;
        }

        public void AddToCart(Product product)
        {

            var cartItem = _context.Carts.SingleOrDefault(
                m => m.CartId == shoppingCartId &&
                m.ProductId == product.ProductId
                
                );

            if (cartItem == null)
            {

                cartItem = new Cart
                {
                    ProductId = product.ProductId,
                    CartId = shoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                    
                };
                _context.Add(cartItem);
            }

            else
            {
                cartItem.Count++;
                
            }
            _context.SaveChanges();

        }

        public int RemoveFromCart (int id)
        {
            var cartItem = _context.Carts.SingleOrDefault(

                m => m.CartId == shoppingCartId &&
                m.ProductId == id

                );
            int itemcount = 0;

            if (cartItem!=null)
            {
                if (cartItem.Count>1)
                {
                    cartItem.Count--;
                    itemcount = cartItem.Count;
                }

                else
                {
                    _context.Carts.Remove(cartItem);
                    
                }

                _context.SaveChanges();

            }

            return itemcount;
        }

        public List<Cart> GetCartItems()
        {
           

            return _context.Carts.Where(cart => cart.CartId == shoppingCartId).Include(x=>x.Product).ToList();
            
        }

        public decimal GetTotal()
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            decimal? total = (from cartItems in _context.Carts
                              where cartItems.CartId == shoppingCartId
                              select (int?)cartItems.Count *
                              cartItems.Product.Price).Sum();
                              

            return total ?? decimal.Zero;
        }

        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in _context.Carts
                          where cartItems.CartId == shoppingCartId
                          select (int?)cartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }
    }
}
