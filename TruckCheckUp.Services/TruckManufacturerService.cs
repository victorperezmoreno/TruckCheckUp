using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TruckCheckUp.Core.Contracts.DataAccess;
using TruckCheckUp.Core.Contracts.Services;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.ViewModels.TruckManufacturerUI;

namespace TruckCheckUp.Services
{
    public class TruckManufacturerService : ITruckManufacturerService
    {
        private IRepository<TruckManufacturer> _truckManufacturerContext;

        public TruckManufacturerService(IRepository<TruckManufacturer> truckManufacturerContext)
        {
            _truckManufacturerContext = truckManufacturerContext;
        }

        public List<TruckManufacturerViewModel> RetrieveAllTruckManufacturers()
        {
            //Get a list of Trucks
            var trucksRetrieved = _truckManufacturerContext.Collection().OrderBy(m => m.ManufacturerDescription).ToList();

            var trucksViewModel = trucksRetrieved.Select(truck => new TruckManufacturerViewModel
            {
                Id                      =truck.Id,
                ManufacturerDescription = truck.ManufacturerDescription

            }).ToList();

            return trucksViewModel;
        }

        public TruckManufacturerViewModel AddTruckManufacturer(TruckManufacturerViewModel truckManufacturer)
        {
            if (!string.IsNullOrEmpty(truckManufacturer.ManufacturerDescription))
            {

                //Verify that only letters and numbers in string manufacturer entered by user
                if (!ValidateManufacturerString(truckManufacturer.ManufacturerDescription))
                {
                    truckManufacturer.ManufacturerIsValid = false;
                    //return Json(truckManufacturer, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //Verify whether the manufacturer is already in DB
                    truckManufacturer.ManufacturerExistInDB = RetrieveTruckManufacturerName(truckManufacturer.ManufacturerDescription);
                    if (!truckManufacturer.ManufacturerExistInDB)
                    {
                        PostNewTruckManufacturerToDB(truckManufacturer);
                    }
                }
            }
            return truckManufacturer;
        }

        public TruckManufacturerViewModel UpdateTruckManufacturer(TruckManufacturerViewModel truckManufacturer)
        {
            if (!string.IsNullOrEmpty(truckManufacturer.ManufacturerDescription))
            {

                //Verify that only letters and numbers in string manufacturer entered by user
                if (!ValidateManufacturerString(truckManufacturer.ManufacturerDescription))
                {
                    truckManufacturer.ManufacturerIsValid = false;
                    //return Json(truckManufacturer, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //Verify whether the manufacturer is already in DB and save value 
                    //in object to return to View for validation purposes 
                    truckManufacturer.ManufacturerExistInDB = RetrieveTruckManufacturerName(truckManufacturer.ManufacturerDescription);
                    
                    if (!truckManufacturer.ManufacturerExistInDB)
                    {
                        UpdateTruckManufacturerData(truckManufacturer);
                    }
                }
            }
            return truckManufacturer;
        }

        public TruckManufacturerViewModel SearchTruckManufacturer(string manufacturerName)
        {
            var truckSearchResult = new TruckManufacturer();
            var truckObjectToReturn = new TruckManufacturerViewModel();
            if (!string.IsNullOrEmpty(manufacturerName))
            {                
                //Search for manufacturer in DB only if numbers and letters in manufacturer name
                if (!ValidateManufacturerString(manufacturerName))
                {
                    truckObjectToReturn.ManufacturerIsValid = false;                    
                }
                else
                {
                    truckSearchResult = _truckManufacturerContext.Collection().Where(manufacturer => manufacturer.ManufacturerDescription == manufacturerName).FirstOrDefault();
                    if (truckSearchResult == null)
                    {
                        truckObjectToReturn.ManufacturerExistInDB = false;
                    }
                    else
                    {
                        truckObjectToReturn.Id = truckSearchResult.Id;
                        truckObjectToReturn.ManufacturerDescription = truckSearchResult.ManufacturerDescription;
                    }
                }
            }  
            return truckObjectToReturn;
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
            truckManufacturerToInsert.ManufacturerDescription = truckManufacturer.ManufacturerDescription;

            _truckManufacturerContext.Insert(truckManufacturerToInsert);
            _truckManufacturerContext.Commit();
        }

        public void UpdateTruckManufacturerData(TruckManufacturerViewModel truckManufacturer)
        {
            var truckManufacturerToUpdate = _truckManufacturerContext.Find(truckManufacturer.Id);
            if (truckManufacturerToUpdate != null)
            {
                truckManufacturerToUpdate.ManufacturerDescription = truckManufacturer.ManufacturerDescription;
                _truckManufacturerContext.Commit();
            }
        }

        public void DeleteTruckManufacturer(string truckManufacturerId)
        {
            var truckManufacturerToDelete = _truckManufacturerContext.Find(truckManufacturerId);
            if (truckManufacturerToDelete != null)
            {
                _truckManufacturerContext.Delete(truckManufacturerId);
                _truckManufacturerContext.Commit();
            }
        }

    }
}
