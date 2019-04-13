using System.Collections.Generic;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.ViewModels.CategoryUI;

namespace TruckCheckUp.Core.Contracts.Services
{
    public interface IPartCategoryService
    {
        CategoryInsertViewModel CreateNewPartCategoryObject();
        void DeletePartCategory(string partCategoryId);
        void PostNewPartCategoryToDB(CategoryInsertViewModel partCategory);
        List<PartCategory> RetrieveAllCategories();
        CategoryUpdateViewModel RetrievePartCategoryDataToUpdate(string partCategoryId);
        PartCategory RetrievePartCategoryToDelete(string partCategoryId);
        void UpdatePartCategoryData(CategoryUpdateViewModel partCategory, string partCategoryId);
    }
}