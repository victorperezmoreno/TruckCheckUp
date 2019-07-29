using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using TruckCheckUp.Core.ViewModels.SituationUI;
using TruckCheckUp.Services;

namespace TruckCheckUp.WebUI.Controllers
{
    public class SituationManagementController : Controller
    {
        private SituationService _situationService;

        public SituationManagementController(SituationService situationService)
        {
            _situationService = situationService;
        }

        // GET: SituationManagement
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult ListOfSituations()
        {
            try
            {
                var situationResultsList = _situationService.RetrieveAllSituationsfromDatabase();
                return Json(situationResultsList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult Add(SituationViewModel situationObject)
        {
            try
            {
                var situationAddResult = _situationService.AddSituation(situationObject);
                return Json(situationAddResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            } 
        }

        [HttpPost]
        public JsonResult Delete (string Id)
        {
            try
            {
                _situationService.DeleteSituation(Id);
                return Json(Id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}