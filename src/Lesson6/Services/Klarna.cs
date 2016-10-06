using Lesson6.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lesson6.Services
{
    public class Klarna
    {
       
        private static HttpClient Client = new HttpClient();
        private string CreateAuthorization(string data)

        {

            using (var algorithm = SHA256.Create())
            {

                var hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(data));
                var base64 = System.Convert.ToBase64String(hash);

                return base64;
            }
        }
        private string getCulture(Product product)
        {
            var culture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            return culture;
        }

        private string SharedSecret = "tE94QeKzSdUVJGe";

        public string createJsonData(ShoppingCart ItemsInCart)
        {
            var cartItems = new List<Dictionary<string, object>>();

            // plocka ut i controllern, skicka in i metoden i denna klass.
            foreach (var item in ItemsInCart.GetCartItems())
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
            "http://localhost:5000/ShoppingCart/OrderConfirmed" +
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
            return jsondata;
        }
        public HttpResponseMessage PostRequest(string jsondata)
        {
            HttpRequestMessage message = new HttpRequestMessage();

            message.RequestUri = new Uri("https://checkout.testdrive.klarna.com/checkout/orders");
            message.Method = HttpMethod.Post;
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.klarna.checkout.aggregated-order-v2+json"));
            message.Headers.Authorization = new AuthenticationHeaderValue("Klarna", CreateAuthorization(jsondata + SharedSecret));
            message.Content = new StringContent(jsondata, Encoding.UTF8, "application/vnd.klarna.checkout.aggregated-order-v2+json");

            var response = Client.SendAsync(message).Result;

            return response;
        }

        public string GetRequest(string location)
        {
            HttpRequestMessage getmessage = new HttpRequestMessage();
            getmessage.RequestUri = new Uri(location);
            getmessage.Method = HttpMethod.Get;

            getmessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.klarna.checkout.aggregated-order-v2+json"));
            getmessage.Headers.Authorization = new AuthenticationHeaderValue("Klarna", CreateAuthorization(SharedSecret));

            var getresponse = Client.SendAsync(getmessage).Result;
            var getresponsebody = getresponse.Content.ReadAsStringAsync().Result;

            JObject gui = JObject.Parse(getresponsebody);

           var snippet = gui["gui"]["snippet"].ToString();

            return snippet;

        }

    }
}
