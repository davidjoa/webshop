using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Globalization;

namespace Lesson6.Services
{
    public class FixerCurrency
    {
        private static HttpClient Client = new HttpClient();


        public decimal GetSEKToGBPRate()
        {
            var result = Client.GetAsync("http://api.fixer.io/latest?base=SEK;symbols=GBP").Result;
            var jsonString = result.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<Rate>(jsonString.Result);


            return obj.rates.GBP;
        }

        public decimal GetSEKToRate(string ISOCurrencySymbol)
        {
            if (ISOCurrencySymbol=="SEK")
            {
                return 1.0m;
            }

            else {
            var result = Client.GetAsync("http://api.fixer.io/latest?base=SEK;symbols=" + ISOCurrencySymbol).Result;
            var jsonString = result.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<Rate>(jsonString.Result);


                return obj.rates.GBP;

            }
        }

        public string GetIsoSymbol()
        {
            var iso = new RegionInfo(CultureInfo.CurrentCulture.Name).ISOCurrencySymbol;

            return iso;

        }
        
    }

   
    public class Rates
    {
               
        public decimal GBP { get; set; }
    
    }

    public class Rate
    {
        public string @base { get; set; }
        public string date { get; set; }
        public Rates rates { get; set; }
    }
}
