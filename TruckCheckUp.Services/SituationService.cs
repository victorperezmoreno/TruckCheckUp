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

        public SituationViewModel EvaluateSituationDescriptionBeforeAdding(SituationViewModel situationObject)
        {
            if (string.IsNullOrEmpty(situationObject.Description) == false)
            {
                //Validate that situation description field contains only letters
                bool descriptionHasOnlyLetters = _validate.OnlyLetters(situationObject.Description);
                if (descriptionHasOnlyLetters == true)
                {
                    bool situationExistInDatabase = SituationAlreadyExistInDatabase(situationObject.Description);
                    //Validate that description is not in database already
                    if (situationExistInDatabase == false)
                    {
                        PostNewSituationToDB(situationObject);
                    }

                    return new SituationViewModel(situationObject.Id, situationObject.Description,
                        situationObject.Status, situationExistInDatabase, descriptionHasOnlyLetters);
                }
            }
            //At this point Situation Description does not contains only letters thus return IsValid as false
            return new SituationViewModel(situationObject.Id, situationObject.Description,
                    situationObject.Status, false, false);
        }

        private bool SituationAlreadyExistInDatabase(string situationDescription)
        {
            //Check whether situation description already in DB
            return _situationContext.Collection().Any(d => d.Description.Equals(situationDescription.Trim(), StringComparison.OrdinalIgnoreCase));
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

        public SituationViewModel EvaluateSituationDescriptionBeforeUpdating(SituationViewModel situationObject)
        {
            bool situationExistInDB;
            bool situationIsValid;
            if (string.IsNullOrEmpty(situationObject.Description) == false)
            {
                situationIsValid = _validate.OnlyLetters(situationObject.Description);
                if (situationIsValid == true)
                {
                    situationExistInDB = SituationAlreadyExistInDatabase(situationObject.Description);
                    if (situationExistInDB == false)
                    {
                        UpdateSituationData(situationObject);
                    }
                    return new SituationViewModel(situationObject.Id, situationObject.Description,
                        situationObject.Status, situationExistInDB, situationIsValid);
                }  
            }
            //At this point Situation Description is null or empty so return IsValid as false
            return new SituationViewModel(situationObject.Id, situationObject.Description,
                situationObject.Status, false, false);
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

        public SituationViewModel EvaluateSituationDescriptionBeforeSearching(SituationViewModel situationToEvaluate)
        {
            if (string.IsNullOrEmpty(situationToEvaluate.Description) == false)
            {
                return SearchSituation(situationToEvaluate);
            }
            else
            {
                return new SituationViewModel(situationToEvaluate.Id, situationToEvaluate.Description,
                    situationToEvaluate.Status, false, false);
            }

        }

        private SituationViewModel SearchSituation(SituationViewModel situationObject)
        {
            var situationDescriptionIsValid = _validate.OnlyLetters(situationObject.Description) == true;
            var newSituationObject = new SituationViewModel();
            if (situationDescriptionIsValid == true)
            {
                var situationSearchResult = _situationContext.Collection().Where(s => s.Description.Equals(situationObject.Description, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                if (situationSearchResult != null)
                {
                    newSituationObject.Id = situationSearchResult.Id;
                    newSituationObject.Description = situationSearchResult.Description;
                    newSituationObject.Status = situationSearchResult.Status;
                    newSituationObject.ExistInDB = true;
                }
                else
                {
                    newSituationObject.ExistInDB = false;
                } 
            }

            newSituationObject.IsValid = situationDescriptionIsValid;

            return newSituationObject;
        }

        public void DeleteSituation(string situationId)
        {
            if (string.IsNullOrEmpty(situationId) == false)
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
}
