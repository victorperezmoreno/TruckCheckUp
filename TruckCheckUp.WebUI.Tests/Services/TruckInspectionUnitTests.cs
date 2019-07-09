using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruckCheckUp.Contracts.Services;
using TruckCheckUp.Core.Contracts.DataAccess;
using TruckCheckUp.Core.Contracts.Logger;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.ViewModels.TruckInspection;
using TruckCheckUp.Services;
using TruckCheckUp.WebUI.Tests.Mocks;

namespace TruckCheckUp.WebUI.Tests.Services
{
    [TestClass]
    public class TruckInspectionUnitTests
    {
        [TestMethod]
        public void Test_CurrentMileageReportedIsGreaterThanPreviousSubmittedAndInspectionHasNOTBeenSubmittedForThisTruckToday_InspectionReportAddedToTruckInspectionTable()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            IRepository<TruckInspection> _truckInspectionContext = new MockTruckCheckUpContext<TruckInspection>();
            IRepository<Driver> _driverContext = new MockTruckCheckUpContext<Driver>();
            IRepository<Truck> _truckContext = new MockTruckCheckUpContext<Truck>();
            IRepository<PartsInspected> _partsInspectedContext = new MockTruckCheckUpContext<PartsInspected>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _partsInspectedContext, _logger);
            
