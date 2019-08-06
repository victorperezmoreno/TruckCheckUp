using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TruckCheckUp.WebUI.Controllers
{
    public class ErrorHandlerController : Controller
    {
        // GET: ErrorHandler
        public ActionResult Error()
        {
            return View();
        }
        // GET: Page not found
        public ActionResult NotFound()
        {
            return View();
        }
        //GET: Server Error
        public ActionResult ServerError()
        {
            return View();
        }
    }
}