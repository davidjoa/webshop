using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lesson6.Models;
using Lesson6.ViewModels;
using System.Collections;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Lesson6.Services;

namespace Lesson6.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly WebshopRepository _context;

        public ShoppingCartController(WebshopRepository context)
        {
            _context = context;

        }

        private string getCulture(Product product)
        {
            var culture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            return culture;
        }

        private string CreateAuthorization(string data)

        {
           
            using (var algorithm = SHA256.Create())  {

                var hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(data));
                var base64 = System.Convert.ToBase64String(hash);

                return base64;
            }
        }

        private string SharedSecret = "tE94QeKzSdUVJGe";

        private static HttpClient Client = new HttpClient();
        public IActionResult Index()
        {
            var cart = ShoppingCart.GetCart(_context, HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            // Return the view
            return View(viewModel);
        }

        
        public IActionResult AddToCart(int id, string returnUrl)
        {
            var addedProduct = _context.Products.Single(

                m => m.ProductId == id

                );

            var cart = ShoppingCart.GetCart(_context,HttpContext);

            cart.AddToCart(addedProduct);

            return Redirect(returnUrl);
        }

        
        public IActionResult RemoveFromCart (int id)
        {

            var cart = ShoppingCart.GetCart(_context, HttpContext);
            cart.RemoveFromCart(id);

            return RedirectToAction("Index");

        }

        public IActionResult CheckOut()
        {
            var cart = ShoppingCart.GetCart(_context, HttpContext);
            Klarna k = new Klarna();
            
            var response = k.PostRequest(k.createJsonData(cart));
            
            var snippet = "";

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {

                var location = response.Headers.Location.AbsoluteUri;
                
                //hämta ordern

                snippet = k.GetRequest(location);
            
            }
            
            ViewBag.snippet = snippet;
              
            return View("CheckOut",snippet);
          
        }

        public IActionResult OrderConfirmed(string klarna_order_id)
        {
            string stdUri = "https://checkout.testdrive.klarna.com/checkout/orders";

            Klarna k = new Klarna();
            var snippet = k.GetRequest(stdUri +"/"+ klarna_order_id);

            ShoppingCart.EmptyCartCookie(HttpContext);

            ViewBag.snippet = snippet;
              
            return View("OrderConfirmed", snippet);

    }
    }
}