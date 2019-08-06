using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TruckCheckUp.WebUI.Controllers;
using TruckCheckUp.Core.Contracts.Services;
using TruckCheckUp.Services;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.Contracts.DataAccess;
using TruckCheckUp.WebUI.Tests.Mocks;
using TruckCheckUp.Core.Contracts.Logger;
using TruckCheckUp.Core.Contracts.InputValidation;
using System.Web.Mvc;
using TruckCheckUp.Core.ViewModels.SituationUI;

namespace TruckCheckUp.WebUI.Tests.Controllers
{
    [TestClass]
    public class SituationControllerTest
    {
        [TestMethod]
        public void Test_Index_IndexActionReturnsIndexView()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();

            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            var situationController = new SituationManagementController(_situationService);
           
            //Act
            ViewResult result = situationController.Index() as ViewResult;
            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Test_ListOfSituations_ReturnsListOfSituationsInDatabase()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();

            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });

            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            var situationController = new SituationManagementController(_situationService);
            //Act
            var result = situationController.ListOfSituations() as JsonResult;
            
            //Assert   
            dynamic resultData = result.Data;
            Assert.AreEqual(2, resultData.Count);
            Assert.AreEqual("2", resultData[0].Id);
        }

        [TestMethod]
        public void Test_ListOfSituations_ReturnsZeroSituationsInDatabase()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();

            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            var situationController = new SituationManagementController(_situationService);
            //Act
            var result = situationController.ListOfSituations() as JsonResult;

            //Assert   
            dynamic resultData = result.Data;
            Assert.AreEqual(0, resultData.Count);
        }

        [TestMethod]
        public void Test_AddSituationDescriptionWhenSituationContainsOnlyLettersAndDoesNotExistInDatabase_SituationAdded()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            //Add 
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });

            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            var situationController = new SituationManagementController(_situationService);
            
            //Act
            var result = situationController.Add(new SituationViewModel() { Id = "", Description = "Open", Status = true, ExistInDB = true, IsValid = true }) as JsonResult;

            //Assert   
            dynamic resultData = result.Data;
            Assert.AreEqual(true, resultData.IsValid);
        }

        [TestMethod]
        public void Test_AddSituationDescriptionWhenSituationDoesNOTContainsOnlyLettersAndDoesNotExistInDatabase_SituationNotAdded()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            //Add 
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });

            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            var situationController = new SituationManagementController(_situationService);

            //Act
            var result = situationController.Add(new SituationViewModel() { Id = "", Description = "Open88", Status = true, ExistInDB = true, IsValid = true }) as JsonResult;

            //Assert   
            dynamic resultData = result.Data;
            Assert.AreEqual(false, resultData.IsValid);
        }

        [TestMethod]
        public void Test_AddSituationWhenSituationContainsOnlyLettersAndExistInDatabase_SituationDescriptionNOTAdded()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            //Add 
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });

            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            var situationController = new SituationManagementController(_situationService);

            //Act
            var result = situationController.Add(new SituationViewModel() { Id = "", Description = "Open", Status = true, ExistInDB = true, IsValid = true }) as JsonResult;

            //Assert   
            dynamic resultData = result.Data;
            Assert.AreEqual(true, resultData.ExistInDB);
        }

        [TestMethod]
        public void Test_AddSituationWhenSituationContainsOnlyLettersAndDoesNOTExistInDatabase_SituationsRetrievedEqualsToSituationsExpected()
        {
            //Arrange
            int situationsExpected = 3;
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            //Add 
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });

            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            var situationController = new SituationManagementController(_situationService);
            
            //Act
            var result = situationController.Add(new SituationViewModel() { Id = "", Description = "Closed", Status = true, ExistInDB = true, IsValid = true }) as JsonResult;
            var situationsRetrieved = _situationService.RetrieveAllSituations();
            //Assert   
            //dynamic resultData = result.Data;
            Assert.AreEqual(situationsExpected, situationsRetrieved.Count);
        }

        [TestMethod]
        public void Test_AddSituationWhenSituationIsEmpty_IsValidPropertyIsSetToFalse()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            //Add some records to populate the Mock table
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });

            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            var situationController = new SituationManagementController(_situationService);
            //Act
            var result = situationController.Add(new SituationViewModel() { Id = "", Description = "", Status = true, ExistInDB = true, IsValid = true }) as JsonResult;
            //Assert   
            dynamic resultData = result.Data;
            Assert.AreEqual(false, resultData.IsValid);
        }

        [TestMethod]
        public void Test_GetSituationByIdWhenSituationIdExistInDatabase_SituationDescriptionFound()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            //Add some records to populate the Mock table
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });

            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            var situationController = new SituationManagementController(_situationService);
            //Act
            var result = situationController.GetSituationById("2") as JsonResult;
            //Assert   
            dynamic resultData = result.Data;
            Assert.AreEqual("Open", resultData.Description);
        }

        [TestMethod]
        public void Test_GetSituationByIdWhenSituationIdDoesNOTExistInDatabase_ReturnsNullSituationViewModelObject()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            //Add some records to populate the Mock table
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });

            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            var situationController = new SituationManagementController(_situationService);
            //Act
            var result = situationController.GetSituationById("3") as JsonResult;
            //Assert   
            dynamic resultData = result.Data;
            Assert.AreEqual(null, resultData.Description);
        }

        [TestMethod]
        public void Test_SearchExactMatchSituationDescriptionWhenSituationIsValidAndExistInDatabase_ReturnsSituationData()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            //Add some records to populate the Mock table
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });

            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            var situationController = new SituationManagementController(_situationService);
            //Act
            var result = situationController.SearchSituationDescription(new SituationViewModel() {Id ="", Description = "InProgress", Status = true, IsValid = true, ExistInDB = true }) as JsonResult;
            //Assert   
            dynamic resultData = result.Data;
            Assert.AreEqual("InProgress", resultData.Description);
        }

        [TestMethod]
        public void Test_SearchLowerCaseLettersSituationDescriptionWhenSituationIsValidAndExistInDatabase_ReturnsSituationData()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            //Add some records to populate the Mock table
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });

            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            var situationController = new SituationManagementController(_situationService);
            //Act
            var result = situationController.SearchSituationDescription(new SituationViewModel() { Id = "", Description = "inprogress", Status = true, IsValid = true, ExistInDB = true }) as JsonResult;
            //Assert   
            dynamic resultData = result.Data;
            Assert.AreEqual("InProgress", resultData.Description);
        }

        [TestMethod]
        public void Test_SearchSituationDescriptionWhenSituationIsValidAndDoesNOTExistInDatabase_ReturnsNullDescription()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            //Add some records to populate the Mock table
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });

            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            var situationController = new SituationManagementController(_situationService);
            //Act
            var result = situationController.SearchSituationDescription(new SituationViewModel() { Id = "", Description = "Closed", Status = true, IsValid = true, ExistInDB = true }) as JsonResult;
            //Assert   
            dynamic resultData = result.Data;
            Assert.AreEqual(null, resultData.Description);
        }

        [TestMethod]
        public void Test_SearchSituationDescriptionWhenSituationIsValidAndDoesNOTExistInDatabase_ExistInDBPropertyReturnsFalse()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            //Add some records to populate the Mock table
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });

            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            var situationController = new SituationManagementController(_situationService);
            //Act
            var result = situationController.SearchSituationDescription(new SituationViewModel() { Id = "", Description = "Closed", Status = true, IsValid = true, ExistInDB = true }) as JsonResult;
            //Assert   
            dynamic resultData = result.Data;
            Assert.AreEqual(false, resultData.ExistInDB);
        }

        [TestMethod]
        public void Test_SearchSituationDescriptionExactMatchWhenSituationIsValidAndExistInDatabase_ExistInDBPropertyReturnsTrue()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            //Add some records to populate the Mock table
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });

            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            var situationController = new SituationManagementController(_situationService);
            //Act
            var result = situationController.SearchSituationDescription(new SituationViewModel() { Id = "", Description = "Open", Status = true, IsValid = true, ExistInDB = true }) as JsonResult;
            //Assert   
            dynamic resultData = result.Data;
            Assert.AreEqual(true, resultData.ExistInDB);
        }

        [TestMethod]
        public void Test_SearchLowerCaseLettersSituationDescriptionWhenSituationIsValidAndExistInDatabase_ExistInDBPropertyReturnsTrue()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            //Add some records to populate the Mock table
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });

            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            var situationController = new SituationManagementController(_situationService);
            //Act
            var result = situationController.SearchSituationDescription(new SituationViewModel() { Id = "", Description = "open", Status = true, IsValid = true, ExistInDB = true }) as JsonResult;
            //Assert   
            dynamic resultData = result.Data;
            Assert.AreEqual(true, resultData.ExistInDB);
        }

        [TestMethod]
        public void Test_SearchSituationDescriptionWhenSituationIsNOTValid_IsValidPropertyReturnsFalse()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            //Add some records to populate the Mock table
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });

            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            var situationController = new SituationManagementController(_situationService);
            //Act
            var result = situationController.SearchSituationDescription(new SituationViewModel() { Id = "", Description = "open88", Status = true, IsValid = true, ExistInDB = true }) as JsonResult;
            //Assert   
            dynamic resultData = result.Data;
            Assert.AreEqual(false, resultData.IsValid);
        }

        [TestMethod]
        public void Test_SearchSituationDescriptionWhenSituationIsEmpty_IsValidPropertyReturnsFalse()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            //Add some records to populate the Mock table
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });

            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            var situationController = new SituationManagementController(_situationService);
            //Act
            var result = situationController.SearchSituationDescription(new SituationViewModel() { Id = "", Description = "", Status = true, IsValid = true, ExistInDB = true }) as JsonResult;
            //Assert   
            dynamic resultData = result.Data;
            Assert.AreEqual(false, resultData.IsValid);
        }

        [TestMethod]
        public void Test_SearchSituationDescriptionWhenSituationIsNull_IsValidPropertyReturnsFalse()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            //Add some records to populate the Mock table
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });

            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            var situationController = new SituationManagementController(_situationService);
            //Act
            var result = situationController.SearchSituationDescription(new SituationViewModel() { Id = "", Description = null, Status = true, IsValid = true, ExistInDB = true }) as JsonResult;
            //Assert   
            dynamic resultData = result.Data;
            Assert.AreEqual(false, resultData.IsValid);
        }

        [TestMethod]
        public void Test_UpdateSituationDescriptionWhenSituationIsValidAndDoesNOTExistInDatabase_SituationDescriptionUpdated()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            //Add some records to populate the Mock table
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });

            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            var situationController = new SituationManagementController(_situationService);
            //Act
            situationController.Update(new SituationViewModel() { Id = "2", Description = "Opened", Status = true, IsValid = true, ExistInDB = true });

            var result = situationController.GetSituationById("2") as JsonResult;
            dynamic resultData = result.Data;
            //Assert
            Assert.AreEqual("Opened", resultData.Description);
        }

        [TestMethod]
        public void Test_UpdateSituationDescriptionWhenSituationIsValidAndExistInDatabase_ExistInDBPropertyReturnsTrue()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            //Add some records to populate the Mock table
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });

            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            var situationController = new SituationManagementController(_situationService);
            //Act
            situationController.Update(new SituationViewModel() { Id = "2", Description = "Open", Status = true, IsValid = true, ExistInDB = true });

            var result = situationController.GetSituationById("2") as JsonResult;
            dynamic resultData = result.Data;
            //Assert
            Assert.AreEqual(true, resultData.ExistInDB);
        }

        [TestMethod]
        public void Test_UpdateSituationDescriptionWhenSituationIsNOTValid_IsValidPropertyReturnsFalse()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            //Add some records to populate the Mock table
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });

            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            var situationController = new SituationManagementController(_situationService);
            //Act
            var result = situationController.Update(new SituationViewModel() { Id = "2", Description = "Open2014", Status = true, IsValid = true, ExistInDB = true });

            //var result = situationController.GetSituationById("2") as JsonResult;
            dynamic resultData = result.Data;
            //Assert
            Assert.AreEqual(false, resultData.IsValid);
        }

        [TestMethod]
        public void Test_UpdateSituationDescriptionWhenSituationIsEmpty_IsValidPropertyReturnsFalse()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            //Add some records to populate the Mock table
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });

            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            var situationController = new SituationManagementController(_situationService);
            //Act
            var result = situationController.Update(new SituationViewModel() { Id = "2", Description = "", Status = true, IsValid = true, ExistInDB = true });
            dynamic resultData = result.Data;
            //Assert
            Assert.AreEqual(false, resultData.IsValid);
        }

        [TestMethod]
        public void Test_UpdateSituationDescriptionWhenSituationIsNull_IsValidPropertyReturnsFalse()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            //Add some records to populate the Mock table
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });

            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            var situationController = new SituationManagementController(_situationService);
            //Act
            var result = situationController.Update(new SituationViewModel() { Id = "2", Description = "", Status = true, IsValid = true, ExistInDB = true });
            dynamic resultData = result.Data;
            //Assert
            Assert.AreEqual(false, resultData.IsValid);
        }

        [TestMethod]
        public void Test_DeleteSituationDescriptionByIdWhenSituationExist_SituationDescriptionDeleted()
        {
            //Arrange
            IRepository<Situation> _situationContext = new MockTruckCheckUpContext<Situation>();
            ILogger _logger = new MockTruckCheckUpLogger();
            IValidateUserInput _validateUserInput = new ValidateUserInput();
            //Add some records to populate the Mock table
            _situationContext.Insert(new Situation() { Id = "1", CreationDate = DateTimeOffset.Now, Description = "InProgress", Status = true });
            _situationContext.Insert(new Situation() { Id = "2", CreationDate = DateTimeOffset.Now, Description = "Open", Status = true });

            ISituationService _situationService = new SituationService(_situationContext, _logger, _validateUserInput);
            var situationController = new SituationManagementController(_situationService);
            //Act
            situationController.Delete("2");
            //Search for Id deleted to validate deletion
            var result = situationController.GetSituationById("2");

            dynamic resultData = result.Data;
            //Assert
            Assert.AreEqual(null, resultData.Description);
        }  
    }
}
