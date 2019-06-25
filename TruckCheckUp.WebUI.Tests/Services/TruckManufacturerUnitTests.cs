using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.Contracts.DataAccess;
using TruckCheckUp.Services;
using TruckCheckUp.Core.ViewModels.TruckManufacturerUI;
using TruckCheckUp.Core.Contracts.Logger;
using TruckCheckUp.WebUI.Tests.Mocks;

namespace TruckCheckUp.WebUI.Tests.Services
{
    [TestClass]
    public class TruckManufacturerUnitTests
    {
        [TestMethod]
        public void Test_RetrieveTruckManufacturers_AllRecords()
        {
            //Arrange
            IRepository<TruckManufacturer> _truckManufacturerContext = new MockTruckCheckUpContext<TruckManufacturer>();
            ILogger _logger = new MockTruckCheckUpLogger();
            //Add couple records to truck manufacturer mock class
            _truckManufacturerContext.Insert(new TruckManufacturer() { Id = "1", ManufacturerDescription = "Isuzu" });
            _truckManufacturerContext.Insert(new TruckManufacturer() { Id = "2", ManufacturerDescription = "Ford" });
            TruckManufacturerService truckManufacturerService = new TruckManufacturerService( _truckManufacturerContext, _logger);
            //Act
            var result = truckManufacturerService.RetrieveAllTruckManufacturers();
            _logger.Info("Test-RetrieveTruckManufacturers-AllRecords");
            //Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void Test_RetrieveTruckManufacturers_ZeroRecords()
        {
            //Arrange
            IRepository<TruckManufacturer> _truckManufacturerContext = new MockTruckCheckUpContext<TruckManufacturer>();
            ILogger _logger = new MockTruckCheckUpLogger();
            TruckManufacturerService truckManufacturerService = new TruckManufacturerService(_truckManufacturerContext, _logger);
            //Act
            var result = truckManufacturerService.RetrieveAllTruckManufacturers();
            _logger.Info("Test-RetrieveTruckManufacturers-ZeroRecords");
            //Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Test_ManufacturerDescriptionIsNotEmptyOrNullAndContainsOnlyLettersOrNumbersAndDoesNotExistAlreadyInDB_ManufacturerAdded()
        {
            //Arrange
            IRepository<TruckManufacturer> _truckManufacturerContext = new MockTruckCheckUpContext<TruckManufacturer>();
            ILogger _logger = new MockTruckCheckUpLogger();
            TruckManufacturerService truckManufacturerService = new TruckManufacturerService(_truckManufacturerContext, _logger);
            truckManufacturerService.ValidateTruckManufacturerToAdd(new TruckManufacturerViewModel()
            { Id = "3", Description = "GMC" });

            var manufacturerToInsertViewModel= new TruckManufacturerViewModel()
            { Id = "4", Description = "Chevy" };

            if (!string.IsNullOrEmpty(manufacturerToInsertViewModel.Description))
            {
                if (truckManufacturerService.ValidateManufacturerString(manufacturerToInsertViewModel.Description))
                    {
                      if (!truckManufacturerService.RetrieveTruckManufacturerName(manufacturerToInsertViewModel.Description))
                        {
                        truckManufacturerService.SaveNewTruckManufacturerData(manufacturerToInsertViewModel, new Log4NetLogger());
                        }
                    }
            }
            //Act
            var result = truckManufacturerService.RetrieveAllTruckManufacturers();
            //Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void Test_ManufacturerDescriptionFieldEmptyOrNull_ManufacturerNotAdded()
        {
            //Arrange
            IRepository<TruckManufacturer> _truckManufacturerContext = new MockTruckCheckUpContext<TruckManufacturer>();
            ILogger _logger = new MockTruckCheckUpLogger();
            TruckManufacturerService truckManufacturerService = new TruckManufacturerService(_truckManufacturerContext, _logger);
            truckManufacturerService.ValidateTruckManufacturerToAdd(new TruckManufacturerViewModel()
            { Id = "5", Description = "Toyota" });
            //Arrange - Add record with empty ManufacturerDescription
            var manufacturerToInsertViewModel = new TruckManufacturerViewModel()
            { Id = "6", Description = "" };
            //Act
            if (!string.IsNullOrEmpty(manufacturerToInsertViewModel.Description))
            {
                truckManufacturerService.SaveNewTruckManufacturerData(manufacturerToInsertViewModel, new Log4NetLogger());
            }
            var result = truckManufacturerService.RetrieveAllTruckManufacturers();
            //Assert
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void Test_ManufacturerDescriptionFieldContainsCharactersNotAllowed_ManufacturerNotAdded()
        {
            //Arrange
            IRepository<TruckManufacturer> _truckManufacturerContext = new Mocks.MockTruckCheckUpContext<TruckManufacturer>();
            ILogger _logger = new MockTruckCheckUpLogger();
            TruckManufacturerService truckManufacturerService = new TruckManufacturerService(_truckManufacturerContext, _logger);
            truckManufacturerService.ValidateTruckManufacturerToAdd(new TruckManufacturerViewModel()
            { Id = "7", Description = "Hyundai" });
            //Add record with characters not allowed
            var manufacturerToInsertViewModel = new TruckManufacturerViewModel()
            { Id = "8", Description = "Intern@tion@l" };
            
            //Act
            if (truckManufacturerService.ValidateManufacturerString(manufacturerToInsertViewModel.Description))
            {
                truckManufacturerService.SaveNewTruckManufacturerData(manufacturerToInsertViewModel, new Log4NetLogger());
            }
            var result = truckManufacturerService.RetrieveAllTruckManufacturers();
            //Assert
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void Test_ManufacturerAlreadyExistInDB_ManufacturerNotAdded()
        {
            //Arrange
            IRepository<TruckManufacturer> _truckManufacturerContext = new Mocks.MockTruckCheckUpContext<TruckManufacturer>();
            ILogger _logger = new MockTruckCheckUpLogger();
            TruckManufacturerService truckManufacturerService = new TruckManufacturerService(_truckManufacturerContext, _logger);
            truckManufacturerService.ValidateTruckManufacturerToAdd(new TruckManufacturerViewModel()
            { Id = "7", Description = "Hyundai" });
            //Add manufacturer that already exist
            var manufacturerToInsertViewModel = new TruckManufacturerViewModel()
            { Id = "8", Description = "Hyundai" };

            //Act
            if (!truckManufacturerService.RetrieveTruckManufacturerName(manufacturerToInsertViewModel.Description))
            {
                truckManufacturerService.SaveNewTruckManufacturerData(manufacturerToInsertViewModel, new Log4NetLogger());
            }
            var result = truckManufacturerService.RetrieveAllTruckManufacturers();
            //Assert
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void Test_ManufacturerDescriptionIsNotEmptyOrNullAndContainsOnlyLettersOrNumbersAndDoesNotExistAlreadyInDB_RecordUpdated()
        {
            //Arrange =
            IRepository<TruckManufacturer> _truckManufacturerContext = new Mocks.MockTruckCheckUpContext<TruckManufacturer>();
            ILogger _logger = new MockTruckCheckUpLogger();
            TruckManufacturerService truckManufacturerService = new TruckManufacturerService(_truckManufacturerContext, _logger);
            truckManufacturerService.ValidateTruckManufacturerToAdd(
                new TruckManufacturerViewModel() { Id = "1", Description = "Chevrolet" });

            var manufacturerViewModel = new TruckManufacturerViewModel()
            { Id = "1",
            Description = "Chevrolet"
            };
            
            var manufacturerSearchResult = truckManufacturerService.SearchTruckManufacturer(manufacturerViewModel);

            var manufacturerToUpdateViewModel = new TruckManufacturerViewModel()
            {
                Id = manufacturerSearchResult.Id, Description = "Chevy"
            };
            
            if (!string.IsNullOrEmpty(manufacturerToUpdateViewModel.Description))
            {
                if (truckManufacturerService.ValidateManufacturerString(manufacturerToUpdateViewModel.Description))
                {
                    if (!truckManufacturerService.RetrieveTruckManufacturerName(manufacturerToUpdateViewModel.Description))
                    {
                        truckManufacturerService.UpdateTruckManufacturerData(manufacturerToUpdateViewModel, new Log4NetLogger());
                    }
                }

            }

            //Act
            var manufacturerToSearchAfterUpdate = new TruckManufacturerViewModel();
            manufacturerToSearchAfterUpdate.Description = "Chevy";
            var result = truckManufacturerService.SearchTruckManufacturer(manufacturerToSearchAfterUpdate);

            //Assert
            Assert.AreEqual("Chevy", result.Description);
        }

        [TestMethod]
        public void Test_ManufacturerDescriptionFieldContainsNotAllowedCharacters_ManufacturerNotUpdated()
        {
            //Arrange
            IRepository<TruckManufacturer> _truckManufacturerContext = new Mocks.MockTruckCheckUpContext<TruckManufacturer>();
            ILogger _logger = new MockTruckCheckUpLogger();
            TruckManufacturerService truckManufacturerService = new TruckManufacturerService(_truckManufacturerContext, _logger);
            truckManufacturerService.ValidateTruckManufacturerToAdd(new TruckManufacturerViewModel()
            { Id = "7", Description = "Hyundai" });
            //Add record with characters not allowed
            var manufacturerToUpdate = new TruckManufacturerViewModel()
            { Id = "7", Description = "Kenw@rth" };

            //Act
            if (truckManufacturerService.ValidateManufacturerString(manufacturerToUpdate.Description))
            {
                truckManufacturerService.UpdateTruckManufacturerData(manufacturerToUpdate, new Log4NetLogger());
            }

            var manufacturerUpdated = new TruckManufacturerViewModel();
            manufacturerUpdated.Description = "Hyundai";

            var result = truckManufacturerService.SearchTruckManufacturer(manufacturerUpdated); 
            //Assert
            Assert.AreEqual("Hyundai", result.Description);
        }

        [TestMethod]
        public void Test_ManufacturerDescriptionFieldEmptyOrNull_NoRecordUpdated()
        {
            //Arrange
            IRepository<TruckManufacturer> _truckManufacturerContext = new Mocks.MockTruckCheckUpContext<TruckManufacturer>();
            ILogger _logger = new MockTruckCheckUpLogger();
            TruckManufacturerService truckManufacturerService = new TruckManufacturerService(_truckManufacturerContext, _logger);
            truckManufacturerService.ValidateTruckManufacturerToAdd(new TruckManufacturerViewModel()
            { Description = "Toyota" });

            var manufacturer = new TruckManufacturerViewModel();
            manufacturer.Description = "Toyota";
            var manufacturerSearchResult = truckManufacturerService.SearchTruckManufacturer(manufacturer);

            var manufacturerToUpdate = new TruckManufacturerViewModel()
            {
                Id = manufacturerSearchResult.Id,
                Description = ""
            };
            //Act
            if (!string.IsNullOrEmpty(manufacturerToUpdate.Description))
            {
                truckManufacturerService.UpdateTruckManufacturerData(manufacturerToUpdate, new Log4NetLogger());
                _logger.Info("Test-ManufacturerDescriptionFieldEmptyOrNull-NoRecordUpdated");
            }

            var result = truckManufacturerService.SearchTruckManufacturer(manufacturer);
            //Assert
            Assert.AreEqual("Toyota", result.Description);
        }

        [TestMethod]
        public void Test_SearchForManufacturerInDB_ManufacturerDoesNotExist()
        {
            //Arrange
            IRepository<TruckManufacturer> _truckManufacturerContext = new MockTruckCheckUpContext<TruckManufacturer>();
            ILogger _logger = new MockTruckCheckUpLogger();
            TruckManufacturerService truckManufacturerService = new TruckManufacturerService(_truckManufacturerContext, _logger);
            truckManufacturerService.ValidateTruckManufacturerToAdd(
                new TruckManufacturerViewModel() { Description = "Chevrolet" });

            truckManufacturerService.ValidateTruckManufacturerToAdd(
               new TruckManufacturerViewModel() { Description = "Dodge" });

            //Act
            var manufacturer = new TruckManufacturerViewModel();
            manufacturer.Description = "Toyota";

            var result = truckManufacturerService.SearchTruckManufacturer(manufacturer);
            _logger.Info("Test-SearchForManufacturerInDB-ManufacturerDoesNotExist");
            //Assert
            Assert.AreEqual(false, result.ExistInDB);
        }

        [TestMethod]
        public void Test_DeleteManufacturerFromDB_ManufacturerDeleted()
        {
            //Arrange
            IRepository<TruckManufacturer> _truckManufacturerContext = new Mocks.MockTruckCheckUpContext<TruckManufacturer>();
            ILogger _logger = new MockTruckCheckUpLogger();
            TruckManufacturerService truckManufacturerService = new TruckManufacturerService(_truckManufacturerContext, _logger);
            truckManufacturerService.ValidateTruckManufacturerToAdd(
                new TruckManufacturerViewModel() { Description = "Chevrolet" });

            truckManufacturerService.ValidateTruckManufacturerToAdd(
               new TruckManufacturerViewModel() { Description = "Dodge" });

            var manufacturer = new TruckManufacturerViewModel();
            manufacturer.Description = "Chevrolet";

            var manufacturerToDelete = truckManufacturerService.SearchTruckManufacturer(manufacturer);
            //Act
            truckManufacturerService.ValidateTruckManufacturerToDelete(manufacturerToDelete.Id);
            _logger.Info("Test-DeleteManufacturerFromDB-ManufacturerDeleted");
            var result = truckManufacturerService.SearchTruckManufacturer(manufacturer);
            //Assert
            Assert.AreEqual(false, result.ExistInDB);
        }
    }
}

