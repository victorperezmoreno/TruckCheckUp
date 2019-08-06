using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.ViewModels;
using TruckCheckUp.Core.ViewModels.DriverUI;
using TruckCheckUp.Services;

namespace TruckCheckUp.WebUI.Controllers
{
    public class DriverManagementController : Controller
    {
        private DriverService  _driverService;

        public DriverManagementController(DriverService driverService)
        {
            this._driverService = driverService;
        }
        
        // GET: List all Drivers
        public ActionResult Index()
        {
            //Get a list of drivers
            var drivers = _driverService.RetrieveAllDrivers();

            return View(drivers);
        }

        public ActionResult Create()
        {
            var driver = _driverService.CreateNewDriverObject();

            return View(driver);
        }

        [HttpPost]
        public ActionResult Create(DriverInsertViewModel driver)
        {
            //if errors found return to view, otherwise insert into DB
            if (!ModelState.IsValid)
            {
                return View(driver);
            }
            else
            {
                _driverService.PostNewDriverToDB(driver);
                //Once driver inserted then return to display list of drivers
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string Id)
        {
            //Look for driver in DB
            var driverToUpdate = _driverService.RetrieveDriverDataToUpdate(Id);

            return View(driverToUpdate);
        }

        [HttpPost]
        public ActionResult Edit(DriverUpdateViewModel driver, string Id)
        {
            if (!ModelState.IsValid)
            {
                return View(driver);
            }
            else
            {
                _driverService.UpdateDriverData(driver, Id);
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string Id)
        {
            var driverToDelete = _driverService.RetrieveDriverToDelete(Id);

            return View(driverToDelete);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            _driverService.DeleteDriver(Id);
            return RedirectToAction("Index");
        }
    }
}