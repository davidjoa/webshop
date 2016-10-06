using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lesson6.Models;
using Xunit;
using Lesson6.Controllers;
using Lesson6.ViewModels;

namespace WebShop.Controllers
{
    public class ProductsControllerTest
    {

        private static DbContextOptions<WebshopRepository> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<WebshopRepository>();
            builder.UseInMemoryDatabase()
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
        [Fact]
        public async Task IndexListAllProducts()
        {
            //Arrange
            // All contexts that share the same service provider will share the same InMemory database
            var options = CreateNewContextOptions();

            // Insert seed data into the database using one instance of the context
            using (var context = new WebshopRepository(options))
            {
                context.ProductCategories.Add(new ProductCategory {ProductCategoryId=3,ProductCategoryName="frukt" });

                context.Products.Add(new Product { ProductId = 1,Price=55,PictureURL="gwgwgw",ProductCategoryId=3 });
                context.Products.Add(new Product { ProductId = 2, Price=69,ProductCategoryId=3 });
                context.Products.Add(new Product { ProductId = 3, Price = 109,ProductCategoryId=3 });
                context.SaveChanges();

                context.ProductTranslations.Add(new ProductTranslation { ProductName = "hallon", ProductId=1,Language="sv",ProductDescription="hejhå" });
                context.ProductTranslations.Add(new ProductTranslation { ProductName = "banan", ProductId =2, Language = "sv", ProductDescription = "hejhå" });
                context.ProductTranslations.Add(new ProductTranslation { ProductName = "päron", ProductId=3, Language = "sv", ProductDescription = "hejhå" });

                context.SaveChanges();


            }

            // Use a clean instance of the context to run the test

            using (var context = new WebshopRepository(options))
            {
                var service = new ProductsController(context);

                //Act
                var result = await service.Index(null);

                //Assert

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Lesson6.ViewModels.ProductViewModel>>(viewResult.ViewData.Model);
                var transModel = Assert.IsAssignableFrom<IEnumerable<Lesson6.ViewModels.ProductViewModel>>(viewResult.ViewData.Model);
               
               
                Assert.Equal(3, model.Count());
                Assert.Equal("hallon", transModel.ElementAt(0).ProductName);
            }
        }

        [Fact]
        public async Task SearchIndexProducts()
        {
            //Arrange
            var options = CreateNewContextOptions();

            using (var context = new WebshopRepository(options))
            {
                context.ProductCategories.Add(new ProductCategory { ProductCategoryId = 3, ProductCategoryName = "frukt" });

                context.Products.Add(new Product { ProductId = 1, Price = 55, PictureURL = "gwgwgw", ProductCategoryId = 3 });
                context.Products.Add(new Product { ProductId = 2, Price = 69, ProductCategoryId = 3 });
                context.Products.Add(new Product { ProductId = 3, Price = 109, ProductCategoryId = 3 });
                context.SaveChanges();



                context.ProductTranslations.Add(new ProductTranslation { ProductName = "hallon", ProductId = 1, Language = "sv", ProductDescription = "hejhå" });
                context.ProductTranslations.Add(new ProductTranslation { ProductName = "banan", ProductId = 2, Language = "sv", ProductDescription = "hejhå" });
                context.ProductTranslations.Add(new ProductTranslation { ProductName = "päron", ProductId = 3, Language = "sv", ProductDescription = "hejhå" });

                context.SaveChanges();

            }

            using (var context = new WebshopRepository(options))
            {
                var service = new ProductsController(context);

                //Act
                var result = await service.Index("hallon");

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Lesson6.ViewModels.ProductViewModel>>(viewResult.ViewData.Model);

                Assert.Equal(1, model.Count());
                Assert.Equal("hallon", model.ElementAt(0).ProductName);

            }

        }

