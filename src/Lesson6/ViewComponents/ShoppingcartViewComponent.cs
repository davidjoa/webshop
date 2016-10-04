
using Lesson6.Models;
using Lesson6.ViewModels;
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
        
        private readonly WebshopRepository _context;

        public ShoppingcartViewComponent(WebshopRepository context)
        {
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var itemCount =  ShoppingCart.GetCart(_context, HttpContext);

            int count = itemCount.GetCount();
          
            return View("Default",count);
        }
      




    }
}
