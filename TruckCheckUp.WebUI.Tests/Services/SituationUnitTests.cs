using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.Contracts.DataAccess;
using TruckCheckUp.WebUI.Tests.Mocks;
using TruckCheckUp.Core.Contracts.Logger;
using TruckCheckUp.Core.Contracts.Services;
using TruckCheckUp.Services;
using TruckCheckUp.Core.Contracts.InputValidation;
using TruckCheckUp.Core.ViewModels.SituationUI;
using System.Collections.Generic;

namespace TruckCheckUp.WebUI.Tests.Services
{
    [TestClass]
    public class SituationUnitTests
    {
        [TestMethod]
        public void Test_AddSituationWhenSituationDoesNOTExistInDatabaseAndContainsOnlyLetters_SituationAddedToDatabase()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            //Populate table, so we can test whether service is adding records.
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });
            //Act
            //Add a third record to test 
            _situationService.EvaluateSituationDescriptionBeforeAdding(new SituationViewModel() { Id = "3", Description = "Closed", Status = true, ExistInDB = false, IsValid = true });
            var result = _situationService.RetrieveAllSituations();
            //Assert
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void Test_AddSituationWhenExactMatchSituationEXISTInDatabaseAndContainsOnlyLetters_SituationNotAddedToDatabase()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            //Populate table, so we can test whether service is adding records.
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });
            //Act
            //Add a third record to test 
            _situationService.EvaluateSituationDescriptionBeforeAdding(new SituationViewModel() { Id = "3", Description = "Open", Status = true, ExistInDB = false, IsValid = true });
            var result = _situationService.RetrieveAllSituations();
            //Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void Test_AddSituationDescriptionInLowerCaseLettersWhenSituationEXISTInDatabaseAndContainsOnlyLetters_SituationNotAddedToDatabase()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            //Populate table, so we can test whether service is adding records.
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });
            //Act
            //Add a third record to test 
            _situationService.EvaluateSituationDescriptionBeforeAdding(new SituationViewModel() { Id = "3", Description = "open", Status = true, ExistInDB = false, IsValid = true });
            var result = _situationService.RetrieveAllSituations();
            //Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void Test_AddSituationDescriptionWhenSituationDoesNotExistInDatabaseAndDoesNotContainsOnlyLetters_SituationNotAddedToDatabase()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            //Populate table, so we can test whether service is adding records.
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });
            //Act
            //Add a third record to test 
            _situationService.EvaluateSituationDescriptionBeforeAdding(new SituationViewModel() { Id = "3", Description = "open88", Status = true, ExistInDB = false, IsValid = true });
            var result = _situationService.RetrieveAllSituations();
            //Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void Test_DeleteSituationDescriptionWhenSituationExist_SituationDescriptionDeletedFromDatabase()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            //Populate table, so we can test whether service is adding records.
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });

            //Act
            _situationService.DeleteSituation("2");
            var result = _situationService.RetrieveAllSituations();
            //Assert
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void Test_RetrieveSituationRecordByIdWhenSearchingForExistingId_SituationDescriptionRetrievedFromDatabase()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            //Populate table, so we can test whether the method is returning the right record
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });
            //Act
            var result= _situationService.RetrieveSituationById("1");
            //Assert
            Assert.AreEqual("InProgress", result.Description);
        }

        [TestMethod]
        public void Test_RetrieveSituationRecordByIdWhenSearchingForNonExistentId_ReturnsNullSituationViewModelObject()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            //Populate table, so we can test whether the method is returning the right record
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });
            //Act
            var result = _situationService.RetrieveSituationById("3");
            //Assert
            Assert.AreEqual(null, result.Description);
        }

        [TestMethod]
        public void Test_SearchSituationWhenExactMatchSituationEXISTInDatabaseAndContainsOnlyLetters_RetrieveSituationRequested()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            IValidateUserInput _validate = new ValidateUserInput();
            ILogger _logger = new MockTruckCheckUpLogger();
            ISituationService _situationService = new SituationService(_situationContext, _logger, _validate);
            //Populate table, so we can test whether there is data available
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });
            //Act
            var result = _situationService.EvaluateSituationDescriptionBeforeSearching(new SituationViewModel() { Id ="1", Description ="InProgress", Status = true, ExistInDB = true, IsValid = true });

            //Assert
            Assert.AreEqual("InProgress", result.Description);
        }

        [TestMethod]
        public void Test_SearchSituationWhenSituationIsEnteredInLowerCaseLettersAndSituationEXISTInDatabase_RetrieveSituationRequested()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            IValidateUserInput _validate = new ValidateUserInput();
            ILogger _logger = new MockTruckCheckUpLogger();
            ISituationService _situationService = new SituationService(_situationContext, _logger, _validate);
            //Populate table, so we can test whether there is data available
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });
            //Act
            var result = _situationService.EvaluateSituationDescriptionBeforeSearching(new SituationViewModel() { Id = "1", Description = "inprogress", Status = true, ExistInDB = true, IsValid = true });

            //Assert
            Assert.AreEqual("InProgress", result.Description);
        }

        [TestMethod]
        public void Test_SearchSituationWhenSituationDoesNOTExistInDatabase_DescriptionPropertyReturnsNull()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            IValidateUserInput _validate = new ValidateUserInput();
            ILogger _logger = new MockTruckCheckUpLogger();
            ISituationService _situationService = new SituationService(_situationContext, _logger, _validate);
            //Populate table, so we can test whether there is data available
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });
            //Act
            var result = _situationService.EvaluateSituationDescriptionBeforeSearching(new SituationViewModel() { Id = "1", Description = "Closed", Status = true, ExistInDB = true, IsValid = true });

            //Assert
            Assert.AreEqual(null, result.Description);
        }

        [TestMethod]
        public void Test_SearchSituationWhenSituationDoesNOTExistInDatabase_ExistInDBPropertyReturnsFalse()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            IValidateUserInput _validate = new ValidateUserInput();
            ILogger _logger = new MockTruckCheckUpLogger();
            ISituationService _situationService = new SituationService(_situationContext, _logger, _validate);
            //Populate table, so we can test whether there is data available
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });
            //Act
            var result = _situationService.EvaluateSituationDescriptionBeforeSearching(new SituationViewModel() { Id = "1", Description = "Closed", Status = true, ExistInDB = true, IsValid = true });

            //Assert
            Assert.AreEqual(false, result.ExistInDB); 
        }

        [TestMethod]
        public void Test_SearchSituationWhenSituationEXISTInDatabase_ExistInDBPropertyReturnsTrue()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            IValidateUserInput _validate = new ValidateUserInput();
            ILogger _logger = new MockTruckCheckUpLogger();
            ISituationService _situationService = new SituationService(_situationContext, _logger, _validate);
            //Populate table, so we can test whether there is data available
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });
            //Act
            var result = _situationService.EvaluateSituationDescriptionBeforeSearching(new SituationViewModel() { Id = "1", Description = "InProgress", Status = true, ExistInDB = true, IsValid = true });

            //Assert
            Assert.AreEqual(true, result.ExistInDB);
        }

        [TestMethod]
        public void Test_SearchSituationWhenSituationEXISTInDatabaseAndSituationContainsAlphanumericCharacters_IsValidPropertyReturnsFalse()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            IValidateUserInput _validate = new ValidateUserInput();
            ILogger _logger = new MockTruckCheckUpLogger();
            ISituationService _situationService = new SituationService(_situationContext, _logger, _validate);
            //Populate table, so we can test whether there is data available
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });
            //Act
            var result = _situationService.EvaluateSituationDescriptionBeforeSearching(new SituationViewModel() { Id = "1", Description = "InProgress88", Status = true, ExistInDB = true, IsValid = true });

            //Assert
            Assert.AreEqual(false, result.IsValid);
        }

        [TestMethod]
        public void Test_UpdateSituationWhenSituationEnteredDoesNotExistInDatabaseAndContainsOnlyLetters_SituationDescriptionUpdated()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            IValidateUserInput _validate = new ValidateUserInput();
            ILogger _logger = new MockTruckCheckUpLogger();
            ISituationService _situationService = new SituationService(_situationContext, _logger, _validate);
            //Populate table, so we can test whether there is data available
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });
            //Act
            _situationService.EvaluateSituationDescriptionBeforeUpdating(new SituationViewModel() { Id = "1", Description = "Closed", Status = true, ExistInDB = true, IsValid = true });
            var result = _situationService.EvaluateSituationDescriptionBeforeSearching(new SituationViewModel() { Id = "1", Description = "Closed", Status = true, ExistInDB = true, IsValid = true });
            //Assert
            Assert.AreEqual("Closed", result.Description);
        }

        [TestMethod]
        public void Test_UpdateSituationWhenSituationEnteredExistInDatabaseAndContainsOnlyLetters_SituationDescriptionNOTUpdated()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            IValidateUserInput _validate = new ValidateUserInput();
            ILogger _logger = new MockTruckCheckUpLogger();
            ISituationService _situationService = new SituationService(_situationContext, _logger, _validate);
            //Populate table, so we can test whether there is data available
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });
            //Act
            _situationService.EvaluateSituationDescriptionBeforeUpdating(new SituationViewModel() { Id = "1", Description = "Open", Status = true, ExistInDB = true, IsValid = true });
            var result = _situationService.RetrieveSituationById("1");
            //Assert
            Assert.AreEqual("InProgress", result.Description);
        }

        [TestMethod]
        public void Test_UpdateSituationWhenSituationEnteredExistInDatabaseAndContainsOnlyLetters_ExistInDBPropertyReturnsTrue()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            IValidateUserInput _validate = new ValidateUserInput();
            ILogger _logger = new MockTruckCheckUpLogger();
            ISituationService _situationService = new SituationService(_situationContext, _logger, _validate);
            //Populate table, so we can test whether there is data available
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });
            //Act
            var result = _situationService.EvaluateSituationDescriptionBeforeUpdating(new SituationViewModel() { Id = "1", Description = "Open", Status = true, ExistInDB = true, IsValid = true });
            
            //Assert
            Assert.AreEqual(true, result.ExistInDB);
        }

        [TestMethod]
        public void Test_UpdateSituationWhenSituationEnteredExistInDatabaseAndDoesNOTContainsOnlyLetters_IsValidPropertyReturnsFalse()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            IValidateUserInput _validate = new ValidateUserInput();
            ILogger _logger = new MockTruckCheckUpLogger();
            ISituationService _situationService = new SituationService(_situationContext, _logger, _validate);
            //Populate table, so we can test whether there is data available
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });
            //Act
            var result = _situationService.EvaluateSituationDescriptionBeforeUpdating(new SituationViewModel() { Id = "1", Description = "Open88", Status = true, ExistInDB = true, IsValid = true });

            //Assert
            Assert.AreEqual(false, result.IsValid);
        }

        [TestMethod]
        public void Test_UpdateSituationWhenSituationEnteredExistInDatabaseAndDoesNOTContainsOnlyLetters_SituationDescriptionNOTUpdated()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            IValidateUserInput _validate = new ValidateUserInput();
            ILogger _logger = new MockTruckCheckUpLogger();
            ISituationService _situationService = new SituationService(_situationContext, _logger, _validate);
            //Populate table, so we can test whether there is data available
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });
            //Act
            _situationService.EvaluateSituationDescriptionBeforeUpdating(new SituationViewModel() { Id = "1", Description = "Open88", Status = true, ExistInDB = true, IsValid = true });
            var result = _situationService.RetrieveSituationById("1");
            //Assert
            Assert.AreEqual("InProgress", result.Description);
        }

        [TestMethod]
        public void Test_RetrieveAllSituationsFromDatabase_AllSituationsRetrieved()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            IValidateUserInput _validate = new ValidateUserInput();
            ILogger _logger = new MockTruckCheckUpLogger();
            ISituationService _situationService = new SituationService(_situationContext, _logger, _validate);
            //Populate table, so we can test whether there is data available
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true});

            //Act
            var result = _situationService.RetrieveAllSituations();

            //Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void Test_RetrieveAllSituationsFromDatabase_ZeroSituationsRetrieved()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);

            //Act
            var result = _situationService.RetrieveAllSituations();

            //Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Test_RetrieveAllSituationsFromDatabase_SituationListViewModelObjectReturned()
        {
            //Arrange 
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            bool expected = true;
            bool actual = false;
           
            //Act
            var result = _situationService.RetrieveAllSituations();
            if (result is List<SituationListViewModel>)
            {
                actual = true;
            }

            //Assert
            Assert.AreEqual(expected, actual);
        }

    }
}
