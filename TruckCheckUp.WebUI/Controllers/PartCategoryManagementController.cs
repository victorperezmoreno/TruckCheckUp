using System.Web.Mvc;
using TruckCheckUp.Core.ViewModels.CategoryUI;
using TruckCheckUp.Services;

namespace TruckCheckUp.WebUI.Controllers
{
    public class PartCategoryManagementController : Controller
    {
        // GET: PartCategoryManagement
        private PartCategoryService _partCategoryService;

        public PartCategoryManagementController(PartCategoryService partCategoryService)
        {
            this._partCategoryService = partCategoryService;
        }

        // GET: List all Part Categories
        public ActionResult Index()
        {
            //Get a list of part categories
            var partCategories = _partCategoryService.RetrieveAllCategories();

            return View(partCategories);
        }

        public ActionResult Create()
        {
            var partCategory = _partCategoryService.CreateNewPartCategoryObject();

            return View(partCategory);
        }

        [HttpPost]
        public ActionResult Create(CategoryInsertViewModel partCategory)
        {
            //if errors found return to view, otherwise insert into DB
            if (!ModelState.IsValid)
            {
                return View(partCategory);
            }
            else
            {
                _partCategoryService.PostNewPartCategoryToDB(partCategory);
                //Once part category inserted then return to display list of categories
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string Id)
        {
            //Look for part category in DB
            var partCategoryToUpdate = _partCategoryService.RetrievePartCategoryDataToUpdate(Id);

            return View(partCategoryToUpdate);
        }

        [HttpPost]
        public ActionResult Edit(CategoryUpdateViewModel partCategory, string Id)
        {
            if (!ModelState.IsValid)
            {
                return View(partCategory);
            }
            else
            {
                _partCategoryService.UpdatePartCategoryData(partCategory, Id);
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string Id)
        {
            var partCategoryToDelete = _partCategoryService.RetrievePartCategoryToDelete(Id);

            return View(partCategoryToDelete);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            _partCategoryService.DeletePartCategory(Id);
            return RedirectToAction("Index");
        }
    }
}