        [Fact]
        public async Task DetailsReturnNotFoundWhenIdIsNull()
        {
            //Arrange
            // All contexts that share the same service provider will share the same InMemory database
            var options = CreateNewContextOptions();

            // Insert seed data into the database using one instance of the context
            using (var context = new WebshopRepository(options))
            {
                context.ProductCategories.Add(new ProductCategory { ProductCategoryId = 3, ProductCategoryName = "frukt" });

                context.Products.Add(new Product { ProductId = 1, Price = 55, PictureURL = "gwgwgw", ProductCategoryId = 3 });
                context.Products.Add(new Product { ProductId = 2, Price = 69, ProductCategoryId = 3 });
                context.Products.Add(new Product { ProductId = 3, Price = 109, ProductCategoryId = 3 });
                context.SaveChanges();



                context.ProductTranslations.Add(new ProductTranslation { ProductName = "hallon", ProductId = 1, Language = "sv", ProductDescription = "hejhå" });
                context.ProductTranslations.Add(new ProductTranslation { ProductName = "banan", ProductId = 2, Language = "sv", ProductDescription = "hejhå" });
                context.ProductTranslations.Add(new ProductTranslation { ProductName = "päron", ProductId = 3, Language = "sv", ProductDescription = "hejhå" });

                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = new WebshopRepository(options))
            {
                var service = new ProductsController(context);

                //Act
                var result = await service.Details(id: null);

                //Assert

                var notFoundResult = Assert.IsType<NotFoundResult>(result);
                Assert.IsType<NotFoundResult>(notFoundResult);

            }



        }

        [Fact]
        public async Task CreateReturnViewDataWhenModelstateIsInvalid()
        {
            //Arrenge
            var options = CreateNewContextOptions();

            using (var context = new WebshopRepository(options))
            {
                context.ProductCategories.Add(new ProductCategory { ProductCategoryId = 3, ProductCategoryName = "frukt" });

                context.Products.Add(new Product { ProductId = 1, Price = 55, PictureURL = "gwgwgw", ProductCategoryId = 3 });
                context.Products.Add(new Product { ProductId = 2, Price = 69, ProductCategoryId = 3 });
                context.Products.Add(new Product { ProductId = 3, Price = 109, ProductCategoryId = 3 });
                context.SaveChanges();



                context.ProductTranslations.Add(new ProductTranslation { ProductName = "hallon", ProductId = 1, Language = "sv", ProductDescription = "hejhå" });
                context.ProductTranslations.Add(new ProductTranslation { ProductName = "banan", ProductId = 2, Language = "sv", ProductDescription = "hejhå" });
                context.ProductTranslations.Add(new ProductTranslation { ProductName = "päron", ProductId = 3, Language = "sv", ProductDescription = "hejhå" });

                context.SaveChanges();

            }

            using (var context = new WebshopRepository(options))
            {
                var service = new ProductsController(context);
                service.ModelState.AddModelError("Error1", "Error2");


                //Act
                var product = new ProductViewModel { ProductName = "testtest", ProductId = 3,PictureURL="wgweew",Price=49,ProductCategoryId=3,ProductDescription="testkategori" };
                var result = await service.Create(product);

                //Assert

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<Lesson6.ViewModels.ProductViewModel>(viewResult.ViewData.Model);
                               
            }
        }

        [Fact]
        public async Task CreateReturnRedirectToActionIndex()
        {

            //Arrange
            var option = CreateNewContextOptions();

            using (var context = new WebshopRepository(option))
            {
                //context.ProductCategories.Add(new ProductCategory { ProductCategoryId = 3, ProductCategoryName = "frukt" });

                //context.Products.Add(new Product { ProductId = 1, Price = 55, PictureURL = "gwgwgw", ProductCategoryId = 3 });
                //context.Products.Add(new Product { ProductId = 2, Price = 69, ProductCategoryId = 3 });
                //context.Products.Add(new Product { ProductId = 3, Price = 109, ProductCategoryId = 3 });
                //context.SaveChanges();



                //context.ProductTranslations.Add(new ProductTranslation { ProductName = "hallon", ProductId = 1, Language = "sv", ProductDescription = "hejhå" });
                //context.ProductTranslations.Add(new ProductTranslation { ProductName = "banan", ProductId = 2, Language = "sv", ProductDescription = "hejhå" });
                //context.ProductTranslations.Add(new ProductTranslation { ProductName = "päron", ProductId = 3, Language = "sv", ProductDescription = "hejhå" });

                //context.SaveChanges();
                
            }

            using (var context = new WebshopRepository(option))
            {
                var service = new ProductsController(context);

                //Act
                var product = new ProductViewModel { ProductName = "testtest", ProductId = 1, PictureURL = "wgweew", Price = 49, ProductCategoryId = 3, ProductDescription = "testkategori" };
                var result = await service.Create(product);



                //Assert
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

                Assert.Equal("Index", redirectToActionResult.ActionName);



            }

        }

    }


}

