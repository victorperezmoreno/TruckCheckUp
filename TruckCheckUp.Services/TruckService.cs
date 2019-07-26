using System;
using System.Collections.Generic;
using System.Linq;
using TruckCheckUp.Core.Contracts.DataAccess;
using TruckCheckUp.Core.Contracts.InputValidation;
using TruckCheckUp.Core.Contracts.Logger;
using TruckCheckUp.Core.Contracts.Services;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.ViewModels.TruckUI;

namespace TruckCheckUp.Services
{
    public class TruckService : ITruckService
    {
        private IRepository<Truck> _truckContext;
        private IRepository<TruckModel> _truckModelContext;
        private IRepository<TruckManufacturer> _truckManufacturerContext;
        private IRepository<TruckYear> _truckYearContext;
        private ILogger _logger;
        private IValidateUserInput _validate;
        string tableNameUsedByLogger = "";

        public TruckService(IRepository<Truck> truckContext, IRepository<TruckManufacturer> truckManufacturerContext,
                            IRepository<TruckModel> truckModelContext,  IRepository<TruckYear> truckYearContext,
                            ILogger logger, IValidateUserInput validate)
        {
            _truckContext = truckContext;
            _truckManufacturerContext = truckManufacturerContext;
            _truckModelContext = truckModelContext;
            _truckYearContext = truckYearContext;
            _logger = logger;
            _validate = validate;
            tableNameUsedByLogger = "Truck";
        }

        public List<TruckViewModel> RetrieveAllTrucks()
        {
            //Get a list of Trucks
            var trucks = _truckContext.Collection().ToList();

            var trucksViewModel = from t in trucks
                                  orderby t.TruckNumber
                                  select new TruckViewModel
                                  {
                                      Id = t.Id,
                                      TruckNumber = t.TruckNumber,
                                      VIN = t.VIN,
                                      Manufacturer = t.TruckManufacturer.ManufacturerDescription,
                                      Model = t.TruckModel.ModelDescription,
                                      Year = t.TruckYear.ModelYear,
                                      Status = t.Status,
                                      StatusLabel = t.MessageBasedOnStatusSelection
                                  };
            return trucksViewModel.ToList();
        }

        public TruckSaveUpdateViewModel RetrieveModelAndYearListsFromDB(string Id)
        {
            var truckViewModel = new TruckSaveUpdateViewModel();

            truckViewModel.ModelDropDownList = RetrieveModelsFromDatabaseBasedOnManufacturerID(Id);
            truckViewModel.YearDropDownList = RetrieveYearsFromDatabase();

            return truckViewModel;
        }

        public TruckSaveUpdateViewModel RetrieveTruckById(string Id)
        {
            var truckRetrievedFromDB = _truckContext.Find(Id);

            var truckViewModel = new TruckSaveUpdateViewModel();

            truckViewModel.Id = truckRetrievedFromDB.Id;
            truckViewModel.VIN = truckRetrievedFromDB.VIN;
            truckViewModel.TruckNumber = truckRetrievedFromDB.TruckNumber.ToString();
            truckViewModel.Manufacturer = truckRetrievedFromDB.TruckManufacturerId;
            truckViewModel.Model = truckRetrievedFromDB.TruckModelId;
            truckViewModel.Year = truckRetrievedFromDB.TruckYearId;
            truckViewModel.Status = truckRetrievedFromDB.Status;
            truckViewModel.StatusLabel = truckRetrievedFromDB.MessageBasedOnStatusSelection;
            truckViewModel.ManufacturerDropDownList = RetrieveManufacturersFromDatabase();
            truckViewModel.ModelDropDownList = RetrieveModelsFromDatabase();
            truckViewModel.YearDropDownList = RetrieveYearsFromDatabase();

            return truckViewModel;
        }

        public TruckSaveUpdateViewModel RetrieveAllTruckManufacturers()
        {
            //Get a list of Manufacturers
            var trucks = new TruckSaveUpdateViewModel();
            trucks.ManufacturerDropDownList = RetrieveManufacturersFromDatabase();

            return trucks;
        }

        public List<ManufacturerDropDownListViewModel> RetrieveManufacturersFromDatabase()
        {
            var manufacturersRetrieved = _truckManufacturerContext.Collection().OrderBy(m => m.ManufacturerDescription).ToList();

            var manufacturersList = manufacturersRetrieved.Select(manufacturer => new ManufacturerDropDownListViewModel
            {
                Id = manufacturer.Id,
                Manufacturer = manufacturer.ManufacturerDescription
            }).ToList();

            return manufacturersList;
        }

        public List<ModelDropDownListViewModel> RetrieveModelsFromDatabaseBasedOnManufacturerID(string Id)
        {
            List<ModelDropDownListViewModel> modelsList = new List<ModelDropDownListViewModel>();
            var modelsRetrieved = _truckModelContext.Collection().Where(m => m.TruckManufacturerId == Id).OrderBy(m => m.ModelDescription).ToList();
            if (modelsRetrieved != null)
            {
                modelsList = MoveListOfModelsToModelDropDownListViewModel(modelsRetrieved);
            }
            return modelsList;
        }


