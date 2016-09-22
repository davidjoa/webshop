using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lesson6.Models;
using Lesson6.ViewModels;

namespace Lesson6.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly WebshopRepository _context;

        public ShoppingCartController(WebshopRepository context)
        {
            _context = context;

        }
        
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
    }
}