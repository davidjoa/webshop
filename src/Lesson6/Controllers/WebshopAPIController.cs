using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Lesson6.Models;
using Microsoft.EntityFrameworkCore;
using Lesson6.ViewModels;
using System.Globalization;

namespace Lesson6.Controllers
{
    [Produces("application/json")]
    [Route("api/WebshopAPI")]
    public class WebshopAPIController : Controller
    {
        private readonly WebshopRepository _context;

        public WebshopAPIController(WebshopRepository context)
        {
            _context = context;
        }

       
        [HttpGet]
        public IEnumerable<Product> GetAll()
        {

            return _context.Products.Include(m => m.Translations).Include(p=>p.ProductCategory);

        }

        [HttpGet("{id}",Name ="Getproduct")]
        public IActionResult GetByID(int id)
        {
            var product = _context.Products.Include(x=>x.Translations).SingleOrDefault(x => x.ProductId == id);
            if (product==null)
            {
                return NotFound();
            }
            return new ObjectResult(product);     
                     
        }

        [HttpPost]
        public IActionResult Create([FromBody]ProductViewModel product)
        {

            Product p = new Product
            {
                Price = product.Price,
                ProductCategory = product.ProductCategory,
                ProductCategoryId = product.ProductCategoryId,
                PictureURL=product.PictureURL
            };

            _context.Add(p);
            _context.SaveChanges();

            ProductTranslation pt = new ProductTranslation
            {

                ProductDescription = product.ProductDescription,
                ProductName = product.ProductName,
                Language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName,
                ProductId = p.ProductId


            };

            _context.Add(pt);
            _context.SaveChanges();

            return CreatedAtRoute("Getproduct", new { id = p.ProductId },p);

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]ProductViewModel product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }


            var p =  _context.Products.SingleOrDefault(m => m.ProductId == id);

            var pt =  _context.ProductTranslations.SingleOrDefault(m => m.ProductId == id && m.Language == CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);

            p.ProductId = product.ProductId;
            p.ProductCategoryId = product.ProductCategoryId;
            p.ProductCategory = product.ProductCategory;
            p.Price = product.Price;
            pt.ProductDescription = product.ProductDescription;
            pt.ProductName = product.ProductName;

             _context.Update(p);
             _context.Update(pt);
             _context.SaveChanges();
          

            return new NoContentResult();

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            var product = _context.Products.SingleOrDefault(m => m.ProductId == id);
            var TranslationProduct = _context.ProductTranslations.SingleOrDefault(m => m.ProductId == id && m.Language == CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);

            _context.Products.Remove(product);
            _context.ProductTranslations.Remove(TranslationProduct);

             _context.SaveChangesAsync();

            return new NoContentResult();


        }

    }
}