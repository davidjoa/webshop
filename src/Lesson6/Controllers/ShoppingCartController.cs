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
            
            var cartItems = new List<Dictionary<string, object>>();


            foreach (var item in ShoppingCart.GetCart(_context, HttpContext).GetCartItems())
            {


                cartItems.Add(
                new Dictionary<string, object>
                    {
                         { "reference", item.ProductId.ToString()},
                        { "name",item.Product.Translations.Where(z=>z.Language==getCulture(item.Product)) },
                        { "quantity", item.Count },
                        { "unit_price",(Convert.ToInt32(item.Product.Price))*100},
                        { "discount_rate", 0 },
                        { "tax_rate", 2500 }

                });
                
            }


            cartItems.Add(
            new Dictionary<string, object>
              {
                        { "type", "shipping_fee" },
                        { "reference", "SHIPPING" },
                        { "name", "Shipping Fee" },
                        { "quantity", 1 },
                        { "unit_price", 4900 },
                        { "tax_rate", 2500 }

          });

            var cart = new Dictionary<string, object> { { "items", cartItems } };

                var data = new Dictionary<string, object>
        {
            { "cart", cart }
        };

            

            var merchant = new Dictionary<string, object>
    {
        { "id", "5160" },
        { "back_to_store_uri", "http://localhost:5000/" },
        { "terms_uri", "http://localhost:5000/terms" },
        {
            "checkout_uri",
            "http://localhost:5000/checkout"
        },
        {
            "confirmation_uri",
            "http://localhost:5000/OrderConfirmed" +
            "?klarna_order_id={checkout.order.id}"
        },
        {
            "push_uri",
            "http://localhost:5000/push" +
            "?klarna_order_id={checkout.order.id}"
        }
    };

            data.Add("purchase_country", "SE");
            data.Add("purchase_currency", "SEK");
            data.Add("locale", "sv-se");
            data.Add("merchant", merchant);

            var jsondata = JsonConvert.SerializeObject(data);        

            HttpClient client = new HttpClient();

            HttpRequestMessage message = new HttpRequestMessage();

            message.RequestUri = new Uri("https://checkout.testdrive.klarna.com/checkout/orders");
            message.Method = HttpMethod.Post;
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.klarna.checkout.aggregated-order-v2+json"));
            message.Headers.Authorization = new AuthenticationHeaderValue("Klarna", CreateAuthorization(jsondata + SharedSecret));
            message.Content = new StringContent(jsondata, Encoding.UTF8, "application/vnd.klarna.checkout.aggregated-order-v2+json");

            var response = client.SendAsync(message).Result;
            var snippet = "";

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {

                var location = response.Headers.Location.AbsoluteUri;


                //hämta ordern

                HttpRequestMessage getmessage = new HttpRequestMessage();
                getmessage.RequestUri = new Uri(location);
                getmessage.Method = HttpMethod.Get;

                getmessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.klarna.checkout.aggregated-order-v2+json"));
                getmessage.Headers.Authorization = new AuthenticationHeaderValue("Klarna", CreateAuthorization(SharedSecret));

                var getresponse = client.SendAsync(getmessage).Result;
                var getresponsebody = getresponse.Content.ReadAsStringAsync().Result;

                JObject gui = JObject.Parse(getresponsebody);

                snippet = gui["gui"]["snippet"].ToString();
            }

            ViewBag.snippet = snippet;
              
            return View("CheckOut",snippet);
          

        }

        public IActionResult OrderConfirmed(string klarna_order_id)
        {

            return View("");

        }
    }
}