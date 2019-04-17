using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruckCheckUp.Core.Contracts.DataAccess;
using TruckCheckUp.Core.Contracts.Logger;
using TruckCheckUp.Core.Contracts.Services;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.ViewModels.CatalogUI;

namespace TruckCheckUp.Services
{
    public class PartCatalogService : IPartCatalogService
    {
        private IRepository<PartCatalog> _partCatalogContext;
        private IRepository<PartCategory> _partCategoryContext;
        private ILogger _logger;
        private string tableNameUsedByLogger;

        public PartCatalogService(IRepository<PartCatalog> partCatalogContext, IRepository<PartCategory> partCategoryContext, ILogger logger)
        {
            _partCatalogContext = partCatalogContext;
            _logger = logger;
            _partCategoryContext = partCategoryContext;
            tableNameUsedByLogger = "PartCatalog";
        }

        // GET: Driver
        public List<PartListViewModel> RetrieveAllParts()
        {
            //Get a list of parts
            var partsCatalogRetrievedFromDB = _partCatalogContext.Collection().OrderBy(p => p.PartName).ToList();

            var partsViewModel = partsCatalogRetrievedFromDB.Select(part => new PartListViewModel
            {
                Id = part.Id,
                PartName = part.PartName,
                CategoryName = part.PartCategory.CategoryPart

            }).ToList();

            return partsViewModel;
        }

        public PartViewModel CreateNewPartObject()
        {
            var part = new PartViewModel();

            part.PartCatalog = new PartCatalog();
            part.Categories = RetrieveAllPartCategories();
            return part;
        }

        public void PostNewPartToDB(PartCatalog part)
        {
            _partCatalogContext.Insert(part);
            _partCatalogContext.Commit();
            _logger.Info("Inserted record Id " + part.Id + " into Table " + tableNameUsedByLogger);
        }

        public PartViewModel RetrievePartDataToUpdate(string partId)
        {
            //Look for part and return its data
            var partRetrievedFromDB = _partCatalogContext.Find(partId);
            var partToUpdateViewModel = new PartViewModel();
            if (partRetrievedFromDB != null)
            {
                partToUpdateViewModel.PartCatalog = partRetrievedFromDB;
                partToUpdateViewModel.Categories = RetrieveAllPartCategories();
            }
            return partToUpdateViewModel;
        }

        public void UpdatePartData(PartCatalog part, string partId)
        {
            var partToUpdate = _partCatalogContext.Find(partId);
            if (partToUpdate != null)
            {
                _logger.Info("Found record Id " + partToUpdate.Id + " in Table " + tableNameUsedByLogger);
                partToUpdate.PartName = part.PartName;
                partToUpdate.PartCategoryId = part.PartCategoryId;
                _partCatalogContext.Commit();
                _logger.Info("Updated record Id " + partToUpdate.Id + " in Table " + tableNameUsedByLogger);
            }
        }

        public PartCatalog RetrievePartToDelete(string partId)
        {
            var partToDelete = _partCatalogContext.Find(partId);
            if (partToDelete != null)
            {
                return partToDelete;
            }
            else
            {
                return new PartCatalog();
            }
        }

        public void DeletePart(string partId)
        {
            var partToDelete = _partCatalogContext.Find(partId);
            if (partToDelete != null)
            {
                _logger.Info("Found record Id " + partToDelete.Id + " in Table " + tableNameUsedByLogger);
                _partCatalogContext.Delete(partToDelete);
                _partCatalogContext.Commit();
                _logger.Info("Deleted record Id " + partToDelete.Id + " from Table " + tableNameUsedByLogger);

            }
        }

        public List<PartCategory> RetrieveAllPartCategories()
        {
            return _partCategoryContext.Collection().OrderBy(c => c.CategoryPart).ToList();
        }
    }
}
