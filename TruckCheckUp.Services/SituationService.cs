using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruckCheckUp.Core.Contracts.DataAccess;
using TruckCheckUp.Core.Contracts.InputValidation;
using TruckCheckUp.Core.Contracts.Logger;
using TruckCheckUp.Core.Contracts.Services;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.ViewModels.SituationUI;
using TruckCheckUp.Core.ViewModels.TruckUI;

namespace TruckCheckUp.Services
{
    public class SituationService : ISituationService
    {
        private IRepository<Situation> _situationContext;
        private ILogger _logger;
        private IValidateUserInput _validate;
        string tableNameUsedByLogger = "Situation";

        public SituationService(IRepository<Situation> situationContext, ILogger logger, IValidateUserInput validate)
        {
            _situationContext = situationContext;
            _logger = logger;
            _validate = validate;
        }

        public List<SituationListViewModel> RetrieveAllSituations()
        {
            //Get a list of Situations from DB
            return _situationContext.Collection().OrderByDescending(s => s.Description).Select(
                situation => new SituationListViewModel
                {
                    Id = situation.Id,
                    Description = situation.Description,
                    Status = situation.Status
                }).ToList();
        }

        public SituationViewModel RetrieveSituationById(string Id)
        {
            return MoveSituationDataToSituationViewModel(_situationContext.Find(Id));
        }

        private SituationViewModel MoveSituationDataToSituationViewModel(Situation situationRetrievedFromDB)
        {
            if (situationRetrievedFromDB != null)
            {
                return new SituationViewModel
                {
                    Id = situationRetrievedFromDB.Id,
                    Description = situationRetrievedFromDB.Description,
                    Status = situationRetrievedFromDB.Status
                };
            }

            return new SituationViewModel();
        }

        public SituationViewModel AddSituation(SituationViewModel situationObject)
        {
            bool descriptionHasOnlyLetters = _validate.OnlyLetters(situationObject.Description);
            bool situationAlreadyExistInDatabase = SituationDescriptionAlreadyExistInDatabase(situationObject.Description);
            //Validate that situation description field contains only letters and is not in database already
            if (descriptionHasOnlyLetters == true && situationAlreadyExistInDatabase == false)
            {
                PostNewSituationToDB(situationObject);
            }

            situationObject.ExistInDB = situationAlreadyExistInDatabase;
            situationObject.IsValid = descriptionHasOnlyLetters;

            return situationObject;
        }

        private bool SituationDescriptionAlreadyExistInDatabase(string situationDescription)
        {
            //Check whether situation description already in DB
            return _situationContext.Collection().Any(d => d.Description == situationDescription.Trim());
        }

        private void PostNewSituationToDB(SituationViewModel situationObject)
        {
            var situationToInsert = new Situation();

            situationToInsert.Description = situationObject.Description;
            situationToInsert.Status = situationObject.Status;
            _situationContext.Insert(situationToInsert);
            _situationContext.Commit();
            _logger.Info("Inserted record Id " + situationToInsert.Id + " into Table " + tableNameUsedByLogger);

        }

        public SituationViewModel UpdateSituation(SituationViewModel situationObject)
        {
            Boolean situationContainsOnlyLetters = _validate.OnlyLetters(situationObject.Description);
            //Validate situation description contains only letters
            if (situationContainsOnlyLetters == true)
            {
                UpdateSituationData(situationObject);
            }

            situationObject.IsValid = situationContainsOnlyLetters;
            return situationObject;
        }

        private void UpdateSituationData(SituationViewModel situationObject)
        {
            var situationToUpdate = _situationContext.Find(situationObject.Id);
            if (situationToUpdate != null)
            {
                _logger.Info("Found record Id " + situationToUpdate.Id + " in Table " + tableNameUsedByLogger);
                situationToUpdate.Description = situationObject.Description;
                situationToUpdate.Status = situationObject.Status;

                _situationContext.Commit();
                _logger.Info("Updated record Id " + situationToUpdate.Id + " in Table " + tableNameUsedByLogger);
            }
        }

        public SituationViewModel SearchSituation(SituationViewModel situationObject)
        {
            var situationDescriptionIsValid = _validate.OnlyLetters(situationObject.Description) == true && string.IsNullOrEmpty(situationObject.Description) == false;
            var situationViewModel = new SituationViewModel();
            if (situationDescriptionIsValid == true)
            {
                var situationSearchResult = _situationContext.Collection().Where(s => s.Description.Equals(situationObject.Description, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                if (situationSearchResult != null)
                {
                    situationViewModel.Id = situationSearchResult.Id;
                    situationViewModel.Description = situationSearchResult.Description;
                    situationViewModel.Status = situationSearchResult.Status;
                    situationViewModel.ExistInDB = true;
                }
                else
                {
                    situationViewModel.ExistInDB = false;
                } 
            }
            
            situationViewModel.IsValid = situationDescriptionIsValid;

            return situationViewModel;
        }

        public void DeleteSituation(string situationId)
        {
            var situationToDelete = _situationContext.Find(situationId);
            if (situationToDelete != null)
            {
                _logger.Info("Found record Id " + situationToDelete.Id + " in Table " + tableNameUsedByLogger);
                _situationContext.Delete(situationToDelete);
                _situationContext.Commit();
                _logger.Info("Deleted record Id " + situationToDelete.Id + " from Table " + tableNameUsedByLogger);
            }
        }

    }
}
