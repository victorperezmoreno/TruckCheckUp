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
            var driver = _driverService.ListDrivers();

            return View(driver);
        }

        public ActionResult Create()
        {
            var driver = _driverService.CreateNewDriver();

            return View(driver);
        }

        [HttpPost]
        public ActionResult Create(DriverInsertViewModel driver)
        {
            //if errors found return to view, otherwise insert in DB
            if (!ModelState.IsValid)
            {
                return View(driver);
            }
            else
            {
                _driverService.CreateNewDriverPost(driver);
                //Once user inserted then return to display list of drivers
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string Id)
        {
            //Look for user in DB
            var driverToUpdate = _driverService.EditDriver(Id);

            return View(driverToUpdate);
        }

        [HttpPost]
        public ActionResult Edit(DriverEditViewModel driver, string Id)
        {
            if (!ModelState.IsValid)
            {
                return View(driver);
            }
            else
            {
                _driverService.ConfirmEditDriver(driver, Id);
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string Id)
        {
            var driverToDelete = _driverService.RemoveDriver(Id);

            return View(driverToDelete);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            _driverService.ConfirmRemoveDriver(Id);
            return RedirectToAction("Index");
        }
    }
}