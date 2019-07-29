using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruckCheckUp.Core.Contracts.DataAccess;
using TruckCheckUp.Core.Contracts.InputValidation;
using TruckCheckUp.Core.Contracts.Logger;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.ViewModels.SituationUI;
using TruckCheckUp.Core.ViewModels.TruckUI;

namespace TruckCheckUp.Services
{
    public class SituationService
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

        public List<SituationListViewModel> RetrieveAllSituationsfromDatabase()
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

        public SituationListViewModel RetrieveSituationById(string Id)
        {
            var situationRetrievedFromDB = _situationContext.Find(Id);

            var situationViewModel = new SituationListViewModel();

            situationViewModel.Id = situationRetrievedFromDB.Id;
            situationViewModel.Description = situationRetrievedFromDB.Description;

            return situationViewModel;
        }

        public SituationViewModel AddSituation(SituationViewModel situationObject)
        {
            bool descriptionHasOnlyLetters = _validate.OnlyLetters(situationObject.Situation);
            bool situationAlreadyExistInDatabase = SituationDescriptionAlreadyExistInDatabase(situationObject.Situation);
            //Validate that situation description field contains only letters and is not in database already
            if (descriptionHasOnlyLetters == true && situationAlreadyExistInDatabase == false)
            {
                PostNewSituationToDB(situationObject);
            }

            situationObject.ExistInDB = situationAlreadyExistInDatabase;
            situationObject.IsValid = descriptionHasOnlyLetters;

            return situationObject;
        }

        public SituationViewModel UpdateTruck(SituationViewModel situationObject)
        {
            //Validate situation description contains only letters
            if (_validate.OnlyLetters(situationObject.Situation) == true)
            {
                UpdateSituationData(situationObject);
                situationObject.IsValid = true;
            }
            else
            {
                situationObject.IsValid = false;
            }

            return situationObject;
        }

        public bool SituationDescriptionAlreadyExistInDatabase(string situationDescription)
        {
            //Check whether situation description already in DB
            return _situationContext.Collection().Any(d => d.Description == situationDescription.Trim());
        }

        public void PostNewSituationToDB(SituationViewModel situationObject)
        {
            var situationToInsert = new Situation();

            situationToInsert.Description = situationObject.Situation;
            situationToInsert.Status = situationObject.Status;
            _situationContext.Insert(situationToInsert);
            _situationContext.Commit();
            _logger.Info("Inserted record Id " + situationToInsert.Id + " into Table " + tableNameUsedByLogger);

        }

        public void UpdateSituationData(SituationViewModel situationObject)
        {
            var situationToUpdate = _situationContext.Find(situationObject.Id);
            if (situationToUpdate != null)
            {
                _logger.Info("Found record Id " + situationToUpdate.Id + " in Table " + tableNameUsedByLogger);
                situationToUpdate.Description = situationObject.Situation;

                _situationContext.Commit();
                _logger.Info("Updated record Id " + situationToUpdate.Id + " in Table " + tableNameUsedByLogger);

            }
        }

        public SituationViewModel SearchSituationInDB(SituationViewModel situationObject)
        {
            var situationSearchResult = new Situation();
            var situationViewModel = new SituationViewModel();
            if (string.IsNullOrEmpty(situationObject.Situation) == false && _validate.OnlyLetters(situationObject.Situation) == true)
            {
                situationSearchResult = _situationContext.Collection().Where(s => s.Id == situationObject.Id).FirstOrDefault();
                if (situationSearchResult == null)
                {
                    situationObject.ExistInDB = false;
                }
                else
                {
                    situationObject.Id = situationSearchResult.Id;
                    situationObject.Situation = situationSearchResult.Description;
                }
                situationObject.IsValid = true;
            }
            else
            {
                situationObject.IsValid = false;
            }

            return situationObject;
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
