using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TruckCheckUp.WebUI.Controllers
{
    public class AdministrationController : Controller
    {
        // GET: Administration. Only Administrator allowed to add/delete data
        //[Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }
    }
}