        public List<ModelDropDownListViewModel> RetrieveModelsFromDatabase()
        {
            List<ModelDropDownListViewModel> modelsList = new List<ModelDropDownListViewModel>();
            var modelsRetrieved = _truckModelContext.Collection().OrderBy(m => m.ModelDescription).ToList();
            if (modelsRetrieved != null)
            {
                modelsList = MoveListOfModelsToModelDropDownListViewModel(modelsRetrieved);
            }
            return modelsList;
        }

        public List<ModelDropDownListViewModel> MoveListOfModelsToModelDropDownListViewModel(List<TruckModel> modelsFromQueryResult)
        {
            var orderedListOfModels = modelsFromQueryResult.Select(model => new ModelDropDownListViewModel
            {
                Id = model.Id,
                Model = model.ModelDescription
            }).ToList();

            return orderedListOfModels;
        }

        public List<YearDropDownListViewModel> RetrieveYearsFromDatabase()
        {
            var yearsRetrieved = _truckYearContext.Collection().OrderBy(m => m.ModelYear).ToList();

            var yearsList = yearsRetrieved.Select(year => new YearDropDownListViewModel
            {
                Id = year.Id,
                Year = year.ModelYear
            }).ToList();

            return yearsList;
        }

        public TruckSaveUpdateViewModel AddTruck(TruckSaveUpdateViewModel truck)
        {
            //Validate truck number contains only digits and VIN only alphanumeric
            if (!_validate.Numeric(truck.TruckNumber))
            {
                truck.TruckNumberIsValid = false;
            }
            else
             if (!_validate.Alphanumeric(truck.VIN))
            {
                truck.VinNumberIsValid = false;
            }
            
            if (truck.TruckNumberIsValid == true && truck.VinNumberIsValid == true)
            {
                //Verify whether the truck number is already in DB
                int truckNumberConvertedToInt = 0;
                Int32.TryParse(truck.TruckNumber, out truckNumberConvertedToInt);
                truck.ExistInDB = RetrieveTruckNumber(truckNumberConvertedToInt);
                if (!truck.ExistInDB)
                {
                    PostNewTruckModelToDB(truck);
                }
            }
            return truck;
        }

        public TruckSaveUpdateViewModel UpdateTruck(TruckSaveUpdateViewModel truck)
        {
            //Validate truck number contains only digits and VIN only alphanumeric
            if (!_validate.Numeric(truck.TruckNumber))
            {
                truck.TruckNumberIsValid = false;
            }
            else
             if (!_validate.Alphanumeric(truck.VIN))
            {
                truck.VinNumberIsValid = false;
            }

            if (truck.TruckNumberIsValid == true && truck.VinNumberIsValid == true)
            {               
                UpdateTruckData(truck);
            }
            return truck;
        }

        public bool RetrieveTruckNumber(int truckNumber)
        {
            //Check whether truck number already in DB
            var truckRetrieved = _truckContext.Collection().Any(t => t.TruckNumber == truckNumber);
            return truckRetrieved;
        }

        public void PostNewTruckModelToDB(TruckSaveUpdateViewModel truck)
        {
            var truckToInsert = new Truck();

            int truckNumberConvertedToInt = 0;
            Int32.TryParse(truck.TruckNumber, out truckNumberConvertedToInt);

            truckToInsert.TruckNumber = truckNumberConvertedToInt;
            truckToInsert.VIN = truck.VIN;
            truckToInsert.TruckManufacturerId = truck.Manufacturer;
            truckToInsert.TruckModelId = truck.Model;
            truckToInsert.TruckYearId = truck.Year;
            truckToInsert.Status = truck.Status;

            _truckContext.Insert(truckToInsert);
            _truckContext.Commit();
            _logger.Info("Inserted record Id " + truckToInsert.Id + " into Table " + tableNameUsedByLogger);

    }

        public void UpdateTruckData(TruckSaveUpdateViewModel truck)
        {
            var truckToUpdate = _truckContext.Find(truck.Id);
            if (truckToUpdate != null)
            {
                _logger.Info("Found record Id " + truckToUpdate.Id + " in Table " + tableNameUsedByLogger);
                truckToUpdate.VIN = truck.VIN;
                int truckNumberConvertedToInt = 0;
                Int32.TryParse(truck.TruckNumber, out truckNumberConvertedToInt);
                truckToUpdate.TruckNumber = truckNumberConvertedToInt;
                truckToUpdate.Status = truck.Status;
                truckToUpdate.TruckManufacturerId = truck.Manufacturer;
                truckToUpdate.TruckModelId = truck.Model;
                truckToUpdate.TruckYearId = truck.Year;

                _truckContext.Commit();
                _logger.Info("Updated record Id " + truckToUpdate.Id + " in Table " + tableNameUsedByLogger);

            }
        }

