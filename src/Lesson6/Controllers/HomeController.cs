using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lesson6.Interfaces;
using Lesson6.ViewModels;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;

namespace Lesson6.Controllers
{
    public class HomeController : Controller
    {

      //  private readonly IDateTime _datetime;
        public HomeController(/*IDateTime datetime*/)
        {
          //  _datetime = datetime;

        }
       

        public IActionResult Index()
        {
            return View();
        }

     

        public IActionResult About(string id, string name, string lang)
        {

            ViewData["Message"] = lang +" "+ id + " " + name;

            return View();
        }
        
        public IActionResult Contact([FromServices] IDateTime _datetime)
        {
            //ViewData["Message"] = _datetime.Now;

            ContactViewModel vmodel = new ContactViewModel();
            vmodel.CurrentDateAndTime = _datetime.Now.ToString();
            vmodel.id = 0;

            List<string> list = new List<string>();
            list.Add("Joakim");
            list.Add("Sven");
            vmodel.Names = list;

            return View(vmodel);

        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
