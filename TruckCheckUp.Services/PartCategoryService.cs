using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruckCheckUp.Core.Contracts.DataAccess;
using TruckCheckUp.Core.Contracts.Services;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.ViewModels.CategoryUI;

namespace TruckCheckUp.Services
{
    public class PartCategoryService : IPartCategoryService
    {
        private IRepository<PartCategory> _partCategoryContext;

        public PartCategoryService(IRepository<PartCategory> partCategoryContext)
        {
            _partCategoryContext = partCategoryContext;
        }

        // GET: Driver
        public List<PartCategory> RetrieveAllCategories()
        {
            //Get a list of drivers
            var partCategoriesRetrieved = _partCategoryContext.Collection().ToList();

            return partCategoriesRetrieved;
        }

        public CategoryInsertViewModel CreateNewPartCategoryObject()
        {
            var partCategory = new CategoryInsertViewModel();

            return partCategory;
        }

        public void PostNewPartCategoryToDB(CategoryInsertViewModel partCategory)
        {
            var partCategoryToInsert = new PartCategory();
            partCategoryToInsert.CategoryPart = partCategory.CategoryPart;
            _partCategoryContext.Insert(partCategoryToInsert);
            _partCategoryContext.Commit();
        }

        public CategoryUpdateViewModel RetrievePartCategoryDataToUpdate(string partCategoryId)
        {
            //Look for driver and return her data
            var partCategoryToUpdate = (
                    from partCategoryStoredInDB in _partCategoryContext.Collection()
                    where partCategoryStoredInDB.Id == partCategoryId
                    select new CategoryUpdateViewModel()
                    {
                        Id = partCategoryStoredInDB.Id,
                        CategoryPart = partCategoryStoredInDB.CategoryPart
                    }).FirstOrDefault();

            if (partCategoryToUpdate != null)
            {
                return partCategoryToUpdate;
            }
            else
            {
                return new CategoryUpdateViewModel();
            }
        }

        public void UpdatePartCategoryData(CategoryUpdateViewModel partCategory, string partCategoryId)
        {
            var partCategoryToUpdate = _partCategoryContext.Find(partCategoryId);
            if (partCategoryToUpdate != null)
            {
                partCategoryToUpdate.CategoryPart = partCategory.CategoryPart;
                _partCategoryContext.Commit();
            }
        }

        public PartCategory RetrievePartCategoryToDelete(string partCategoryId)
        {
            var partCategoryToDelete = _partCategoryContext.Find(partCategoryId);
            if (partCategoryToDelete != null)
            {
                return partCategoryToDelete;
            }
            else
            {
                return new PartCategory();
            }
        }

        public void DeletePartCategory(string partCategoryId)
        {
            var partCategoryToDelete = _partCategoryContext.Find(partCategoryId);
            if (partCategoryToDelete != null)
            {
                _partCategoryContext.Delete(partCategoryToDelete);
                _partCategoryContext.Commit();
            }
        }
    }
}
