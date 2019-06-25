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
        //private ILogger _logger;
        string tableNameUsedByLogger = "";
        public TruckManufacturerService(IRepository<TruckManufacturer> truckManufacturerContext,ILogger logger)
        {
            _truckManufacturerContext = truckManufacturerContext;
           // _logger = logger;
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

        public TruckManufacturerViewModel ValidateTruckManufacturerToAdd(TruckManufacturerViewModel truckManufacturer)
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
                        SaveNewTruckManufacturerData(truckManufacturer, new Log4NetLogger());
                    }
                }
            }
            return truckManufacturer;
        }

        public TruckManufacturerViewModel ValidateTruckManufacturerToUpdate(TruckManufacturerViewModel truckManufacturer)
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
                        UpdateTruckManufacturerData(truckManufacturer, new Log4NetLogger());
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

        public void SaveNewTruckManufacturerData(TruckManufacturerViewModel truckManufacturer, ILogger logger)
        {
            var truckManufacturerToInsert = new TruckManufacturer();
            truckManufacturerToInsert.ManufacturerDescription = truckManufacturer.Description;

            _truckManufacturerContext.Insert(truckManufacturerToInsert);
            _truckManufacturerContext.Commit();  
            logger.Info("Inserted record Id " + truckManufacturerToInsert.Id + " into Table " + tableNameUsedByLogger);

        }

        public void UpdateTruckManufacturerData(TruckManufacturerViewModel truckManufacturer, ILogger logger)
        {
            var truckManufacturerToUpdate = _truckManufacturerContext.Find(truckManufacturer.Id);
            if (truckManufacturerToUpdate != null)
            {
                logger.Info("Found record Id " + truckManufacturerToUpdate.Id + " in Table " + tableNameUsedByLogger);
                truckManufacturerToUpdate.ManufacturerDescription = truckManufacturer.Description;
                _truckManufacturerContext.Commit();
                logger.Info("Updated record Id " + truckManufacturerToUpdate.Id + " in Table " + tableNameUsedByLogger);

            }
        }

        public void ValidateTruckManufacturerToDelete(string truckManufacturerId)
        {
            var truckManufacturerFound = _truckManufacturerContext.Find(truckManufacturerId);
            if (truckManufacturerFound != null)
            {
                DeleteTruckManufacturerData(truckManufacturerFound, new Log4NetLogger());
            }            
        }

        public void DeleteTruckManufacturerData(TruckManufacturer truckManufacturerToDelete, ILogger logger)
        {
            logger.Info("Found record Id " + truckManufacturerToDelete.Id + " in Table " + tableNameUsedByLogger);
            _truckManufacturerContext.Delete(truckManufacturerToDelete);
            _truckManufacturerContext.Commit();
            logger.Info("Deleted record Id " + truckManufacturerToDelete.Id + " from Table " + tableNameUsedByLogger);

        }

    }
}
