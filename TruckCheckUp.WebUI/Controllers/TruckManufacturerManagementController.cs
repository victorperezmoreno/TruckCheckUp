using System;
using System.Web.Mvc;
using System.Web.UI;
using TruckCheckUp.Core.Contracts.Logger;
using TruckCheckUp.Core.ViewModels.TruckManufacturerUI;
using TruckCheckUp.Services;

namespace TruckCheckUp.WebUI.Controllers
{
    public class TruckManufacturerManagementController : Controller
    {
        private TruckManufacturerService _truckManufacturerService;
        //, ILogger logger

        public TruckManufacturerManagementController(TruckManufacturerService truckManufacturerService)
        {
            this._truckManufacturerService = truckManufacturerService;
           // this._logger = logger;
        }

        // GET: TruckManufacturerManagement
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult ListOfManufacturers()
        {
            try
            {
                var truckManufacturers = _truckManufacturerService.RetrieveAllTruckManufacturers();
                return Json(truckManufacturers, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult GetManufacturerbyId(string Id)
        {
            try
            {
                var Manufacturer = _truckManufacturerService.RetrieveAllTruckManufacturers().Find(manufacturer => manufacturer.Id.Equals(Id));
                return Json(Manufacturer, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: TruckManufacturerManagement/SearchManufacturerName
        [HttpPost]
        public JsonResult SearchManufacturerName(TruckManufacturerViewModel truckManufacturer)
        {
            try
            { 
                var truckManufacturerSearchResult = _truckManufacturerService.SearchTruckManufacturer(truckManufacturer);
                return Json(truckManufacturerSearchResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            } 
        }

        // GET: TruckManufacturerManagement/Add
        [HttpPost]
        public JsonResult Add(TruckManufacturerViewModel truckManufacturer)
        {
            try
            {
                var truckManufacturerAddResult = _truckManufacturerService.ValidateTruckManufacturerToAdd(truckManufacturer);
                return Json(truckManufacturerAddResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }   
        }

        [HttpPost]
        public JsonResult Update(TruckManufacturerViewModel truckManufacturer)
        {
            try
            {
                var truckManufacturerUpdateResult = _truckManufacturerService.ValidateTruckManufacturerToUpdate(truckManufacturer);
                return Json(truckManufacturerUpdateResult, JsonRequestBehavior.AllowGet);
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
              _truckManufacturerService.ValidateTruckManufacturerToDelete(Id);
              return Json(Id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
              throw;
            }         
        }
    }
}
