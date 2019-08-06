using System.Collections.Generic;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.ViewModels.CatalogUI;

namespace TruckCheckUp.Core.Contracts.Services
{
    public interface IPartCatalogService
    {
        PartViewModel CreateNewPartObject();
        void DeletePart(string partId);
        void PostNewPartToDB(PartCatalog part);
        List<PartCategory> RetrieveAllPartCategories();
        List<PartListViewModel> RetrieveAllParts();
        PartViewModel RetrievePartDataToUpdate(string partId);
        PartCatalog RetrievePartToDelete(string partId);
        void UpdatePartData(PartCatalog part, string partId);
    }
}