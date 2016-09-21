using Lesson6.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lesson6.ViewComponents
{
    [ViewComponent(Name = "Shoppingcart")]
    public class ShoppingcartViewComponent : ViewComponent
    {
        
        private readonly ShoppingCart _cart;

        public ShoppingcartViewComponent(ShoppingCart cart)
        {
           _cart = cart;
        }


        //public async Task<IViewComponentResult> InvokeAsync(ShoppingCart _cart)
        //{
        //    //var items = await ShoppingCart.GetCart();

        //    return View(items);
        //}
     


    }
}
