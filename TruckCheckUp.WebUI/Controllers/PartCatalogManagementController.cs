using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.ViewModels.CatalogUI;
using TruckCheckUp.Services;

namespace TruckCheckUp.WebUI.Controllers
{
    public class PartCatalogManagementController : Controller
    {
        // GET: PartCatalogManagement
        private PartCatalogService _partCatalogService;

        public PartCatalogManagementController(PartCatalogService partCatalogService)
        {
            this._partCatalogService = partCatalogService;
        }

        // GET: List all Parts
        public ActionResult Index()
        {
            //Get a list of parts from DB
            var partCatalog = _partCatalogService.RetrieveAllParts();

            return View(partCatalog);
        }

        public ActionResult Create()
        {
            var partCatalog = _partCatalogService.CreateNewPartObject();

            return View(partCatalog);
        }

        [HttpPost]
        public ActionResult Create(PartViewModel part)
        {
            //if errors found return to view, otherwise insert into DB
            if (!ModelState.IsValid)
            {
                //Populate Categories DropDownList
                part.Categories = _partCatalogService.RetrieveAllPartCategories();
                return View(part);
            }
            else
            {
                _partCatalogService.PostNewPartToDB(part.PartCatalog);
                //Once part inserted then return to display list of parts
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string Id)
        {
            //Look for part in DB
            var partToUpdate = _partCatalogService.RetrievePartDataToUpdate(Id);

            return View(partToUpdate);
        }

        [HttpPost]
        public ActionResult Edit(PartViewModel part, string Id)
        {
            //if errors found return to view, otherwise insert into DB
            if (!ModelState.IsValid)
            {
                //Populate Categories DropDownList
                part.Categories = _partCatalogService.RetrieveAllPartCategories();
                return View(part);
            }
            else
            {
                _partCatalogService.UpdatePartData(part.PartCatalog, Id);
                //Once part updated then return to display list of parts
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string Id)
        {
            var partCatalogToDelete = _partCatalogService.RetrievePartToDelete(Id);

            return View(partCatalogToDelete);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            _partCatalogService.DeletePart(Id);
            return RedirectToAction("Index");
        }
    }
}