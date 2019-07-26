using System.Collections.Generic;
using System.Linq;
using TruckCheckUp.Core.Contracts.DataAccess;
using TruckCheckUp.Core.Contracts.InputValidation;
using TruckCheckUp.Core.Contracts.Logger;
using TruckCheckUp.Core.Contracts.Services;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.ViewModels.TruckModelUI;

namespace TruckCheckUp.Services
{
    public class TruckModelService : ITruckModelService
    {
        private IRepository<TruckModel> _truckModelContext;
        private IRepository<TruckManufacturer> _truckManufacturerContext;
        private ILogger _logger;
        private IValidateUserInput _validate;
        private string tableNameUsedByLogger = "TruckModel";
        public TruckModelService(IRepository<TruckModel> truckModelContext,
                                 IRepository<TruckManufacturer> truckManufacturerContext,
                                 ILogger logger, IValidateUserInput validate)
        {
            _truckModelContext = truckModelContext;
            _truckManufacturerContext = truckManufacturerContext;
            _logger = logger;
            _validate = validate;
        }

        public List<TruckModelViewModel> RetrieveAllTruckModels()
        {
            //Get a list of Models and Manufacturers 
            var models = _truckModelContext.Collection().ToList();

            var modelsViewModel = from m in models
                                 select new TruckModelViewModel
                                 {
                                     Id = m.Id,
                                     Description = m.ModelDescription,
                                     ManufacturerDescription = m.TruckManufacturer.ManufacturerDescription
                                 };
            return modelsViewModel.ToList();
        }

        public TruckModelSaveUpdateViewModel RetrieveAllTruckManufacturers()
        {
            //Get a list of Manufacturers
            var truckModel = new TruckModelSaveUpdateViewModel();
            truckModel.ManufacturerDropDownList = RetrieveManufacturersFromDatabase();

            return truckModel;
        }

        public TruckModelSaveUpdateViewModel RetrieveModelById(string Id)
        {
            var modelRetrievedFromDB = _truckModelContext.Find(Id);
           
            var modelViewModel = new TruckModelSaveUpdateViewModel();

            modelViewModel.Id = modelRetrievedFromDB.Id;
            modelViewModel.Description = modelRetrievedFromDB.ModelDescription;
            modelViewModel.ManufacturerId = modelRetrievedFromDB.TruckManufacturerId;
            modelViewModel.ManufacturerDropDownList = RetrieveManufacturersFromDatabase();

            return modelViewModel;
        }

        public List<TruckManufacturerDropDownListViewModel> RetrieveManufacturersFromDatabase()
        {
            var manufacturersRetrieved = _truckManufacturerContext.Collection().OrderBy(m => m.ManufacturerDescription).ToList();

            var manufacturersList = manufacturersRetrieved.Select(manufacturer => new TruckManufacturerDropDownListViewModel
            {
                Id = manufacturer.Id,
                Description = manufacturer.ManufacturerDescription


            }).ToList();

            return manufacturersList;
        }

        public TruckModelSaveUpdateViewModel AddTruckModel(TruckModelSaveUpdateViewModel truckModel)
        {
            if (!string.IsNullOrEmpty(truckModel.Description))
            {

                //Verify that only letters and numbers in string model entered by user
                if (!_validate.Alphanumeric(truckModel.Description))
                {
                    truckModel.IsValid = false;
                }
                else
                {
                    //Verify whether the model is already in DB
                    truckModel.ExistInDB = RetrieveTruckModelName(truckModel.Description);
                    if (!truckModel.ExistInDB)
                    {
                        PostNewTruckModelToDB(truckModel);
                    }
                }
            }
            return truckModel;
        }

        public TruckModelSaveUpdateViewModel UpdateTruckModel(TruckModelSaveUpdateViewModel truckModel)
        {
            if (!string.IsNullOrEmpty(truckModel.Description))
            {

                //Verify that only letters and numbers in string model entered by user
                if (!_validate.Alphanumeric(truckModel.Description))
                {
                    truckModel.IsValid = false;
                }
                else
                {
                    //Verify whether the model is already in DB and save value 
                    //in object to return to View for validation purposes 
                    truckModel.ExistInDB = RetrieveTruckModelName(truckModel.Description);

                    if (!truckModel.ExistInDB)
                    {
                        UpdateTruckModelData(truckModel);
                    }
                }
            }
            return truckModel;
        }

        public TruckModelSaveUpdateViewModel SearchTruckModel(TruckModelSaveUpdateViewModel model)
        {
            var modelSearchResult = new TruckModel();
            if (!string.IsNullOrEmpty(model.Description))
            {
                //Search for model in DB only if numbers and letters in model name
                if (!_validate.Alphanumeric(model.Description))
                {
                    model.IsValid = false;
                }
                else
                {
                    modelSearchResult = _truckModelContext.Collection().Where(m => m.ModelDescription == model.Description).FirstOrDefault();
                    if (modelSearchResult == null)
                    {
                        model.ExistInDB = false;
                    }
                    else
                    {
                        model.Id = modelSearchResult.Id;
                        model.Description = modelSearchResult.ModelDescription;
                        model.ManufacturerId = modelSearchResult.TruckManufacturerId;
                        var manufacturer = _truckManufacturerContext.Collection().Where(m => m.Id == modelSearchResult.TruckManufacturerId).FirstOrDefault();
                        model.ManufacturerDescription = manufacturer.ManufacturerDescription;
                    }
                }
            }
            return model;
        }

        public bool RetrieveTruckModelName(string modelName)
        {
            //Check whether truck model already in DB
            var modelRetrieved = _truckModelContext.Collection().Any(model => model.ModelDescription == modelName);
            return modelRetrieved;
        }

        public void PostNewTruckModelToDB(TruckModelSaveUpdateViewModel truckModel)
        {
            var truckModelToInsert = new TruckModel();
            truckModelToInsert.ModelDescription = truckModel.Description;
            truckModelToInsert.TruckManufacturerId = truckModel.ManufacturerId;

            _truckModelContext.Insert(truckModelToInsert);
            _truckModelContext.Commit();
            _logger.Info("Inserted record Id " + truckModelToInsert.Id + " into Table " + tableNameUsedByLogger);

        }

        public void UpdateTruckModelData(TruckModelSaveUpdateViewModel truckModel)
        {
            var truckModelToUpdate = _truckModelContext.Find(truckModel.Id);
            if (truckModelToUpdate != null)
            {
                _logger.Info("Found record Id " + truckModelToUpdate.Id + " in Table " + tableNameUsedByLogger);
                truckModelToUpdate.ModelDescription = truckModel.Description;
                truckModelToUpdate.TruckManufacturerId = truckModel.ManufacturerId;
                _truckModelContext.Commit();
                _logger.Info("Updated record Id " + truckModelToUpdate.Id + " in Table " + tableNameUsedByLogger);

            }
        }

        public void DeleteTruckModel(string truckModelId)
        {
            var truckModelToDelete = _truckModelContext.Find(truckModelId);
            if (truckModelToDelete != null)
            {
                _logger.Info("Found record Id " + truckModelToDelete.Id + " in Table " + tableNameUsedByLogger);
                _truckModelContext.Delete(truckModelToDelete);
                _truckModelContext.Commit();
                _logger.Info("Deleted record Id " + truckModelToDelete.Id + " from Table " + tableNameUsedByLogger);
            }
        }
    }
}
