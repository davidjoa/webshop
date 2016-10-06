using Lesson6.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Lesson6.Controllers
{
    class StaticDateTime : Interfaces.IDateTime
    {
        public DateTime Now
        {
            get { return new DateTime(2016, 09, 01, 6, 0, 0); }
        }

    }

    public class HomeControllerTest
    {
        //[Fact]
        //public void HomeControllerContactTest()
        //{
        //    //Arrange
        //    var datetime = new StaticDateTime();
        //    var controller = new HomeController();

        //    //Act
        //    var result = controller.Contact(datetime);


        //    //Assert
            
        //    var viewResult = Assert.IsType<ViewResult>(result);

        //    var model = Assert.IsAssignableFrom<ContactViewModel>(viewResult.ViewData.Model);
        //    //  Assert.Equal(datetime.Now,viewResult.ViewData["Message"]);
        //    Assert.Equal(datetime.Now.ToString(), model.CurrentDateAndTime);
        //    Assert.Equal(2, model.Names.Count());
        //    Assert.Equal("Joakim", model.Names.ElementAt(0));

        //}
    }
}
