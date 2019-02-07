using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

        // GET: List all trucks
        public ActionResult Index()
        {
            //Get a list of trucks
            var truck = _truckService.RetrieveAllTrucks();

            return View(truck);
        }

        public ActionResult Create()
        {
            var truck = _truckService.CreateNewTruckObject();

            return View(truck);
        }

        [HttpPost]
        public ActionResult Create(TruckInsertViewModel truck)
        {
            //if errors found return to view, otherwise insert into DB
            if (!ModelState.IsValid)
            {
                return View(truck);
            }
            else
            {
                _truckService.PostNewTruckToDB(truck);
                //Once truck inserted then return to display list of trucks
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string Id)
        {
            //Look for truck in DB
            var truckToUpdate = _truckService.RetrieveTruckDataToUpdate(Id);

            return View(truckToUpdate);
        }

        [HttpPost]
        public ActionResult Edit(TruckUpdateViewModel truck, string Id)
        {
            if (!ModelState.IsValid)
            {
                return View(truck);
            }
            else
            {
                _truckService.UpdateTruckData(truck, Id);
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string Id)
        {
            var truckToDelete = _truckService.RetrieveTruckToDelete(Id);

            return View(truckToDelete);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            _truckService.DeleteTruck(Id);
            return RedirectToAction("Index");
        }
    }
}