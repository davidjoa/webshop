//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Lesson6.Models;
//using Xunit;
//using Lesson6.Controllers;

//namespace WebShop.Controllers
//{
//    public class ProductsControllerTest
//    {

//        private static DbContextOptions<WebshopRepository> CreateNewContextOptions()
//        {
//            // Create a fresh service provider, and therefore a fresh 
//            // InMemory database instance.
//            var serviceProvider = new ServiceCollection()
//                .AddEntityFrameworkInMemoryDatabase()
//                .BuildServiceProvider();

//            // Create a new options instance telling the context to use an
//            // InMemory database and the new service provider.
//            var builder = new DbContextOptionsBuilder<WebshopRepository>();
//            builder.UseInMemoryDatabase()
//                   .UseInternalServiceProvider(serviceProvider);

//            return builder.Options;
//        }
//        [Fact]
//        public async Task IndexListAllProducts()
//        {
//            //Arrange
//            // All contexts that share the same service provider will share the same InMemory database
//            var options = CreateNewContextOptions();

//            // Insert seed data into the database using one instance of the context
//            using (var context = new WebshopRepository(options))
//            {
//                context.Products.Add(new Product { ProductName = "Test Product 1" });
//                context.Products.Add(new Product { ProductName = "Test Product 2" });
//                context.SaveChanges();
//            }

//            // Use a clean instance of the context to run the test
//            //using (var context = new WebshopRepository(options))
//            //{
//            //    var service = new ProductsController(context);

//            //    //Act
//            //    var result = await service.Index();

//            //    //Assert
//            //    var viewResult = Assert.IsType<ViewResult>(result);
//            //    var model = Assert.IsAssignableFrom<IEnumerable<Lesson6.Models.Product>>(
//            //        viewResult.ViewData.Model);
//            //    Assert.Equal(2, model.Count());
//            //    Assert.Equal("Test Product 1", model.ElementAt(0).ProductName);
//            //}
//        }

//        [Fact]
//        public async Task SearchIndexProducts()
//        {
//            //Arrange
//            var options = CreateNewContextOptions();

//            using (var context = new WebshopRepository(options))
//            {
//                context.Add(new Product {ProductName="banan",ProductId=1 });
//                context.Add(new Product { ProductName = "hallon", ProductId = 2 });
//                context.Add(new Product { ProductName = "äpple", ProductId = 3 });
//                context.Add(new Product { ProductName = "plommon", ProductId = 4 });
//                context.SaveChanges();

//            }

//            using (var context = new WebshopRepository(options))
//            {
//                var service = new ProductsController(context);

//                //Act
//                var result = await service.Index("hallon");

//                //Assert
//                var viewResult = Assert.IsType<ViewResult>(result);
//                var model = Assert.IsAssignableFrom<IEnumerable<Lesson6.Models.Product>>(viewResult.ViewData.Model);

//                Assert.Equal(1, model.Count());
//                Assert.Equal("banan",model.ElementAt(0).ProductName);

//            }
            
//        }

//       [Fact]
//       public async Task DetailsReturnNotFoundWhenIdIsNull()
//        {
//            //Arrange
//            // All contexts that share the same service provider will share the same InMemory database
//            var options = CreateNewContextOptions();

//            // Insert seed data into the database using one instance of the context
//            using (var context = new WebshopRepository(options))
//            {
//                context.Products.Add(new Product { ProductName = "Test Product 1",ProductId=1 });
//                context.Products.Add(new Product { ProductName = "Test Product 2", ProductId=2 });
//                context.SaveChanges();
//            }

//            // Use a clean instance of the context to run the test
//            using (var context = new WebshopRepository(options))
//            {
//                var service = new ProductsController(context);

//                //Act
//                var result = await service.Details(id: null);

//                //Assert

//                var notFoundResult = Assert.IsType<NotFoundResult>(result);
//              //  Assert.IsType<NotFoundResult>(notFoundResult);
               
//            }



//        }

//        [Fact]
//        public async Task CreateReturnViewDataWhenModelstateIsInvalid()
//        {
//            //Arrenge
//            var options = CreateNewContextOptions();

//            using (var context = new WebshopRepository(options))
//            {
//                context.Products.Add(new Product { ProductName = "Test Product 1" });
//                context.Products.Add(new Product { ProductName = "Test Product 2" });
//                context.SaveChanges();

//            }

//            using (var context = new WebshopRepository(options))
//            {
//                var service = new ProductsController(context);
//                service.ModelState.AddModelError("Error1", "Error2");


//                //Act
//                var product = new Product { ProductName = "testtest", ProductId = 1 };
//                var result = await service.Create(product);

//                //Assert

//                var viewResult = Assert.IsType<ViewResult>(result);
//                var model = Assert.IsAssignableFrom<Lesson6.Models.Product>(viewResult.ViewData.Model);


//            }
//        }

//        [Fact]
//        public async Task CreateReturnRedirectToActionIndex()
//        {

//            //Arrange
//            var option = CreateNewContextOptions();

//            //using (var context = new WebshopRepository(option))
//            //{

//            //    context.Products.Add(new Product { ProductName = "Test Product 1" });
//            //    context.Products.Add(new Product { ProductName = "Test Product 2" });
//            //    context.SaveChanges();

//            //}

//            using (var context = new WebshopRepository(option))
//            {
//                var service = new ProductsController(context);

//                //Act
//                var product = new Product { ProductName = "testtest", ProductId = 1 };
//                var result = await service.Create(product);



//                //Assert
//                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
             
//                Assert.Equal("Index", redirectToActionResult.ActionName);

               

//            }

//        }

//    }


//}

