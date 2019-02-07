using System.Collections.Generic;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.ViewModels;
using TruckCheckUp.Core.ViewModels.DriverUI;

namespace TruckCheckUp.Core.Contracts.Services
{
    public interface IDriverService
    {
        List<Driver> RetrieveAllDrivers();
        DriverInsertViewModel CreateNewDriverObject();
        void PostNewDriverToDB(DriverInsertViewModel driver);   
        DriverUpdateViewModel RetrieveDriverDataToUpdate(string driverId);
        void UpdateDriverData(DriverUpdateViewModel driver, string driverId);
        Driver RetrieveDriverToDelete(string driverId);
        void DeleteDriver(string driverId);
    }
}