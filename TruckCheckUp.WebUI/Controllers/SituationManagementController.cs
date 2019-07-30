using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using TruckCheckUp.Core.Contracts.Services;
using TruckCheckUp.Core.ViewModels.SituationUI;
using TruckCheckUp.Services;

namespace TruckCheckUp.WebUI.Controllers
{
    public class SituationManagementController : Controller
    {
        private ISituationService _situationService;

        public SituationManagementController(ISituationService situationService)
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
                var situationResultsList = _situationService.RetrieveAllSituations();
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

        public JsonResult GetSituationById(string Id)
        {
            try
            {
                var situationByIdResult = _situationService.RetrieveSituationById(Id);
                return Json(situationByIdResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult Update(SituationViewModel situationObject)
        {
            try
            {
                var situationUpdateResult = _situationService.UpdateSituation(situationObject);
                return Json(situationUpdateResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult SearchSituationDescription (SituationViewModel situationObject)
        {
            try
            {
                var searchSituationResult = _situationService.SearchSituation(situationObject);
                return Json(searchSituationResult, JsonRequestBehavior.AllowGet);
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