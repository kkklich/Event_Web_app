using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KLich;


namespace KLich.Controllers
{
    public class HomeController : Controller
    {       

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
                      
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Dispose(bool disposing)
        {         
            base.Dispose(disposing);
        }
    }
}