            /****Create a Mock TruckInspection object that represents current data in DB ****/
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId ="10", Mileage = 1000});
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "11", Mileage = 1000});
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "12",  Mileage = 1000});

            /****Create a Mock Driver object that represents current data in DB****/
            _driverContext.Insert(new Driver() {FirstName = "Juan", LastName = "Gonzalez" });
            _driverContext.Insert(new Driver() { FirstName = "Jose", LastName = "Morales" });
            _driverContext.Insert(new Driver() { FirstName = "Jesus", LastName = "Ramos" });

            /****Create a Mock Truck object that represents current data in DB ****/
            _truckContext.Insert(new Truck() {TruckNumber = 10  });
            _truckContext.Insert(new Truck() { TruckNumber = 11 });
            _truckContext.Insert(new Truck() { TruckNumber = 12 });
        
            /*****Create PartCatalog mock class that represents data in DB and add 3 general parts*****/
            _partCatalogContext.Insert(new PartCatalog() {PartName = "Driver-Front Tire", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8"});
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Passenger-Rear Tire", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Check Engine Light", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            //Add 3 light parts
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Brake Light", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Passenger-Parking Light", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Headlamp", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            //add 3 fluid parts
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Engine Oil", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Transmission Oil", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Brake fluid", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });

            //Create ViewModel Mock that represents data coming from View
            var truckInspectionObjectFromViewWithDropDownAndCheckBoxListPopulated = _truckInspectionService.PopulateDriversTrucksAndPartsCatalog(new TruckInspectionViewModel()
            {
                DriverId = "1",
                TruckId = "10",
                CurrentMileage = "2000",
                LastMileageReported = 0,
                PartsReportedForeignKeyId = "1234",
                LastTimeAReportWasSubmitted = DateTime.Now.Date,
                //Populate of DropDownLists and Checkboxlists is done by the PopulateDriversTrucksAndPartsCatalog method
                Comments = "Few issues on this truck"
            });
            
            //Act
            _truckInspectionService.CreateTruckInspection(truckInspectionObjectFromViewWithDropDownAndCheckBoxListPopulated);
            var inspectionTableResult = _truckInspectionContext.Collection().ToList();
           
            //Assert
            Assert.AreEqual(4, inspectionTableResult.Count);  
        }

        [TestMethod]
        public void Test_CurrentMileageReportedIsGreaterThanPreviousSubmittedAndInspectionHasNOTBeenSubmittedForThisTruckToday_InspectedItemsAddedToPartsInspectedTable()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            IRepository<TruckInspection> _truckInspectionContext = new MockTruckCheckUpContext<TruckInspection>();
            IRepository<Driver> _driverContext = new MockTruckCheckUpContext<Driver>();
            IRepository<Truck> _truckContext = new MockTruckCheckUpContext<Truck>();
            IRepository<PartsInspected> _partsInspectedContext = new MockTruckCheckUpContext<PartsInspected>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _partsInspectedContext, _logger);

            /****Create a Mock TruckInspection object that represents current data in DB ****/
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "10", Mileage = 1000 });
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "11", Mileage = 1000 });
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "12", Mileage = 1000 });

            /****Create a Mock Driver object that represents current data in DB****/
            _driverContext.Insert(new Driver() { FirstName = "Juan", LastName = "Gonzalez" });
            _driverContext.Insert(new Driver() { FirstName = "Jose", LastName = "Morales" });
            _driverContext.Insert(new Driver() { FirstName = "Jesus", LastName = "Ramos" });

            /****Create a Mock Truck object that represents current data in DB ****/
            _truckContext.Insert(new Truck() { TruckNumber = 10 });
            _truckContext.Insert(new Truck() { TruckNumber = 11 });
            _truckContext.Insert(new Truck() { TruckNumber = 12 });

            /*****Create PartCatalog mock class that represents data in DB and add 3 general parts*****/
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Front Tire", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Passenger-Rear Tire", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Check Engine Light", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            //Add 3 light parts
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Brake Light", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Passenger-Parking Light", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Headlamp", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            //add 3 fluid parts
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Engine Oil", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Transmission Oil", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Brake fluid", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });

            //Create ViewModel Mock that represents data coming from View
            var truckInspectionObjectFromViewWithDropDownAndCheckBoxListPopulated = _truckInspectionService.PopulateDriversTrucksAndPartsCatalog(new TruckInspectionViewModel()
            {
                DriverId = "1",
                TruckId = "10",
                CurrentMileage = "2000",
                LastMileageReported = 0,
                PartsReportedForeignKeyId = "1234",
                LastTimeAReportWasSubmitted = DateTime.Now.Date,
                Comments = "Few issues on this truck"
            });

            //Act
            _truckInspectionService.CreateTruckInspection(truckInspectionObjectFromViewWithDropDownAndCheckBoxListPopulated);
            var result = _partsInspectedContext.Collection().ToList();
            //Assert
            Assert.AreEqual(9, result.Count);
        }

        [TestMethod]
        public void Test_CurrentMileageReportedIsGreaterThanPreviousSubmittedAndInspectionHasBeenSubmittedForThisTruckToday_InspectionReportNOTAdded()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            IRepository<TruckInspection> _truckInspectionContext = new MockTruckCheckUpContext<TruckInspection>();
            IRepository<Driver> _driverContext = new MockTruckCheckUpContext<Driver>();
            IRepository<Truck> _truckContext = new MockTruckCheckUpContext<Truck>();
            IRepository<PartsInspected> _partsInspectedContext = new MockTruckCheckUpContext<PartsInspected>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _partsInspectedContext, _logger);

            /****Create a Mock TruckInspection object that represents current data in DB ****/
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now, DriverId = "1", TruckId = "10", Mileage = 1000,});
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "11",Mileage = 1000,});
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "12", Mileage = 1000});

            /****Create a Mock Driver object that represents current data in DB****/
            _driverContext.Insert(new Driver() { FirstName = "Juan", LastName = "Gonzalez" });
            _driverContext.Insert(new Driver() { FirstName = "Jose", LastName = "Morales" });
            _driverContext.Insert(new Driver() { FirstName = "Jesus", LastName = "Ramos" });

            /****Create a Mock Truck object that represents current data in DB ****/
            _truckContext.Insert(new Truck() { TruckNumber = 10 });
            _truckContext.Insert(new Truck() { TruckNumber = 11 });
            _truckContext.Insert(new Truck() { TruckNumber = 12 });

            /*****Create PartCatalog mock class that represents data in DB and add 3 general parts*****/
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Front Tire", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Passenger-Rear Tire", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Check Engine Light", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            //Add 3 light parts
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Brake Light", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Passenger-Parking Light", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Headlamp", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            //add 3 fluid parts
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Engine Oil", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Transmission Oil", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Brake fluid", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });

            //Create ViewModel Mock that represents data coming from View
            var truckInspectionObjectFromViewWithDropDownAndCheckBoxListPopulated = _truckInspectionService.PopulateDriversTrucksAndPartsCatalog(new TruckInspectionViewModel()
            {
                DriverId = "1",
                TruckId = "10",
                CurrentMileage = "2000",
                LastMileageReported = 0,
                PartsReportedForeignKeyId = "1234",
                LastTimeAReportWasSubmitted = DateTime.Now.Date,
                //Populate of DropDownLists and Checkboxlists is done by the PopulateDriversTrucksAndPartsCatalog method
                Comments = "Few issues on this truck"
            });
            
            //Act
            _truckInspectionService.CreateTruckInspection(truckInspectionObjectFromViewWithDropDownAndCheckBoxListPopulated);
            var result = _truckInspectionContext.Collection().ToList();
            //Assert
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void Test_CurrentMileageReportedIsLesserThanPreviousSubmittedAndInspectionHasNOTBeenSubmittedForThisTruckToday_InspectionReportNOTAdded()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            IRepository<TruckInspection> _truckInspectionContext = new MockTruckCheckUpContext<TruckInspection>();
            IRepository<Driver> _driverContext = new MockTruckCheckUpContext<Driver>();
            IRepository<Truck> _truckContext = new MockTruckCheckUpContext<Truck>();
            IRepository<PartsInspected> _partsInspectedContext = new MockTruckCheckUpContext<PartsInspected>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _partsInspectedContext, _logger);

            /****Create a Mock TruckInspection object that represents current data in DB ****/
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "10",Mileage = 1000});
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "11",Mileage = 1000});
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "12",Mileage = 1000});

            /****Create a Mock Driver object that represents current data in DB****/
            _driverContext.Insert(new Driver() { FirstName = "Juan", LastName = "Gonzalez" });
            _driverContext.Insert(new Driver() { FirstName = "Jose", LastName = "Morales" });
            _driverContext.Insert(new Driver() { FirstName = "Jesus", LastName = "Ramos" });

            /****Create a Mock Truck object that represents current data in DB ****/
            _truckContext.Insert(new Truck() { TruckNumber = 10 });
            _truckContext.Insert(new Truck() { TruckNumber = 11 });
            _truckContext.Insert(new Truck() { TruckNumber = 12 });

            /*****Create PartCatalog mock class that represents data in DB and add 3 general parts*****/
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Front Tire", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Passenger-Rear Tire", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Check Engine Light", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            //Add 3 light parts
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Brake Light", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Passenger-Parking Light", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Headlamp", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            //add 3 fluid parts
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Engine Oil", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Transmission Oil", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Brake fluid", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });

            //Create ViewModel Mock that represents data coming from View
            var truckInspectionObjectFromViewWithDropDownAndCheckBoxListPopulated = _truckInspectionService.PopulateDriversTrucksAndPartsCatalog(new TruckInspectionViewModel()
            {
                DriverId = "1",
                TruckId = "10",
                CurrentMileage = "900",
                LastMileageReported = 0,
                PartsReportedForeignKeyId = "1234",
                LastTimeAReportWasSubmitted = DateTime.Now.Date,
                //Populate of DropDownLists and Checkboxlists is done by the PopulateDriversTrucksAndPartsCatalog method
                Comments = "Few issues on this truck"
            });
            
            //Act
            _truckInspectionService.CreateTruckInspection(truckInspectionObjectFromViewWithDropDownAndCheckBoxListPopulated);
            var result = _truckInspectionContext.Collection().ToList();
            //Assert
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void Test_CurrentMileageReportedIsLesserThanPreviousSubmittedAndInspectionHasBeenSubmittedForThisTruckToday_InspectionReportNOTAdded()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            IRepository<TruckInspection> _truckInspectionContext = new MockTruckCheckUpContext<TruckInspection>();
            IRepository<Driver> _driverContext = new MockTruckCheckUpContext<Driver>();
            IRepository<Truck> _truckContext = new MockTruckCheckUpContext<Truck>();
            IRepository<PartsInspected> _partsInspectedContext = new MockTruckCheckUpContext<PartsInspected>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _partsInspectedContext, _logger);

            /****Create a Mock TruckInspection object that represents current data in DB ****/
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now, DriverId = "1", TruckId = "10", Mileage = 1000});
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "11", Mileage = 1000});
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "12", Mileage = 1000});

            /****Create a Mock Driver object that represents current data in DB****/
            _driverContext.Insert(new Driver() { FirstName = "Juan", LastName = "Gonzalez" });
            _driverContext.Insert(new Driver() { FirstName = "Jose", LastName = "Morales" });
            _driverContext.Insert(new Driver() { FirstName = "Jesus", LastName = "Ramos" });

            /****Create a Mock Truck object that represents current data in DB ****/
            _truckContext.Insert(new Truck() { TruckNumber = 10 });
            _truckContext.Insert(new Truck() { TruckNumber = 11 });
            _truckContext.Insert(new Truck() { TruckNumber = 12 });

            /*****Create PartCatalog mock class that represents data in DB and add 3 general parts*****/
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Front Tire", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Passenger-Rear Tire", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Check Engine Light", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            //Add 3 light parts
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Brake Light", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Passenger-Parking Light", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Headlamp", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            //add 3 fluid parts
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Engine Oil", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Transmission Oil", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Brake fluid", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });

            //Create ViewModel Mock that represents data coming from View
            var truckInspectionObjectFromViewWithDropDownAndCheckBoxListPopulated = _truckInspectionService.PopulateDriversTrucksAndPartsCatalog(new TruckInspectionViewModel()
            {
                DriverId = "1",
                TruckId = "10",
                CurrentMileage = "900",
                LastMileageReported = 0,
                PartsReportedForeignKeyId = "1234",
                LastTimeAReportWasSubmitted = DateTime.Now.Date,
                //Populate of DropDownLists and Checkboxlists is done by the PopulateDriversTrucksAndPartsCatalog method
                Comments = "Few issues on this truck"
            });
            
            //Act
            _truckInspectionService.CreateTruckInspection(truckInspectionObjectFromViewWithDropDownAndCheckBoxListPopulated);
            var result = _truckInspectionContext.Collection().ToList();
            //Assert
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void Test_CurrentMileageReportedIsGreaterThanPreviousSubmittedAndInspectionHasNOTBeenSubmittedForThisTruckToday_AlertStylePropertyEqualToSuccess()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            IRepository<TruckInspection> _truckInspectionContext = new MockTruckCheckUpContext<TruckInspection>();
            IRepository<Driver> _driverContext = new MockTruckCheckUpContext<Driver>();
            IRepository<Truck> _truckContext = new MockTruckCheckUpContext<Truck>();
            IRepository<PartsInspected> _partsInspectedContext = new MockTruckCheckUpContext<PartsInspected>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _partsInspectedContext, _logger);

            /****Create a Mock TruckInspection object that represents current data in DB ****/
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "10", Mileage = 1000});
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "11", Mileage = 1000});
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "12", Mileage = 1000});

            /****Create a Mock Driver object that represents current data in DB****/
            _driverContext.Insert(new Driver() { FirstName = "Juan", LastName = "Gonzalez" });
            _driverContext.Insert(new Driver() { FirstName = "Jose", LastName = "Morales" });
            _driverContext.Insert(new Driver() { FirstName = "Jesus", LastName = "Ramos" });

            /****Create a Mock Truck object that represents current data in DB ****/
            _truckContext.Insert(new Truck() { TruckNumber = 10 });
            _truckContext.Insert(new Truck() { TruckNumber = 11 });
            _truckContext.Insert(new Truck() { TruckNumber = 12 });

            /*****Create PartCatalog mock class that represents data in DB and add 3 general parts*****/
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Front Tire", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Passenger-Rear Tire", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Check Engine Light", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            //Add 3 light parts
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Brake Light", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Passenger-Parking Light", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Headlamp", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            //add 3 fluid parts
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Engine Oil", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Transmission Oil", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Brake fluid", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });

            //Create ViewModel Mock that represents data coming from View
            var truckInspectionObjectFromViewWithDropDownAndCheckBoxListPopulated = _truckInspectionService.PopulateDriversTrucksAndPartsCatalog(new TruckInspectionViewModel()
            {
                DriverId = "1",
                TruckId = "10",
                CurrentMileage = "2000",
                LastMileageReported = 0,
                PartsReportedForeignKeyId = "1234",
                LastTimeAReportWasSubmitted = DateTime.Now.Date,
                //Populate of DropDownLists and Checkboxlists is done by the PopulateDriversTrucksAndPartsCatalog method
                Comments = "Few issues on this truck"
            });
           
            //Act
            var result = _truckInspectionService.CreateTruckInspection(truckInspectionObjectFromViewWithDropDownAndCheckBoxListPopulated);
         
            //Assert
            Assert.AreEqual("success", result.AlertStyle);
        }

        [TestMethod]
        public void Test_CurrentMileageReportedIsGreaterThanPreviousSubmittedAndInspectionHasBeenSubmittedForThisTruckToday_AlertStylePropertyEqualToDanger()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            IRepository<TruckInspection> _truckInspectionContext = new MockTruckCheckUpContext<TruckInspection>();
            IRepository<Driver> _driverContext = new MockTruckCheckUpContext<Driver>();
            IRepository<Truck> _truckContext = new MockTruckCheckUpContext<Truck>();
            IRepository<PartsInspected> _partsInspectedContext = new MockTruckCheckUpContext<PartsInspected>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _partsInspectedContext, _logger);

            /****Create a Mock TruckInspection object that represents current data in DB ****/
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now, DriverId = "1", TruckId = "10", Mileage = 1000});
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "11", Mileage = 1000});
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "12", Mileage = 1000});

            /****Create a Mock Driver object that represents current data in DB****/
            _driverContext.Insert(new Driver() { FirstName = "Juan", LastName = "Gonzalez" });
            _driverContext.Insert(new Driver() { FirstName = "Jose", LastName = "Morales" });
            _driverContext.Insert(new Driver() { FirstName = "Jesus", LastName = "Ramos" });

            /****Create a Mock Truck object that represents current data in DB ****/
            _truckContext.Insert(new Truck() { TruckNumber = 10 });
            _truckContext.Insert(new Truck() { TruckNumber = 11 });
            _truckContext.Insert(new Truck() { TruckNumber = 12 });

            /*****Create PartCatalog mock class that represents data in DB and add 3 general parts*****/
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Front Tire", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Passenger-Rear Tire", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Check Engine Light", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            //Add 3 light parts
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Brake Light", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Passenger-Parking Light", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Headlamp", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            //add 3 fluid parts
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Engine Oil", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Transmission Oil", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Brake fluid", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });

            //Create ViewModel Mock that represents data coming from View
            var truckInspectionObjectFromViewWithDropDownAndCheckBoxListPopulated = _truckInspectionService.PopulateDriversTrucksAndPartsCatalog(new TruckInspectionViewModel()
            {
                DriverId = "1",
                TruckId = "10",
                CurrentMileage = "2000",
                LastMileageReported = 0,
                PartsReportedForeignKeyId = "1234",
                LastTimeAReportWasSubmitted = DateTime.Now.Date,
                //Populate of DropDownLists and Checkboxlists is done by the PopulateDriversTrucksAndPartsCatalog method
                Comments = "Few issues on this truck"
            });
           
            //Act
            var result = _truckInspectionService.CreateTruckInspection(truckInspectionObjectFromViewWithDropDownAndCheckBoxListPopulated);
            
            //Assert
            Assert.AreEqual("danger", result.AlertStyle);
        }

        [TestMethod]
        public void Test_CurrentMileageReportedIsGreaterThanPreviousSubmittedAndInspectionHasBeenSubmittedForThisTruckToday_InspectionCommentsNOTAddedToDriverCommentsTable()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            IRepository<TruckInspection> _truckInspectionContext = new MockTruckCheckUpContext<TruckInspection>();
            IRepository<Driver> _driverContext = new MockTruckCheckUpContext<Driver>();
            IRepository<Truck> _truckContext = new MockTruckCheckUpContext<Truck>();
            IRepository<PartsInspected> _partsInspectedContext = new MockTruckCheckUpContext<PartsInspected>();
            ILogger _logger = new MockTruckCheckUpLogger();           
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _partsInspectedContext, _logger);

            /****Create a Mock TruckInspection object that represents current data in DB ****/
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now, DriverId = "1", TruckId = "10", Mileage = 1000});
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "11", Mileage = 1000});
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "12", Mileage = 1000});

            /****Create a Mock Driver object that represents current data in DB****/
            _driverContext.Insert(new Driver() { FirstName = "Juan", LastName = "Gonzalez" });
            _driverContext.Insert(new Driver() { FirstName = "Jose", LastName = "Morales" });
            _driverContext.Insert(new Driver() { FirstName = "Jesus", LastName = "Ramos" });

            /****Create a Mock Truck object that represents current data in DB ****/
            _truckContext.Insert(new Truck() { TruckNumber = 10 });
            _truckContext.Insert(new Truck() { TruckNumber = 11 });
            _truckContext.Insert(new Truck() { TruckNumber = 12 });

            /*****Create PartCatalog mock class that represents data in DB and add 3 general parts*****/
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Front Tire", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Passenger-Rear Tire", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Check Engine Light", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            //Add 3 light parts
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Brake Light", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Passenger-Parking Light", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Headlamp", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            //add 3 fluid parts
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Engine Oil", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Transmission Oil", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Brake fluid", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });

            //Create ViewModel Mock that represents data coming from View
            var truckInspectionObjectFromViewWithDropDownAndCheckBoxListPopulated = _truckInspectionService.PopulateDriversTrucksAndPartsCatalog(new TruckInspectionViewModel()
            {
                DriverId = "1",
                TruckId = "10",
                CurrentMileage = "2000",
                LastMileageReported = 0,
                PartsReportedForeignKeyId = "1234",
                LastTimeAReportWasSubmitted = DateTime.Now.Date,
                //Populate of DropDownLists and Checkboxlists is done by the PopulateDriversTrucksAndPartsCatalog method
                Comments = "Few issues on this truck"
            });
            
            //Act
            _truckInspectionService.CreateTruckInspection(truckInspectionObjectFromViewWithDropDownAndCheckBoxListPopulated);
            var result = _partsInspectedContext.Collection().ToList();
            //Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Test_CurrentMileageReportedIsLesserThanPreviousSubmittedAndInspectionHasNOTBeenSubmittedForThisTruckToday_InspectionCommentsNOTAddedToDriverCommentsTable()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            IRepository<TruckInspection> _truckInspectionContext = new MockTruckCheckUpContext<TruckInspection>();
            IRepository<Driver> _driverContext = new MockTruckCheckUpContext<Driver>();
            IRepository<Truck> _truckContext = new MockTruckCheckUpContext<Truck>();
            IRepository<PartsInspected> _partsInspectedContext = new MockTruckCheckUpContext<PartsInspected>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _partsInspectedContext, _logger);

            /****Create a Mock TruckInspection object that represents current data in DB ****/
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "10", Mileage = 1000});
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "11", Mileage = 1000});
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "12", Mileage = 1000});

            /****Create a Mock Driver object that represents current data in DB****/
            _driverContext.Insert(new Driver() { FirstName = "Juan", LastName = "Gonzalez" });
            _driverContext.Insert(new Driver() { FirstName = "Jose", LastName = "Morales" });
            _driverContext.Insert(new Driver() { FirstName = "Jesus", LastName = "Ramos" });

            /****Create a Mock Truck object that represents current data in DB ****/
            _truckContext.Insert(new Truck() { TruckNumber = 10 });
            _truckContext.Insert(new Truck() { TruckNumber = 11 });
            _truckContext.Insert(new Truck() { TruckNumber = 12 });

            /*****Create PartCatalog mock class that represents data in DB and add 3 general parts*****/
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Front Tire", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Passenger-Rear Tire", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Check Engine Light", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            //Add 3 light parts
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Brake Light", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Passenger-Parking Light", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Headlamp", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            //add 3 fluid parts
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Engine Oil", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Transmission Oil", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Brake fluid", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });

            //Create ViewModel Mock that represents data coming from View
            var truckInspectionObjectFromViewWithDropDownAndCheckBoxListPopulated = _truckInspectionService.PopulateDriversTrucksAndPartsCatalog(new TruckInspectionViewModel()
            {
                DriverId = "1",
                TruckId = "10",
                CurrentMileage = "900",
                LastMileageReported = 0,
                PartsReportedForeignKeyId = "1234",
                LastTimeAReportWasSubmitted = DateTime.Now.Date,
                //Populate of DropDownLists and Checkboxlists is done by the PopulateDriversTrucksAndPartsCatalog method
                Comments = "Few issues on this truck"
            });
            
            //Act
            _truckInspectionService.CreateTruckInspection(truckInspectionObjectFromViewWithDropDownAndCheckBoxListPopulated);
            var result = _partsInspectedContext.Collection().ToList();
            //Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Test_CurrentMileageReportedIsLesserThanPreviousSubmittedAndInspectionHasBeenSubmittedForThisTruckToday_InspectionCommentsNOTAddedToDriverCommentsTable()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            IRepository<TruckInspection> _truckInspectionContext = new MockTruckCheckUpContext<TruckInspection>();
            IRepository<Driver> _driverContext = new MockTruckCheckUpContext<Driver>();
            IRepository<Truck> _truckContext = new MockTruckCheckUpContext<Truck>();
            IRepository<PartsInspected> _partsInspectedContext = new MockTruckCheckUpContext<PartsInspected>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _partsInspectedContext, _logger);

            /****Create a Mock TruckInspection object that represents current data in DB ****/
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now, DriverId = "1", TruckId = "10", Mileage = 1000});
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "11", Mileage = 1000});
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "12", Mileage = 1000});

            /****Create a Mock Driver object that represents current data in DB****/
            _driverContext.Insert(new Driver() { FirstName = "Juan", LastName = "Gonzalez" });
            _driverContext.Insert(new Driver() { FirstName = "Jose", LastName = "Morales" });
            _driverContext.Insert(new Driver() { FirstName = "Jesus", LastName = "Ramos" });

            /****Create a Mock Truck object that represents current data in DB ****/
            _truckContext.Insert(new Truck() { TruckNumber = 10 });
            _truckContext.Insert(new Truck() { TruckNumber = 11 });
            _truckContext.Insert(new Truck() { TruckNumber = 12 });

            /*****Create PartCatalog mock class that represents data in DB and add 3 general parts*****/
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Front Tire", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Passenger-Rear Tire", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Check Engine Light", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            //Add 3 light parts
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Brake Light", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Passenger-Parking Light", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Headlamp", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            //add 3 fluid parts
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Engine Oil", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Transmission Oil", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Brake fluid", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });

            //Create ViewModel Mock that represents data coming from View
            var truckInspectionObjectFromViewWithDropDownAndCheckBoxListPopulated = _truckInspectionService.PopulateDriversTrucksAndPartsCatalog(new TruckInspectionViewModel()
            {
                DriverId = "1",
                TruckId = "10",
                CurrentMileage = "900",
                LastMileageReported = 0,
                PartsReportedForeignKeyId = "1234",
                LastTimeAReportWasSubmitted = DateTime.Now.Date,
                //Populate of DropDownLists and Checkboxlists is done by the PopulateDriversTrucksAndPartsCatalog method
                Comments = "Few issues on this truck"
            });
            
            //Act
            _truckInspectionService.CreateTruckInspection(truckInspectionObjectFromViewWithDropDownAndCheckBoxListPopulated);
            var result = _partsInspectedContext.Collection().ToList();
            //Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Test_CreateNewTruckInspectionObject_ReturnsTruckInspectionViewModelObject()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            IRepository<TruckInspection> _truckInspectionContext = new MockTruckCheckUpContext<TruckInspection>();
            IRepository<Driver> _driverContext = new MockTruckCheckUpContext<Driver>();
            IRepository<Truck> _truckContext = new MockTruckCheckUpContext<Truck>();
            IRepository<PartsInspected> _partsInspectedContext = new MockTruckCheckUpContext<PartsInspected>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _partsInspectedContext, _logger);
            bool expected = true;
            bool actual = false;
            //Act
            //Check that a TruckInspectionViewModel object is return by CreateNewTruckInspectionObject function
            if (_truckInspectionService.CreateNewTruckInspectionObject() is TruckInspectionViewModel)
            {
                actual = true;
            }
            //Assert
            Assert.AreEqual(expected, actual);
        }

        /* Behavioral unit test for object that has to be returned to controller */
        [TestMethod]
        public void Test_CreateNewTruckInspectionObject_ReturnsListOfDriversLoadedIntoDriverListDropDownList()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            IRepository<TruckInspection> _truckInspectionContext = new MockTruckCheckUpContext<TruckInspection>();
            IRepository<Driver> _driverContext = new MockTruckCheckUpContext<Driver>();
            IRepository<Truck> _truckContext = new MockTruckCheckUpContext<Truck>();
            IRepository<PartsInspected> _partsInspectedContext = new MockTruckCheckUpContext<PartsInspected>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _partsInspectedContext, _logger);
            
            /****Create some Mock Driver objects that represents current data in DB****/
            _driverContext.Insert(new Driver() { FirstName = "Juan", LastName = "Gonzalez" });
            _driverContext.Insert(new Driver() { FirstName = "Jose", LastName = "Morales" });
            _driverContext.Insert(new Driver() { FirstName = "Jesus", LastName = "Ramos" });

            //Act
            //Check that a TruckInspectionViewModel object contains a list of drivers list
            var result = _truckInspectionService.CreateNewTruckInspectionObject();

            //Assert
            Assert.AreEqual(3, result.DriverList.Count);
        }

        /* Behavioral unit test for object that has to be returned to controller */
        [TestMethod]
        public void Test_CreateNewTruckInspectionObject_ReturnsListOfTrucksLoadedIntoTruckListDropDownList()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            IRepository<TruckInspection> _truckInspectionContext = new MockTruckCheckUpContext<TruckInspection>();
            IRepository<Driver> _driverContext = new MockTruckCheckUpContext<Driver>();
            IRepository<Truck> _truckContext = new MockTruckCheckUpContext<Truck>();
            IRepository<PartsInspected> _partsInspectedContext = new MockTruckCheckUpContext<PartsInspected>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _partsInspectedContext, _logger);

            /****Create a Mock Truck object that represents current data in DB ****/
            _truckContext.Insert(new Truck() { TruckNumber = 10 });
            _truckContext.Insert(new Truck() { TruckNumber = 11 });
            _truckContext.Insert(new Truck() { TruckNumber = 12 });

            //Act
            //Check that a TruckInspectionViewModel object contains a list of drivers list
            var result = _truckInspectionService.CreateNewTruckInspectionObject();

            //Assert
            Assert.AreEqual(3, result.TruckList.Count);
        }

        /* Behavioral unit test for object that has to be returned to controller */
        [TestMethod]
        public void Test_CreateNewTruckInspectionObject_ReturnsListOfGeneralPartsLoadedIntoGeneralPartsCheckBoxList()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            IRepository<TruckInspection> _truckInspectionContext = new MockTruckCheckUpContext<TruckInspection>();
            IRepository<Driver> _driverContext = new MockTruckCheckUpContext<Driver>();
            IRepository<Truck> _truckContext = new MockTruckCheckUpContext<Truck>();
            IRepository<PartsInspected> _partsInspectedContext = new MockTruckCheckUpContext<PartsInspected>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _partsInspectedContext, _logger);

            /*****Create PartCatalog mock class that represents data in DB and add 3 general parts*****/
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Front Tire", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Passenger-Rear Tire", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Check Engine Light", PartCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8" });

            //Act
            //Check that a TruckInspectionViewModel object contains a list of drivers list
            var result = _truckInspectionService.CreateNewTruckInspectionObject();

            //Assert
            Assert.AreEqual(3, result.GeneralCatalog.Count);
        }


        /* Behavioral unit test for object that has to be returned to controller */
        [TestMethod]
        public void Test_CreateNewTruckInspectionObject_ReturnsListOfLightPartsLoadedIntoLightPartsCheckBoxList()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            IRepository<TruckInspection> _truckInspectionContext = new MockTruckCheckUpContext<TruckInspection>();
            IRepository<Driver> _driverContext = new MockTruckCheckUpContext<Driver>();
            IRepository<Truck> _truckContext = new MockTruckCheckUpContext<Truck>();
            IRepository<PartsInspected> _partsInspectedContext = new MockTruckCheckUpContext<PartsInspected>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _partsInspectedContext, _logger);

            //Add 3 light parts
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Brake Light", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Passenger-Parking Light", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Driver-Headlamp", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });

            //Act
            //Check that a TruckInspectionViewModel object contains a list of drivers list
            var result = _truckInspectionService.CreateNewTruckInspectionObject();

            //Assert
            Assert.AreEqual(3, result.LightsCatalog.Count);
        }

        /* Behavioral unit test for object that has to be returned to controller */
        [TestMethod]
        public void Test_CreateNewTruckInspectionObject_ReturnsListOfFluidPartsLoadedIntoFluidPartsCheckBoxList()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            IRepository<TruckInspection> _truckInspectionContext = new MockTruckCheckUpContext<TruckInspection>();
            IRepository<Driver> _driverContext = new MockTruckCheckUpContext<Driver>();
            IRepository<Truck> _truckContext = new MockTruckCheckUpContext<Truck>();
            IRepository<PartsInspected> _partsInspectedContext = new MockTruckCheckUpContext<PartsInspected>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _partsInspectedContext, _logger);

            //add 3 fluid parts
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Engine Oil", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Transmission Oil", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });
            _partCatalogContext.Insert(new PartCatalog() { PartName = "Brake fluid", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });

            //Act
            //Check that a TruckInspectionViewModel object contains a list of drivers list
            var result = _truckInspectionService.CreateNewTruckInspectionObject();

            //Assert
            Assert.AreEqual(3, result.FluidsCatalog.Count);
        }

        [TestMethod]
        public void Test_PopulateDriversTrucksAndPartsCatalog_ReturnsTruckInspectionViewModelObject()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            IRepository<TruckInspection> _truckInspectionContext = new MockTruckCheckUpContext<TruckInspection>();
            IRepository<Driver> _driverContext = new MockTruckCheckUpContext<Driver>();
            IRepository<Truck> _truckContext = new MockTruckCheckUpContext<Truck>();
            IRepository<PartsInspected> _partsInspectedContext = new MockTruckCheckUpContext<PartsInspected>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _partsInspectedContext, _logger);
            bool expected = true;
            bool actual = false;
            //Act
            //Check that a TruckInspectionViewModel object is return by PopulateDriversTrucksAndPartsCatalog function
            if (_truckInspectionService.PopulateDriversTrucksAndPartsCatalog(new TruckInspectionViewModel()) is TruckInspectionViewModel)
            {
                actual = true;
            }
            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
