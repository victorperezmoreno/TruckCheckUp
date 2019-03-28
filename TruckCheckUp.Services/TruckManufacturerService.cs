using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TruckCheckUp.Core.Contracts.DataAccess;
using TruckCheckUp.Core.Contracts.Logger;
using TruckCheckUp.Core.Contracts.Services;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.ViewModels.TruckManufacturerUI;

namespace TruckCheckUp.Services
{
    public class TruckManufacturerService : ITruckManufacturerService
    {
        private IRepository<TruckManufacturer> _truckManufacturerContext;
        private ILogger _logger;
        string tableNameUsedByLogger = "";
        public TruckManufacturerService(IRepository<TruckManufacturer> truckManufacturerContext,ILogger logger)
        {
            _truckManufacturerContext = truckManufacturerContext;
            _logger = logger;
            tableNameUsedByLogger = "TruckManufacturer";
        }

        public List<TruckManufacturerViewModel> RetrieveAllTruckManufacturers()
        {
            //Get a list of Trucks
            var manufacturersRetrieved = _truckManufacturerContext.Collection().OrderBy(m => m.ManufacturerDescription).ToList();

            var manufacturersViewModel = manufacturersRetrieved.Select(manufacturer => new TruckManufacturerViewModel
            {
                Id                      = manufacturer.Id,
                Description = manufacturer.ManufacturerDescription

            }).ToList();

            return manufacturersViewModel;
        }

        public TruckManufacturerViewModel AddTruckManufacturer(TruckManufacturerViewModel truckManufacturer)
        {
            if (!string.IsNullOrEmpty(truckManufacturer.Description))
            {

                //Verify that only letters and numbers in string manufacturer entered by user
                if (!ValidateManufacturerString(truckManufacturer.Description))
                {
                    truckManufacturer.IsValid = false;
                }
                else
                {
                    //Verify whether the manufacturer is already in DB
                    truckManufacturer.ExistInDB = RetrieveTruckManufacturerName(truckManufacturer.Description);
                    if (!truckManufacturer.ExistInDB)
                    {
                        PostNewTruckManufacturerToDB(truckManufacturer);
                    }
                }
            }
            return truckManufacturer;
        }

        public TruckManufacturerViewModel UpdateTruckManufacturer(TruckManufacturerViewModel truckManufacturer)
        {
            if (!string.IsNullOrEmpty(truckManufacturer.Description))
            {

                //Verify that only letters and numbers in string manufacturer entered by user
                if (!ValidateManufacturerString(truckManufacturer.Description))
                {
                    truckManufacturer.IsValid = false;
                }
                else
                {
                    //Verify whether the manufacturer is already in DB and save value 
                    //in object to return to View for validation purposes 
                    truckManufacturer.ExistInDB = RetrieveTruckManufacturerName(truckManufacturer.Description);
                    
                    if (!truckManufacturer.ExistInDB)
                    {
                        UpdateTruckManufacturerData(truckManufacturer);
                    }
                }
            }
            return truckManufacturer;
        }

        public TruckManufacturerViewModel SearchTruckManufacturer(TruckManufacturerViewModel manufacturer)
        {
            var truckSearchResult = new TruckManufacturer();
            if (!string.IsNullOrEmpty(manufacturer.Description))
            {                
                //Search for manufacturer in DB only if numbers and letters in manufacturer name
                if (!ValidateManufacturerString(manufacturer.Description))
                {
                    manufacturer.IsValid = false;                
                }
                else
                {
                    truckSearchResult = _truckManufacturerContext.Collection().Where(m => m.ManufacturerDescription == manufacturer.Description).FirstOrDefault();
                    if (truckSearchResult == null)
                    {
                        manufacturer.ExistInDB = false;
                    }
                    else
                    {
                        manufacturer.Id = truckSearchResult.Id;
                        manufacturer.Description = truckSearchResult.ManufacturerDescription;
                    }
                }
            }
            return manufacturer;
        }

        public bool RetrieveTruckManufacturerName(string manufacturerName)
        {
            //Check whether truck manufacturer already in DB
            var truckRetrieved = _truckManufacturerContext.Collection().Any(manufacturer => manufacturer.ManufacturerDescription == manufacturerName);
            return truckRetrieved;
        }

        public bool ValidateManufacturerString(string manufacturer)
        {
            Regex regexManufacturer = new Regex(@"^[a-zA-Z0-9]+$");
            return (regexManufacturer.IsMatch(manufacturer));
        }

        public void PostNewTruckManufacturerToDB(TruckManufacturerViewModel truckManufacturer)
        {
            var truckManufacturerToInsert = new TruckManufacturer();
            truckManufacturerToInsert.ManufacturerDescription = truckManufacturer.Description;

            _truckManufacturerContext.Insert(truckManufacturerToInsert);
            _truckManufacturerContext.Commit();  
            _logger.Info("Inserted record Id " + truckManufacturerToInsert.Id + " into Table " + tableNameUsedByLogger);

        }

        public void UpdateTruckManufacturerData(TruckManufacturerViewModel truckManufacturer)
        {
            var truckManufacturerToUpdate = _truckManufacturerContext.Find(truckManufacturer.Id);
            if (truckManufacturerToUpdate != null)
            {
                _logger.Info("Found record Id " + truckManufacturerToUpdate.Id + " in Table " + tableNameUsedByLogger);
                truckManufacturerToUpdate.ManufacturerDescription = truckManufacturer.Description;
                _truckManufacturerContext.Commit();
                _logger.Info("Updated record Id " + truckManufacturerToUpdate.Id + " in Table " + tableNameUsedByLogger);

            }
        }

        public void DeleteTruckManufacturer(string truckManufacturerId)
        {
            var truckManufacturerToDelete = _truckManufacturerContext.Find(truckManufacturerId);
            if (truckManufacturerToDelete != null)
            {
                _logger.Info("Found record Id " + truckManufacturerToDelete.Id + " in Table " + tableNameUsedByLogger);
                _truckManufacturerContext.Delete(truckManufacturerToDelete);
                _truckManufacturerContext.Commit();
                _logger.Info("Deleted record Id " + truckManufacturerToDelete.Id + " from Table " + tableNameUsedByLogger);
            }            
        }

    }
}