        public TruckSaveUpdateViewModel SearchTruck(TruckSaveUpdateViewModel truck)
        {
            var truckSearchResult = new Truck();
            var truckViewModel = new TruckViewModel();
            if (!string.IsNullOrEmpty(truck.TruckNumber))
            {
                if (!_validate.Numeric(truck.TruckNumber))
                {
                    truck.TruckNumberIsValid = false;
                }
                else
                {
                    int truckNumberConvertedToInt = 0;
                    Int32.TryParse(truck.TruckNumber, out truckNumberConvertedToInt);
                    truckSearchResult = _truckContext.Collection().Where(t => t.TruckNumber == truckNumberConvertedToInt).FirstOrDefault();
                    if (truckSearchResult == null)
                    {
                        truck.ExistInDB = false;
                    }
                    else
                    {
                        truck.Id = truckSearchResult.Id;
                        truck.TruckNumber = truckSearchResult.TruckNumber.ToString();
                        truck.VIN = truckSearchResult.VIN;
                        truck.Manufacturer = truckSearchResult.TruckManufacturer.ManufacturerDescription;
                        truck.Model = truckSearchResult.TruckModel.ModelDescription;
                        truck.Year = truckSearchResult.TruckYear.ModelYear.ToString();
                        truck.Status = truckSearchResult.Status;
                        truck.StatusLabel = truckSearchResult.MessageBasedOnStatusSelection;
                    }
                }
            }
            return truck;
        }

        public void DeleteTruck(string truckId)
        {
            var truckToDelete = _truckContext.Find(truckId);
            if (truckToDelete != null)
            {
                _logger.Info("Found record Id " + truckToDelete.Id + " in Table " + tableNameUsedByLogger);
                _truckContext.Delete(truckToDelete);
                _truckContext.Commit();
                _logger.Info("Deleted record Id " + truckToDelete.Id + " from Table " + tableNameUsedByLogger);
            }



            //Old methods using Razor
            //public void PostNewTruckToDB(TruckInsertViewModel truck)
            //{
            //    var truckToInsert = new Truck();
            //    truckToInsert.VIN          = truck.VIN;
            //    truckToInsert.TruckNumber  = truck.TruckNumber;
            //    truckToInsert.Manufacturer = truck.Manufacturer;
            //    truckToInsert.Model        = truck.Model;
            //    truckToInsert.Year         = truck.Year;
            //    truckToInsert.Status       = truck.Status;

            //    _truckContext.Insert(truckToInsert);
            //    _truckContext.Commit();
            //}

            //public TruckUpdateViewModel RetrieveTruckDataToUpdate(string truckId)
            //{
            //    //Look for truck and return her data
            //    var truckToUpdate = (
            //            from truckStoredInDB in _truckContext.Collection()
            //            where truckStoredInDB.Id == truckId
            //            select new TruckUpdateViewModel()
            //            {
            //                Id           = truckStoredInDB.Id,
            //                VIN          = truckStoredInDB.VIN,
            //                TruckNumber  = truckStoredInDB.TruckNumber,
            //                Manufacturer = truckStoredInDB.Manufacturer,
            //                Model        = truckStoredInDB.Model,
            //                Year         = truckStoredInDB.Year,
            //                Status       = truckStoredInDB.Status
            //            }).FirstOrDefault();

            //    if (truckToUpdate != null)
            //    {
            //        return truckToUpdate;
            //    }
            //    else
            //    {
            //        return new TruckUpdateViewModel();
            //    }
            //}

            //public void UpdateTruckData(TruckUpdateViewModel truck, string truckId)
            //{
            //    var truckToUpdate = _truckContext.Find(truckId);
            //    if (truckToUpdate != null)
            //    {
            //        truckToUpdate.VIN          = truck.VIN;
            //        truckToUpdate.TruckNumber  = truck.TruckNumber;
            //        truckToUpdate.Manufacturer = truck.Manufacturer;
            //        truckToUpdate.Model        = truck.Model;
            //        truckToUpdate.Year         = truck.Year;
            //        truckToUpdate.Status       = truck.Status;

            //        _truckContext.Commit();
            //    }
            //}

            //public Truck RetrieveTruckToDelete(string truckId)
            //{
            //    var truckToDelete = _truckContext.Find(truckId);
            //    if (truckToDelete != null)
            //    {
            //        return truckToDelete;
            //    }
            //    else
            //    {
            //        return new Truck();
            //    }
            //}

            //public void DeleteTruck(string truckId)
            //{
            //    var truckToDelete = _truckContext.Find(truckId);
            //    if (truckToDelete != null)
            //    {
            //        _truckContext.Delete(truckToDelete);
            //        _truckContext.Commit();
            //    }
            //}

        }
    }
}
