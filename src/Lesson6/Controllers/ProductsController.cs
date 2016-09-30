using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lesson6.Models;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Lesson6.ViewModels;
using System.Net.Http;
using Lesson6.Services;

namespace Lesson6.Controllers
{
    public class ProductsController : Controller
    {
        private readonly WebshopRepository _context;
        private readonly IStringLocalizer<WebshopRepository> _localizer;


        private async Task <IEnumerable<ProductViewModel>> ProductQuery(int? id)
        {
            if (id!=null)
            {
                var product = from p in _context.Products
                               join pt in _context.ProductTranslations on
                                   new { p.ProductId, second = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName }
                                   equals new { pt.ProductId, second = pt.Language }

                               where p.ProductId == id
                               select new ProductViewModel
                               {
                                   ProductId = p.ProductId,
                                   ProductName = pt.ProductName,
                                   ProductDescription = pt.ProductDescription,
                                   ProductCategoryId = p.ProductCategoryId,
                                   ProductCategory = p.ProductCategory,
                                   Price = p.Price
                               };

              return await product.ToListAsync();

            }


            var products = from p in _context.Products
                           join pt in _context.ProductTranslations on
                               new { p.ProductId, second = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName }
                               equals new { pt.ProductId, second = pt.Language }
                            
                               
                           select new ProductViewModel
                           {
                               ProductId = p.ProductId,
                               ProductName = pt.ProductName,
                               ProductDescription = pt.ProductDescription,
                               ProductCategoryId = p.ProductCategoryId,
                               ProductCategory = p.ProductCategory,
                               Price = p.Price
                           }; 
                            
           
                               
            return await products.ToListAsync();
        }


        public ProductsController(WebshopRepository context, IStringLocalizer<WebshopRepository> localizer)
        {
            _context = context;
            _localizer = localizer;    
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        // GET: Products    

        public async Task<IActionResult> Index(string searchString)
        {
            
            var products = await ProductQuery(null);

            var Iso = new RegionInfo(CultureInfo.CurrentCulture.Name).ISOCurrencySymbol;

            //FixerCurrency.GetSEKToRate(Iso);

            


            


            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(x => x.ProductName.Contains(searchString));

            }

            return View(products);     
           
        }

      
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var product = await ProductQuery(id);

            return View(product.ElementAt(0));
   
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategories, "ProductCategoryId", "ProductCategoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel product)
        {
            Product p = new Product
            {
                Price=product.Price,
                ProductCategory=product.ProductCategory,
                ProductCategoryId=product.ProductCategoryId
               
            };

            _context.Add(p);
            _context.SaveChanges();

            ProductTranslation pt = new ProductTranslation
            {
               
                ProductDescription=product.ProductDescription,
                ProductName=product.ProductName,
                Language=CultureInfo.CurrentUICulture.TwoLetterISOLanguageName,
                ProductId=p.ProductId
               
            
            };

            _context.Add(pt);
            _context.SaveChanges();

            //if (ModelState.IsValid)
            //{                
            //    _context.Add(p);
            //    _context.Add(pt);
                
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction("Index");
            //}
            ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategories, "ProductCategoryId", "ProductCategoryName", product.ProductCategoryId);
            return RedirectToAction("Index");
            // return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await ProductQuery(id);


            ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategories, "ProductCategoryId", "ProductCategoryName");

            return View(product.ElementAt(0));
         
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductViewModel product)
        {

            if (id != product.ProductId)
            {
                return NotFound();
            }


            var p = await _context.Products.SingleOrDefaultAsync(m => m.ProductId == id);

            var pt =await _context.ProductTranslations.SingleOrDefaultAsync(m => m.ProductId == id&&m.Language==CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);

            p.ProductId = product.ProductId;
            p.ProductCategoryId = product.ProductCategoryId;
            p.ProductCategory = product.ProductCategory;
            p.Price = product.Price;
            pt.ProductDescription = product.ProductDescription;
            pt.ProductName = product.ProductName;
            

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(p);
                    _context.Update(pt);


                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategories, "ProductCategoryId", "ProductCategoryName", product.ProductCategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
                       
            var product = await ProductQuery(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product.ElementAt(0));
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product =  _context.Products.SingleOrDefault(m => m.ProductId == id);
            var TranslationProduct = _context.ProductTranslations.SingleOrDefault(m => m.ProductId == id && m.Language == CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);

            _context.Products.Remove(product);
            _context.ProductTranslations.Remove(TranslationProduct);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
