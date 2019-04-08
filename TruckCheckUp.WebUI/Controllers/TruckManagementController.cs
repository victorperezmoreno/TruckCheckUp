using System;
using System.Web.Mvc;
using System.Web.UI;
using TruckCheckUp.Core.ViewModels.TruckUI;
using TruckCheckUp.Services;

namespace TruckCheckUp.WebUI.Controllers
{
    public class TruckManagementController : Controller
    {
        private TruckService _truckService;
        
        public TruckManagementController(TruckService truckService)
        {
            _truckService = truckService;
        }

        // GET: TruckModelManagement
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult ListOfTrucks()
        {
            try
            {
                var trucks = _truckService.RetrieveAllTrucks();
                return Json(trucks, JsonRequestBehavior.AllowGet);
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
                var truckManufacturers = _truckService.RetrieveAllTruckManufacturers();
                return Json(truckManufacturers, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult GetModelAndYearLists(string Id)
        {
            try
            {
                var truck = _truckService.RetrieveModelAndYearListsFromDB(Id);
                return Json(truck, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult GetTruckById(string Id)
        {
            try
            {
                var Truck = _truckService.RetrieveTruckById(Id);
                return Json(Truck, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: TruckManagement/SearchTruckNumber
        [HttpPost]
        public JsonResult SearchTruckNumber(TruckSaveUpdateViewModel truck)
        {
            try
            {
                var truckModelSearchResult = _truckService.SearchTruck(truck);
                return Json(truckModelSearchResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: TruckManagement/Add
        [HttpPost]
        public JsonResult Add(TruckSaveUpdateViewModel truck)
        {
            try
            {
                var truckAddResult = _truckService.AddTruck(truck);
                return Json(truckAddResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult Update(TruckSaveUpdateViewModel truck)
        {
            try
            {
                var truckUpdateResult = _truckService.UpdateTruck(truck);
                return Json(truckUpdateResult, JsonRequestBehavior.AllowGet);
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
                _truckService.DeleteTruck(Id);
                return Json(Id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}

//--------------------------------------------------------------------
//        public TruckManagementController(TruckService truckService)
//        {
//            _truckService = truckService;
//        }

//        // GET: List all trucks
//        public ActionResult Index()
//        {
//            //Get a list of trucks
//            var truck = _truckService.RetrieveAllTrucks();

//            return View(truck);
//        }

//        public ActionResult Create()
//        {
//            var truck = _truckService.CreateNewTruckObject();

//            return View(truck);
//        }

//        [HttpPost]
//        public ActionResult Create(TruckInsertViewModel truck)
//        {
//            //if errors found return to view, otherwise insert into DB
//            if (!ModelState.IsValid)
//            {
//                return View(truck);
//            }
//            else
//            {
//                _truckService.PostNewTruckToDB(truck);
//                //Once truck inserted then return to display list of trucks
//                return RedirectToAction("Index");
//            }
//        }

//        public ActionResult Edit(string Id)
//        {
//            //Look for truck in DB
//            var truckToUpdate = _truckService.RetrieveTruckDataToUpdate(Id);

//            return View(truckToUpdate);
//        }

//        [HttpPost]
//        public ActionResult Edit(TruckUpdateViewModel truck, string Id)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(truck);
//            }
//            else
//            {
//                _truckService.UpdateTruckData(truck, Id);
//                return RedirectToAction("Index");
//            }
//        }

//        public ActionResult Delete(string Id)
//        {
//            var truckToDelete = _truckService.RetrieveTruckToDelete(Id);

//            return View(truckToDelete);
//        }

//        [HttpPost]
//        [ActionName("Delete")]
//        public ActionResult ConfirmDelete(string Id)
//        {
//            _truckService.DeleteTruck(Id);
//            return RedirectToAction("Index");
//        }
//    }
//}