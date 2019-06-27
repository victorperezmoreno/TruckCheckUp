using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TruckCheckUp.Contracts.Services;
using TruckCheckUp.Core.ViewModels;
using TruckCheckUp.Core.ViewModels.TruckInspection;
using TruckCheckUp.Services;

namespace TruckCheckUp.WebUI.Controllers
{
    public class TruckInspectionController : Controller
    {
        private ITruckInspectionService _truckInspectionService;

        public TruckInspectionController(ITruckInspectionService truckInspectionService)
        {
            _truckInspectionService = truckInspectionService;
        }

        public ActionResult Create()
        {
            var inspectionObject = _truckInspectionService.CreateNewTruckInspectionObject();
            return View(inspectionObject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TruckInspectionViewModel truckInspection)
        {
            if (!ModelState.IsValid)
            {
                _truckInspectionService.PopulateDriversTrucksAndPartsCatalog(truckInspection);
                return View(truckInspection);
            }
            else
            {
                var truckInspectionReturnedFromServiceLayer = _truckInspectionService.CreateTruckInspection(truckInspection);
                if (truckInspectionReturnedFromServiceLayer.AlertStyle == AlertStyle.Success)
                {
                    return View(_truckInspectionService.PopulateDriversTrucksAndPartsCatalog(truckInspectionReturnedFromServiceLayer));
                }
                else
                {
                    int currentMileageConvertedToInt;
                    if (int.TryParse(truckInspectionReturnedFromServiceLayer.CurrentMileage, out currentMileageConvertedToInt))
                    {
                        if (currentMileageConvertedToInt < truckInspectionReturnedFromServiceLayer.LastMileageReported)
                        {
                            ModelState.AddModelError("CurrentMileage", "Mileage entered must be greater than " + truckInspection.LastMileageReported.ToString());
                        }
                    }
                    _truckInspectionService.PopulateDriversTrucksAndPartsCatalog(truckInspectionReturnedFromServiceLayer);
                    return View(truckInspectionReturnedFromServiceLayer);
                }
            }
        }
    }
}