using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.Contracts.DataAccess;
using TruckCheckUp.Services;
using TruckCheckUp.Core.ViewModels.TruckManufacturerUI;

namespace TruckCheckUp.WebUI.Tests.Services
{
    [TestClass]
    public class TruckManufacturerUnitTests
    {
        [TestMethod]
        public void Test_RetrieveTruckManufacturers_AllRecords()
        {
            //Arrange
            IRepository<TruckManufacturer> _truckManufacturerContext = new Mocks.MockTruckCheckUpContext<TruckManufacturer>();
            //Add couple records to truck manufacturer mock class
            _truckManufacturerContext.Insert(
            new TruckManufacturer() { Id = "1", ManufacturerDescription = "Isuzu" });
            _truckManufacturerContext.Insert(
                new TruckManufacturer() { Id = "2", ManufacturerDescription = "Ford" });
            TruckManufacturerService truckManufacturerService = new TruckManufacturerService( _truckManufacturerContext);
            //Act
            var result = truckManufacturerService.RetrieveAllTruckManufacturers();
            //Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void Test_RetrieveTruckManufacturers_ZeroRecords()
        {
            //Arrange
            IRepository<TruckManufacturer> _truckManufacturerContext = new Mocks.MockTruckCheckUpContext<TruckManufacturer>();
            TruckManufacturerService truckManufacturerService = new TruckManufacturerService(_truckManufacturerContext);
            //Act
            var result = truckManufacturerService.RetrieveAllTruckManufacturers();
            //Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Test_ManufacturerDescriptionIsNotEmptyOrNullAndContainsOnlyLettersOrNumbersAndDoesNotExistAlreadyInDB_RecordAdded()
        {
            //Arrange
            IRepository<TruckManufacturer> _truckManufacturerContext = new Mocks.MockTruckCheckUpContext<TruckManufacturer>();
            TruckManufacturerService truckManufacturerService = new TruckManufacturerService(_truckManufacturerContext);
            truckManufacturerService.AddTruckManufacturer(
                new TruckManufacturerViewModel() { Id = "3", ManufacturerDescription = "GMC" });

            var manufacturerToInsert= new TruckManufacturerViewModel() { Id = "4", ManufacturerDescription = "Chevy" };


            if (!string.IsNullOrEmpty(manufacturerToInsert.ManufacturerDescription))
            {
                if (truckManufacturerService.ValidateManufacturerString(manufacturerToInsert.ManufacturerDescription))
                    {
                      if (!truckManufacturerService.RetrieveTruckManufacturerName(manufacturerToInsert.ManufacturerDescription))
                        {
                        truckManufacturerService.AddTruckManufacturer(manufacturerToInsert); 
                        }
                    }

            }
        
                //Act
            var result = truckManufacturerService.RetrieveAllTruckManufacturers();
            //Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void Test_ManufacturerDescriptionFieldEmptyOrNull_NoRecordAdded()
        {
            //Arrange
            IRepository<TruckManufacturer> _truckManufacturerContext = new Mocks.MockTruckCheckUpContext<TruckManufacturer>();
            TruckManufacturerService truckManufacturerService = new TruckManufacturerService(_truckManufacturerContext);
            truckManufacturerService.AddTruckManufacturer(new TruckManufacturerViewModel()
            { Id = "5", ManufacturerDescription = "Toyota" });
            //Add record with empty ManufacturerDescription
            var manufacturerToInsert = new TruckManufacturerViewModel()
            { Id = "6", ManufacturerDescription = "" };
            //Act
            if (!string.IsNullOrEmpty(manufacturerToInsert.ManufacturerDescription))
            {
                truckManufacturerService.AddTruckManufacturer(manufacturerToInsert);
            }
            var result = truckManufacturerService.RetrieveAllTruckManufacturers();
            //Assert
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void Test_ManufacturerDescriptionFieldContainsNotAllowedCharacters_ManufacturerNotAdded()
        {
            //Arrange
            IRepository<TruckManufacturer> _truckManufacturerContext = new Mocks.MockTruckCheckUpContext<TruckManufacturer>();
            TruckManufacturerService truckManufacturerService = new TruckManufacturerService(_truckManufacturerContext);
            truckManufacturerService.AddTruckManufacturer(new TruckManufacturerViewModel()
            { Id = "7", ManufacturerDescription = "Hyundai" });
            //Add record with characters not allowed
            var newManufacturerToInsert = new TruckManufacturerViewModel()
            { Id = "8", ManufacturerDescription = "Intern@tion@l" };
            
            //Act
            if (truckManufacturerService.ValidateManufacturerString(newManufacturerToInsert.ManufacturerDescription))
            {
                truckManufacturerService.AddTruckManufacturer(newManufacturerToInsert);
            }
            var result = truckManufacturerService.RetrieveAllTruckManufacturers();
            //Assert
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void Test_ManufacturerDescriptionFieldAlreadyExistInDB_ManufacturerNotAdded()
        {
            //Arrange
            IRepository<TruckManufacturer> _truckManufacturerContext = new Mocks.MockTruckCheckUpContext<TruckManufacturer>();
            TruckManufacturerService truckManufacturerService = new TruckManufacturerService(_truckManufacturerContext);
            truckManufacturerService.AddTruckManufacturer(new TruckManufacturerViewModel()
            { Id = "7", ManufacturerDescription = "Hyundai" });
            //Add manufacturer that already exist
            var manufacturerToInsert = new TruckManufacturerViewModel()
            { Id = "8", ManufacturerDescription = "Hyundai" };

            //Act
            if (!truckManufacturerService.RetrieveTruckManufacturerName(manufacturerToInsert.ManufacturerDescription))
            {
                truckManufacturerService.AddTruckManufacturer(manufacturerToInsert);
            }
            var result = truckManufacturerService.RetrieveAllTruckManufacturers();
            //Assert
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void Test_ManufacturerDescriptionIsNotEmptyOrNullAndContainsOnlyLettersOrNumbersAndDoesNotExistAlreadyInDB_RecordUpdated()
        {
            //Arrange
            IRepository<TruckManufacturer> _truckManufacturerContext = new Mocks.MockTruckCheckUpContext<TruckManufacturer>();
            TruckManufacturerService truckManufacturerService = new TruckManufacturerService(_truckManufacturerContext);
            truckManufacturerService.AddTruckManufacturer(
                new TruckManufacturerViewModel() { ManufacturerDescription = "Chevrolet" });

            var manufacturerSearchResult = truckManufacturerService.SearchTruckManufacturer("Chevrolet");

            var manufacturerToUpdate = new TruckManufacturerViewModel()
            {
                Id = manufacturerSearchResult.Id, ManufacturerDescription = "Chevy"
            };
            
            if (!string.IsNullOrEmpty(manufacturerToUpdate.ManufacturerDescription))
            {
                if (truckManufacturerService.ValidateManufacturerString(manufacturerToUpdate.ManufacturerDescription))
                {
                    if (!truckManufacturerService.RetrieveTruckManufacturerName(manufacturerToUpdate.ManufacturerDescription))
                    {
                        truckManufacturerService.UpdateTruckManufacturerData(manufacturerToUpdate);
                    }
                }

            }

            //Act
            var result = truckManufacturerService.SearchTruckManufacturer("Chevy");
            //Assert
            Assert.AreEqual("Chevy", result.ManufacturerDescription);
        }

        [TestMethod]
        public void Test_ManufacturerDescriptionFieldContainsNotAllowedCharacters_ManufacturerNotUpdated()
        {
            //Arrange
            IRepository<TruckManufacturer> _truckManufacturerContext = new Mocks.MockTruckCheckUpContext<TruckManufacturer>();
            TruckManufacturerService truckManufacturerService = new TruckManufacturerService(_truckManufacturerContext);
            truckManufacturerService.AddTruckManufacturer(new TruckManufacturerViewModel()
            { Id = "7", ManufacturerDescription = "Hyundai" });
            //Add record with characters not allowed
            var manufacturerToUpdate = new TruckManufacturerViewModel()
            { Id = "7", ManufacturerDescription = "Kenw@rth" };

            //Act
            if (truckManufacturerService.ValidateManufacturerString(manufacturerToUpdate.ManufacturerDescription))
            {
                truckManufacturerService.UpdateTruckManufacturerData(manufacturerToUpdate);
            }
            var result = truckManufacturerService.SearchTruckManufacturer("Hyundai"); 
            //Assert
            Assert.AreEqual("Hyundai", result.ManufacturerDescription);
        }

        [TestMethod]
        public void Test_ManufacturerDescriptionFieldEmptyOrNull_NoRecordUpdated()
        {
            //Arrange
            IRepository<TruckManufacturer> _truckManufacturerContext = new Mocks.MockTruckCheckUpContext<TruckManufacturer>();
            TruckManufacturerService truckManufacturerService = new TruckManufacturerService(_truckManufacturerContext);
            truckManufacturerService.AddTruckManufacturer(new TruckManufacturerViewModel()
            { ManufacturerDescription = "Toyota" });
            var manufacturerSearchResult = truckManufacturerService.SearchTruckManufacturer("Toyota");

            var manufacturerToUpdate = new TruckManufacturerViewModel()
            {
                Id = manufacturerSearchResult.Id,
                ManufacturerDescription = ""
            };
            //Act
            if (!string.IsNullOrEmpty(manufacturerToUpdate.ManufacturerDescription))
            {
                truckManufacturerService.UpdateTruckManufacturerData(manufacturerToUpdate);
            }
            var result = truckManufacturerService.SearchTruckManufacturer("Toyota");
            //Assert
            Assert.AreEqual("Toyota", result.ManufacturerDescription);
        }

        [TestMethod]
        public void Test_SearchForManufacturerInDB_ManufacturerDoesNotExist()
        {
            //Arrange
            IRepository<TruckManufacturer> _truckManufacturerContext = new Mocks.MockTruckCheckUpContext<TruckManufacturer>();
            TruckManufacturerService truckManufacturerService = new TruckManufacturerService(_truckManufacturerContext);
            truckManufacturerService.AddTruckManufacturer(
                new TruckManufacturerViewModel() { ManufacturerDescription = "Chevrolet" });

            truckManufacturerService.AddTruckManufacturer(
               new TruckManufacturerViewModel() { ManufacturerDescription = "Dodge" });
       
            //Act
            var result = truckManufacturerService.SearchTruckManufacturer("Toyota");
            //Assert
            Assert.AreEqual(false, result.ManufacturerExistInDB);
        }

        [TestMethod]
        public void Test_DeleteManufacturerFromDB_ManufacturerDeleted()
        {
            //Arrange
            IRepository<TruckManufacturer> _truckManufacturerContext = new Mocks.MockTruckCheckUpContext<TruckManufacturer>();
            TruckManufacturerService truckManufacturerService = new TruckManufacturerService(_truckManufacturerContext);
            truckManufacturerService.AddTruckManufacturer(
                new TruckManufacturerViewModel() { ManufacturerDescription = "Chevrolet" });

            truckManufacturerService.AddTruckManufacturer(
               new TruckManufacturerViewModel() { ManufacturerDescription = "Dodge" });

            var manufacturerToDelete = truckManufacturerService.SearchTruckManufacturer("Chevrolet");

            //Act
            truckManufacturerService.DeleteTruckManufacturer(manufacturerToDelete.Id);
            var result = truckManufacturerService.SearchTruckManufacturer("Chevrolet");
            //Assert
            Assert.AreEqual(false, result.ManufacturerExistInDB);
        }
    }
}

