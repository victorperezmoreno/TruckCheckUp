using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using TruckCheckUp.Core.Contracts.Logger;
using TruckCheckUp.Core.ViewModels.TruckModelUI;
using TruckCheckUp.Services;

namespace TruckCheckUp.WebUI.Controllers
{
    public class TruckModelManagementController : Controller
    {
        private TruckModelService _truckModelService;

        public TruckModelManagementController(TruckModelService truckModelService)
        {
            _truckModelService = truckModelService;
        }

        // GET: TruckModelManagement
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult ListOfModels()
        {
            try
            {
                var truckModels = _truckModelService.RetrieveAllTruckModels();
                return Json(truckModels, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult GetManufacturersList()
        {
            try
            {
                var truckManufacturers = _truckModelService.RetrieveAllTruckManufacturers();
                return Json(truckManufacturers, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult GetModelById(string Id)
        {
            try
            {
                var Model = _truckModelService.RetrieveModelById(Id);
                return Json(Model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: TruckModelManagement/SearchModelName
        [HttpPost]
        public JsonResult SearchModelName(TruckModelSaveUpdateViewModel truckModel)
        {
            try
            {
                var truckModelSearchResult = _truckModelService.SearchTruckModel(truckModel);
                return Json(truckModelSearchResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: TruckModelManagement/Add
        [HttpPost]
        public JsonResult Add(TruckModelSaveUpdateViewModel truckModel)
        {
            try
            {
                var truckModelAddResult = _truckModelService.AddTruckModel(truckModel);
                return Json(truckModelAddResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult Update(TruckModelSaveUpdateViewModel truckModel)
        {
            try
            {
                var truckModelUpdateResult = _truckModelService.UpdateTruckModel(truckModel);
                return Json(truckModelUpdateResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public JsonResult Delete(string Id)
        {
            try
            {
                _truckModelService.DeleteTruckModel(Id);
                return Json(Id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}