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
        public void Test_CurrentMileageReportedIsGreaterThanPreviousSubmittedAndInspectionHasNOTBeenSubmittedForThisTruckToday_InspectionReportAdded()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            IRepository<TruckInspection> _truckInspectionContext = new MockTruckCheckUpContext<TruckInspection>();
            IRepository<Driver> _driverContext = new MockTruckCheckUpContext<Driver>();
            IRepository<Truck> _truckContext = new MockTruckCheckUpContext<Truck>();
            IRepository<DriverComment> _driverCommentContext = new MockTruckCheckUpContext<DriverComment>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionServiceExtensionMethods _extensionMethods = new TruckInspectionServiceExtensionMethods();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _driverCommentContext, _logger, _extensionMethods);
            
            /****Create a Mock TruckInspection object that represents current data in DB ****/
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId ="10", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false});
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "11", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "12", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });

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
            var TruckInspectionObjectFromView = new TruckInspectionViewModel()
            {
                DriverId = "1",
                TruckId = "10",
                CurrentMileage = 2000,
                LastMileageReported = 0,
                TicketNumber = 0,
                LastTimeAReportWasSubmitted = DateTime.Now.Date,
                DriverList = _extensionMethods.ConvertDriverNamesToDropDownListView(_driverContext.Collection().OrderBy(n => n.FirstName).ToList()),
                TruckList = _extensionMethods.ConvertTruckNumbersToDropDownListView(_truckContext.Collection().OrderBy(t => t.TruckNumber).ToList()),
                GeneralCatalog = _extensionMethods.ConvertGeneralCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(g => g.PartCategoryId == "607412fa-729b-4dfd-94ee-9a8d84d1eed8").OrderBy(p => p.PartName).ToList()),
                LightsCatalog = _extensionMethods.ConvertLightsCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(l => l.PartCategoryId == "43d29546-83c4-4a92-bafe-262aa58d6496").OrderBy(c => c.PartName).ToList()),
                FluidsCatalog = _extensionMethods.ConvertFluidsCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(f => f.PartCategoryId == "3897c106-7357-4ab8-b594-02020a7ce9c6").OrderBy(c => c.PartName).ToList()),
                Comments = "Few issues on this truck"
            };
            //Act
            _truckInspectionService.CreateTruckInspection(TruckInspectionObjectFromView);
            var result = _truckInspectionContext.Collection().ToList();
            //Assert
            Assert.AreEqual(12, result.Count);
        }

        [TestMethod]
        public void Test_CurrentMileageReportedIsGreaterThanPreviousSubmittedAndInspectionHasBeenSubmittedForThisTruckToday_InspectionReportNOTAdded()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            IRepository<TruckInspection> _truckInspectionContext = new MockTruckCheckUpContext<TruckInspection>();
            IRepository<Driver> _driverContext = new MockTruckCheckUpContext<Driver>();
            IRepository<Truck> _truckContext = new MockTruckCheckUpContext<Truck>();
            IRepository<DriverComment> _driverCommentContext = new MockTruckCheckUpContext<DriverComment>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionServiceExtensionMethods _extensionMethods = new TruckInspectionServiceExtensionMethods();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _driverCommentContext, _logger, _extensionMethods);

            /****Create a Mock TruckInspection object that represents current data in DB ****/
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now, DriverId = "1", TruckId = "10", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "11", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "12", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });

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
            var TruckInspectionObjectFromView = new TruckInspectionViewModel()
            {
                DriverId = "1",
                TruckId = "10",
                CurrentMileage = 2000,
                LastMileageReported = 0,
                TicketNumber = 0,
                LastTimeAReportWasSubmitted = DateTime.Now.Date,
                DriverList = _extensionMethods.ConvertDriverNamesToDropDownListView(_driverContext.Collection().OrderBy(n => n.FirstName).ToList()),
                TruckList = _extensionMethods.ConvertTruckNumbersToDropDownListView(_truckContext.Collection().OrderBy(t => t.TruckNumber).ToList()),
                GeneralCatalog = _extensionMethods.ConvertGeneralCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(g => g.PartCategoryId == "607412fa-729b-4dfd-94ee-9a8d84d1eed8").OrderBy(p => p.PartName).ToList()),
                LightsCatalog = _extensionMethods.ConvertLightsCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(l => l.PartCategoryId == "43d29546-83c4-4a92-bafe-262aa58d6496").OrderBy(c => c.PartName).ToList()),
                FluidsCatalog = _extensionMethods.ConvertFluidsCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(f => f.PartCategoryId == "3897c106-7357-4ab8-b594-02020a7ce9c6").OrderBy(c => c.PartName).ToList()),
                Comments = "Few issues on this truck"
            };
            //Act
            _truckInspectionService.CreateTruckInspection(TruckInspectionObjectFromView);
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
            IRepository<DriverComment> _driverCommentContext = new MockTruckCheckUpContext<DriverComment>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionServiceExtensionMethods _extensionMethods = new TruckInspectionServiceExtensionMethods();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _driverCommentContext, _logger, _extensionMethods);

            /****Create a Mock TruckInspection object that represents current data in DB ****/
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "10", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "11", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "12", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });

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
            var TruckInspectionObjectFromView = new TruckInspectionViewModel()
            {
                DriverId = "1",
                TruckId = "10",
                CurrentMileage = 900,
                LastMileageReported = 0,
                TicketNumber = 0,
                LastTimeAReportWasSubmitted = DateTime.Now.Date,
                DriverList = _extensionMethods.ConvertDriverNamesToDropDownListView(_driverContext.Collection().OrderBy(n => n.FirstName).ToList()),
                TruckList = _extensionMethods.ConvertTruckNumbersToDropDownListView(_truckContext.Collection().OrderBy(t => t.TruckNumber).ToList()),
                GeneralCatalog = _extensionMethods.ConvertGeneralCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(g => g.PartCategoryId == "607412fa-729b-4dfd-94ee-9a8d84d1eed8").OrderBy(p => p.PartName).ToList()),
                LightsCatalog = _extensionMethods.ConvertLightsCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(l => l.PartCategoryId == "43d29546-83c4-4a92-bafe-262aa58d6496").OrderBy(c => c.PartName).ToList()),
                FluidsCatalog = _extensionMethods.ConvertFluidsCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(f => f.PartCategoryId == "3897c106-7357-4ab8-b594-02020a7ce9c6").OrderBy(c => c.PartName).ToList()),
                Comments = "Few issues on this truck"
            };
            //Act
            _truckInspectionService.CreateTruckInspection(TruckInspectionObjectFromView);
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
            IRepository<DriverComment> _driverCommentContext = new MockTruckCheckUpContext<DriverComment>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionServiceExtensionMethods _extensionMethods = new TruckInspectionServiceExtensionMethods();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _driverCommentContext, _logger, _extensionMethods);

            /****Create a Mock TruckInspection object that represents current data in DB ****/
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now, DriverId = "1", TruckId = "10", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "11", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "12", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });

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
            var TruckInspectionObjectFromView = new TruckInspectionViewModel()
            {
                DriverId = "1",
                TruckId = "10",
                CurrentMileage = 900,
                LastMileageReported = 0,
                TicketNumber = 0,
                LastTimeAReportWasSubmitted = DateTime.Now.Date,
                DriverList = _extensionMethods.ConvertDriverNamesToDropDownListView(_driverContext.Collection().OrderBy(n => n.FirstName).ToList()),
                TruckList = _extensionMethods.ConvertTruckNumbersToDropDownListView(_truckContext.Collection().OrderBy(t => t.TruckNumber).ToList()),
                GeneralCatalog = _extensionMethods.ConvertGeneralCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(g => g.PartCategoryId == "607412fa-729b-4dfd-94ee-9a8d84d1eed8").OrderBy(p => p.PartName).ToList()),
                LightsCatalog = _extensionMethods.ConvertLightsCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(l => l.PartCategoryId == "43d29546-83c4-4a92-bafe-262aa58d6496").OrderBy(c => c.PartName).ToList()),
                FluidsCatalog = _extensionMethods.ConvertFluidsCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(f => f.PartCategoryId == "3897c106-7357-4ab8-b594-02020a7ce9c6").OrderBy(c => c.PartName).ToList()),
                Comments = "Few issues on this truck"
            };
            //Act
            _truckInspectionService.CreateTruckInspection(TruckInspectionObjectFromView);
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
            IRepository<DriverComment> _driverCommentContext = new MockTruckCheckUpContext<DriverComment>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionServiceExtensionMethods _extensionMethods = new TruckInspectionServiceExtensionMethods();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _driverCommentContext, _logger, _extensionMethods);

            /****Create a Mock TruckInspection object that represents current data in DB ****/
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "10", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "11", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "12", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });

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
            var TruckInspectionObjectFromView = new TruckInspectionViewModel()
            {
                DriverId = "1",
                TruckId = "10",
                CurrentMileage = 2000,
                LastMileageReported = 0,
                TicketNumber = 0,
                LastTimeAReportWasSubmitted = DateTime.Now.Date,
                DriverList = _extensionMethods.ConvertDriverNamesToDropDownListView(_driverContext.Collection().OrderBy(n => n.FirstName).ToList()),
                TruckList = _extensionMethods.ConvertTruckNumbersToDropDownListView(_truckContext.Collection().OrderBy(t => t.TruckNumber).ToList()),
                GeneralCatalog = _extensionMethods.ConvertGeneralCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(g => g.PartCategoryId == "607412fa-729b-4dfd-94ee-9a8d84d1eed8").OrderBy(p => p.PartName).ToList()),
                LightsCatalog = _extensionMethods.ConvertLightsCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(l => l.PartCategoryId == "43d29546-83c4-4a92-bafe-262aa58d6496").OrderBy(c => c.PartName).ToList()),
                FluidsCatalog = _extensionMethods.ConvertFluidsCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(f => f.PartCategoryId == "3897c106-7357-4ab8-b594-02020a7ce9c6").OrderBy(c => c.PartName).ToList()),
                Comments = "Few issues on this truck"
            };
            //Act
            var result = _truckInspectionService.CreateTruckInspection(TruckInspectionObjectFromView);
         
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
            IRepository<DriverComment> _driverCommentContext = new MockTruckCheckUpContext<DriverComment>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionServiceExtensionMethods _extensionMethods = new TruckInspectionServiceExtensionMethods();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _driverCommentContext, _logger, _extensionMethods);

            /****Create a Mock TruckInspection object that represents current data in DB ****/
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now, DriverId = "1", TruckId = "10", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "11", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "12", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });

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
            var TruckInspectionObjectFromView = new TruckInspectionViewModel()
            {
                DriverId = "1",
                TruckId = "10",
                CurrentMileage = 2000,
                LastMileageReported = 0,
                TicketNumber = 0,
                LastTimeAReportWasSubmitted = DateTime.Now.Date,
                DriverList = _extensionMethods.ConvertDriverNamesToDropDownListView(_driverContext.Collection().OrderBy(n => n.FirstName).ToList()),
                TruckList = _extensionMethods.ConvertTruckNumbersToDropDownListView(_truckContext.Collection().OrderBy(t => t.TruckNumber).ToList()),
                GeneralCatalog = _extensionMethods.ConvertGeneralCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(g => g.PartCategoryId == "607412fa-729b-4dfd-94ee-9a8d84d1eed8").OrderBy(p => p.PartName).ToList()),
                LightsCatalog = _extensionMethods.ConvertLightsCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(l => l.PartCategoryId == "43d29546-83c4-4a92-bafe-262aa58d6496").OrderBy(c => c.PartName).ToList()),
                FluidsCatalog = _extensionMethods.ConvertFluidsCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(f => f.PartCategoryId == "3897c106-7357-4ab8-b594-02020a7ce9c6").OrderBy(c => c.PartName).ToList()),
                Comments = "Few issues on this truck"
            };
            //Act
            var result = _truckInspectionService.CreateTruckInspection(TruckInspectionObjectFromView);
            
            //Assert
            Assert.AreEqual("danger", result.AlertStyle);
        }

        [TestMethod]
        public void Test_CurrentMileageReportedIsGreaterThanPreviousSubmittedAndInspectionHasNOTBeenSubmittedForThisTruckToday_InspectionCommentsAddedToDriverCommentsTable()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            IRepository<TruckInspection> _truckInspectionContext = new MockTruckCheckUpContext<TruckInspection>();
            IRepository<Driver> _driverContext = new MockTruckCheckUpContext<Driver>();
            IRepository<Truck> _truckContext = new MockTruckCheckUpContext<Truck>();
            IRepository<DriverComment> _driverCommentContext = new MockTruckCheckUpContext<DriverComment>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionServiceExtensionMethods _extensionMethods = new TruckInspectionServiceExtensionMethods();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _driverCommentContext, _logger, _extensionMethods);

            /****Create a Mock TruckInspection object that represents current data in DB ****/
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "10", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "11", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "12", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });

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
            var TruckInspectionObjectFromView = new TruckInspectionViewModel()
            {
                DriverId = "1",
                TruckId = "10",
                CurrentMileage = 2000,
                LastMileageReported = 0,
                TicketNumber = 0,
                LastTimeAReportWasSubmitted = DateTime.Now.Date,
                DriverList = _extensionMethods.ConvertDriverNamesToDropDownListView(_driverContext.Collection().OrderBy(n => n.FirstName).ToList()),
                TruckList = _extensionMethods.ConvertTruckNumbersToDropDownListView(_truckContext.Collection().OrderBy(t => t.TruckNumber).ToList()),
                GeneralCatalog = _extensionMethods.ConvertGeneralCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(g => g.PartCategoryId == "607412fa-729b-4dfd-94ee-9a8d84d1eed8").OrderBy(p => p.PartName).ToList()),
                LightsCatalog = _extensionMethods.ConvertLightsCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(l => l.PartCategoryId == "43d29546-83c4-4a92-bafe-262aa58d6496").OrderBy(c => c.PartName).ToList()),
                FluidsCatalog = _extensionMethods.ConvertFluidsCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(f => f.PartCategoryId == "3897c106-7357-4ab8-b594-02020a7ce9c6").OrderBy(c => c.PartName).ToList()),
                Comments = "Few issues on this truck"
            };
            //Act
            _truckInspectionService.CreateTruckInspection(TruckInspectionObjectFromView);
            var result = _driverCommentContext.Collection().ToList();
            //Assert
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void Test_CurrentMileageReportedIsGreaterThanPreviousSubmittedAndInspectionHasBeenSubmittedForThisTruckToday_InspectionCommentsNOTAddedToDriverCommentsTable()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            IRepository<TruckInspection> _truckInspectionContext = new MockTruckCheckUpContext<TruckInspection>();
            IRepository<Driver> _driverContext = new MockTruckCheckUpContext<Driver>();
            IRepository<Truck> _truckContext = new MockTruckCheckUpContext<Truck>();
            IRepository<DriverComment> _driverCommentContext = new MockTruckCheckUpContext<DriverComment>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionServiceExtensionMethods _extensionMethods = new TruckInspectionServiceExtensionMethods();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _driverCommentContext, _logger, _extensionMethods);

            /****Create a Mock TruckInspection object that represents current data in DB ****/
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now, DriverId = "1", TruckId = "10", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "11", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "12", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });

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
            var TruckInspectionObjectFromView = new TruckInspectionViewModel()
            {
                DriverId = "1",
                TruckId = "10",
                CurrentMileage = 2000,
                LastMileageReported = 0,
                TicketNumber = 0,
                LastTimeAReportWasSubmitted = DateTime.Now.Date,
                DriverList = _extensionMethods.ConvertDriverNamesToDropDownListView(_driverContext.Collection().OrderBy(n => n.FirstName).ToList()),
                TruckList = _extensionMethods.ConvertTruckNumbersToDropDownListView(_truckContext.Collection().OrderBy(t => t.TruckNumber).ToList()),
                GeneralCatalog = _extensionMethods.ConvertGeneralCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(g => g.PartCategoryId == "607412fa-729b-4dfd-94ee-9a8d84d1eed8").OrderBy(p => p.PartName).ToList()),
                LightsCatalog = _extensionMethods.ConvertLightsCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(l => l.PartCategoryId == "43d29546-83c4-4a92-bafe-262aa58d6496").OrderBy(c => c.PartName).ToList()),
                FluidsCatalog = _extensionMethods.ConvertFluidsCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(f => f.PartCategoryId == "3897c106-7357-4ab8-b594-02020a7ce9c6").OrderBy(c => c.PartName).ToList()),
                Comments = "Few issues on this truck"
            };
            //Act
            _truckInspectionService.CreateTruckInspection(TruckInspectionObjectFromView);
            var result = _driverCommentContext.Collection().ToList();
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
            IRepository<DriverComment> _driverCommentContext = new MockTruckCheckUpContext<DriverComment>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionServiceExtensionMethods _extensionMethods = new TruckInspectionServiceExtensionMethods();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _driverCommentContext, _logger, _extensionMethods);

            /****Create a Mock TruckInspection object that represents current data in DB ****/
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "10", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "11", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "12", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });

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
            var TruckInspectionObjectFromView = new TruckInspectionViewModel()
            {
                DriverId = "1",
                TruckId = "10",
                CurrentMileage = 900,
                LastMileageReported = 0,
                TicketNumber = 0,
                LastTimeAReportWasSubmitted = DateTime.Now.Date,
                DriverList = _extensionMethods.ConvertDriverNamesToDropDownListView(_driverContext.Collection().OrderBy(n => n.FirstName).ToList()),
                TruckList = _extensionMethods.ConvertTruckNumbersToDropDownListView(_truckContext.Collection().OrderBy(t => t.TruckNumber).ToList()),
                GeneralCatalog = _extensionMethods.ConvertGeneralCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(g => g.PartCategoryId == "607412fa-729b-4dfd-94ee-9a8d84d1eed8").OrderBy(p => p.PartName).ToList()),
                LightsCatalog = _extensionMethods.ConvertLightsCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(l => l.PartCategoryId == "43d29546-83c4-4a92-bafe-262aa58d6496").OrderBy(c => c.PartName).ToList()),
                FluidsCatalog = _extensionMethods.ConvertFluidsCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(f => f.PartCategoryId == "3897c106-7357-4ab8-b594-02020a7ce9c6").OrderBy(c => c.PartName).ToList()),
                Comments = "Few issues on this truck"
            };
            //Act
            _truckInspectionService.CreateTruckInspection(TruckInspectionObjectFromView);
            var result = _driverCommentContext.Collection().ToList();
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
            IRepository<DriverComment> _driverCommentContext = new MockTruckCheckUpContext<DriverComment>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionServiceExtensionMethods _extensionMethods = new TruckInspectionServiceExtensionMethods();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _driverCommentContext, _logger, _extensionMethods);

            /****Create a Mock TruckInspection object that represents current data in DB ****/
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now, DriverId = "1", TruckId = "10", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "11", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });
            _truckInspectionContext.Insert(new TruckInspection() { CreationDate = DateTime.Now.AddDays(-1), DriverId = "1", TruckId = "12", TicketNumber = 1, Mileage = 1000, PartCatalogId = "1", IsOK = false });

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
            var TruckInspectionObjectFromView = new TruckInspectionViewModel()
            {
                DriverId = "1",
                TruckId = "10",
                CurrentMileage = 900,
                LastMileageReported = 0,
                TicketNumber = 0,
                LastTimeAReportWasSubmitted = DateTime.Now.Date,
                DriverList = _extensionMethods.ConvertDriverNamesToDropDownListView(_driverContext.Collection().OrderBy(n => n.FirstName).ToList()),
                TruckList = _extensionMethods.ConvertTruckNumbersToDropDownListView(_truckContext.Collection().OrderBy(t => t.TruckNumber).ToList()),
                GeneralCatalog = _extensionMethods.ConvertGeneralCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(g => g.PartCategoryId == "607412fa-729b-4dfd-94ee-9a8d84d1eed8").OrderBy(p => p.PartName).ToList()),
                LightsCatalog = _extensionMethods.ConvertLightsCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(l => l.PartCategoryId == "43d29546-83c4-4a92-bafe-262aa58d6496").OrderBy(c => c.PartName).ToList()),
                FluidsCatalog = _extensionMethods.ConvertFluidsCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(f => f.PartCategoryId == "3897c106-7357-4ab8-b594-02020a7ce9c6").OrderBy(c => c.PartName).ToList()),
                Comments = "Few issues on this truck"
            };
            //Act
            _truckInspectionService.CreateTruckInspection(TruckInspectionObjectFromView);
            var result = _driverCommentContext.Collection().ToList();
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
            IRepository<DriverComment> _driverCommentContext = new MockTruckCheckUpContext<DriverComment>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionServiceExtensionMethods _extensionMethods = new TruckInspectionServiceExtensionMethods();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _driverCommentContext, _logger, _extensionMethods);
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

        [TestMethod]
        public void Test_PopulateDriversTrucksAndPartsCatalog_ReturnsTruckInspectionViewModelObject()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            IRepository<TruckInspection> _truckInspectionContext = new MockTruckCheckUpContext<TruckInspection>();
            IRepository<Driver> _driverContext = new MockTruckCheckUpContext<Driver>();
            IRepository<Truck> _truckContext = new MockTruckCheckUpContext<Truck>();
            IRepository<DriverComment> _driverCommentContext = new MockTruckCheckUpContext<DriverComment>();
            ILogger _logger = new MockTruckCheckUpLogger();
            ITruckInspectionServiceExtensionMethods _extensionMethods = new TruckInspectionServiceExtensionMethods();
            ITruckInspectionService _truckInspectionService = new TruckInspectionService(_partCatalogContext, _truckInspectionContext, _driverContext, _truckContext, _driverCommentContext, _logger, _extensionMethods);
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

        [TestMethod]
        public void Test_PopulateDriverNamesDropDownListViewModel_DriverNamesAddedSuccesfully()
        {
            //Arrange
            IRepository<Driver> _driverContext = new MockTruckCheckUpContext<Driver>();
            ITruckInspectionServiceExtensionMethods _extensionMethods = new TruckInspectionServiceExtensionMethods();
            //Add a few records to driver mock class
            _driverContext.Insert(new Driver() {FirstName = "Bella", LastName = "Smith" });
            _driverContext.Insert(new Driver() { FirstName = "Edward", LastName = "Collen" });
            _driverContext.Insert(new Driver() { FirstName = "Jake", LastName = "Wolf" });
            //Get all drivers in mock class
            var driversList = _driverContext.Collection().OrderBy(n => n.FirstName).ToList();
            //Act
            //Move list of trucks to a DropdownListView Object
            var driverNamesDropDownListViewModel = _extensionMethods.ConvertDriverNamesToDropDownListView(driversList);
            //Assert
            Assert.AreEqual(3, driverNamesDropDownListViewModel.Count);
        }

        [TestMethod]
        public void Test_PopulateDriverNamesDropDownListViewModel_EmptyListReturned()
        {
            //Arrange
            IRepository<Driver> _driverContext = new MockTruckCheckUpContext<Driver>();
            ITruckInspectionServiceExtensionMethods _extensionMethods = new TruckInspectionServiceExtensionMethods();
            //Get all drivers in mock class
            var driversList = _driverContext.Collection().OrderBy(n => n.FirstName).ToList();
            //Act
            //Move list of trucks to a DropdownListView Object
            var driverNamesDropDownListViewModel = _extensionMethods.ConvertDriverNamesToDropDownListView(driversList);
            //Assert            
            Assert.AreEqual(0, driverNamesDropDownListViewModel.Count);
        }

        [TestMethod]
        public void Test_PopulateTruckNumberDropDownListViewModel_TruckNumbersAddedSuccesfully()
        {
            //Arrange
            IRepository<Truck> _truckContext = new MockTruckCheckUpContext<Truck>();
            ITruckInspectionServiceExtensionMethods _extensionMethods = new TruckInspectionServiceExtensionMethods();
            //Add a few records to truck mock class
            _truckContext.Insert(new Truck(){ TruckNumber = 1});
            _truckContext.Insert(new Truck() { TruckNumber = 2 });
            _truckContext.Insert(new Truck() { TruckNumber = 3 });
            //Get all trucks in mock class
            var truckList = _truckContext.Collection().OrderBy(t => t.TruckNumber).ToList();
            //Act
            //Move list of trucks to a DropdownListView Object
            var truckNumberDropDownListViewModel = _extensionMethods.ConvertTruckNumbersToDropDownListView(truckList);
            //Assert
            Assert.AreEqual(3, truckNumberDropDownListViewModel.Count);
        }

        [TestMethod]
        public void Test_PopulateTruckNumberDropDownListViewModel_EmptyListReturned()
        {
            //Arrange
            IRepository<Truck> _truckContext = new MockTruckCheckUpContext<Truck>();
            ITruckInspectionServiceExtensionMethods _extensionMethods = new TruckInspectionServiceExtensionMethods();
           
            var truckList = _truckContext.Collection().OrderBy(t => t.TruckNumber).ToList();
            //Act
            var truckNumberDropDownListViewModel = _extensionMethods.ConvertTruckNumbersToDropDownListView(truckList);
            //Assert
            Assert.AreEqual(0, truckNumberDropDownListViewModel.Count);
        }

        [TestMethod]
        public void Test_PopulateGeneralCatalogCheckBoxListViewModel_EmptyGeneralListReturned()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            ITruckInspectionServiceExtensionMethods _extensionMethods = new TruckInspectionServiceExtensionMethods();
            //Generate the list that will be passed as a parameter to method that converts to CheckListViewModel
            var generalCatalog = _partCatalogContext.Collection().Where(f => f.PartCategoryId == "607412fa-729b-4dfd-94ee-9a8d84d1eed8").OrderBy(c => c.PartName).ToList();

            //Act
            var generalCheckBoxList = _extensionMethods.ConvertLightsCatalogToCheckBoxListView(generalCatalog);
            //Assert
            //Results are the same for both list (PartCatalog List and CheckBoxListViewModel)
            Assert.AreEqual(0, generalCheckBoxList.Count);
        }


        [TestMethod]
        public void Test_PopulateLightsCatalogCheckBoxListViewModel_LightItemsAddedSuccesfully()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            ITruckInspectionServiceExtensionMethods _extensionMethods = new TruckInspectionServiceExtensionMethods();

            //Add a few records to parts catalog mock class
            _partCatalogContext.Insert(
            new PartCatalog() { PartName = "Break Light-Driver", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            _partCatalogContext.Insert(
            new PartCatalog() { PartName = "Parking Light-Driver", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            _partCatalogContext.Insert(
            new PartCatalog() { PartName = "Turn Signal-Passenger", PartCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496" });
            //Generate the list that will be passed as a parameter to method that converts to CheckListViewModel
            var lightsCatalog = _partCatalogContext.Collection().Where(f => f.PartCategoryId == "43d29546-83c4-4a92-bafe-262aa58d6496").OrderBy(c => c.PartName).ToList();

            //Act
            var lightsCheckBoxList = _extensionMethods.ConvertLightsCatalogToCheckBoxListView(lightsCatalog);
            //Assert
            //Results are the same for both list (PartCatalog List and CheckBoxListViewModel)
            Assert.AreEqual(3, lightsCheckBoxList.Count);
        }

        [TestMethod]
        public void Test_PopulateLightsCatalogCheckBoxListViewModel_EmptyLightListReturned()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            ITruckInspectionServiceExtensionMethods _extensionMethods = new TruckInspectionServiceExtensionMethods();
            //Generate the list that will be passed as a parameter to method that converts to CheckListViewModel
            var lightsCatalog = _partCatalogContext.Collection().Where(f => f.PartCategoryId == "43d29546-83c4-4a92-bafe-262aa58d6496").OrderBy(c => c.PartName).ToList();

            //Act
            var lightsCheckBoxList = _extensionMethods.ConvertFluidsCatalogToCheckBoxListView(lightsCatalog);
            //Assert
            //Results are the same for both list (PartCatalog List and CheckBoxListViewModel)
            Assert.AreEqual(0, lightsCheckBoxList.Count);
        }

        [TestMethod]
        public void Test_PopulateFluidsCatalogCheckBoxListViewModel_FluidItemsAddedSuccesfully()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            ITruckInspectionServiceExtensionMethods _extensionMethods = new TruckInspectionServiceExtensionMethods();

            //Add a few records to parts catalog mock class
            _partCatalogContext.Insert(
            new PartCatalog() { PartName = "Engine Oil", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });
            _partCatalogContext.Insert(
            new PartCatalog() { PartName = "Transmission Oil", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });
            _partCatalogContext.Insert(
            new PartCatalog() { PartName = "Antifreeze/Coolant", PartCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6" });
            //Generate the list that will be passed as a parameter to method that converts to CheckListViewModel
            var fluidsCatalog = _partCatalogContext.Collection().Where(f => f.PartCategoryId == "3897c106-7357-4ab8-b594-02020a7ce9c6").OrderBy(c => c.PartName).ToList();

            //Act
            var fluidsCheckBoxList = _extensionMethods.ConvertFluidsCatalogToCheckBoxListView(fluidsCatalog);
            //Assert
            //Results are the same for both list (PartCatalog List and CheckBoxListViewModel)
            Assert.AreEqual(3, fluidsCheckBoxList.Count);
        }

        [TestMethod]
        public void Test_PopulateFluidsCatalogCheckBoxListViewModel_EmptyFluidListReturned()
        {
            //Arrange
            IRepository<PartCatalog> _partCatalogContext = new MockTruckCheckUpContext<PartCatalog>();
            ITruckInspectionServiceExtensionMethods _extensionMethods = new TruckInspectionServiceExtensionMethods();
            //Generate the list that will be passed as a parameter to method that converts to CheckListViewModel
            var fluidsCatalog = _partCatalogContext.Collection().Where(f => f.PartCategoryId == "3897c106-7357-4ab8-b594-02020a7ce9c6").OrderBy(c => c.PartName).ToList();

            //Act
            var fluidsCheckBoxList = _extensionMethods.ConvertFluidsCatalogToCheckBoxListView(fluidsCatalog);
            //Assert
            //Results are the same for both list (PartCatalog List and CheckBoxListViewModel)
            Assert.AreEqual(0, fluidsCheckBoxList.Count);
        }
    }
}
