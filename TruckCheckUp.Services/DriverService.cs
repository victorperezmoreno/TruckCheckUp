using System.Collections.Generic;
using System.Linq;
using TruckCheckUp.Core.Contracts;
using TruckCheckUp.Core.Contracts.DataAccess;
using TruckCheckUp.Core.Contracts.Services;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.ViewModels;
using TruckCheckUp.Core.ViewModels.DriverUI;

namespace TruckCheckUp.Services
{
    public class DriverService : IDriverService
    {
        private IRepository<Driver> _driverContext;

        public DriverService(IRepository<Driver> driverContext)
        {
            _driverContext = driverContext;
        }

        // GET: Driver
        public List<Driver> RetrieveAllDrivers()
        {
            //Get a list of drivers
            var driversRetrieved = _driverContext.Collection().ToList();

            return driversRetrieved;
        }

        public DriverInsertViewModel CreateNewDriverObject()
        {
            var driver = new DriverInsertViewModel();

            return driver;
        }

        public void PostNewDriverToDB(DriverInsertViewModel driver)
        {
            var driverToInsert = new Driver();
            driverToInsert.FirstName = driver.FirstName;
            driverToInsert.LastName = driver.LastName;
            driverToInsert.Status = driver.Status; 

            _driverContext.Insert(driverToInsert);
            _driverContext.Commit();
        }

        public DriverUpdateViewModel RetrieveDriverDataToUpdate(string driverId)
        {
            //Look for driver and return her data
            var driverToUpdate = (
                    from driverStoredInDB in _driverContext.Collection()
                    where driverStoredInDB.Id == driverId
                    select new DriverUpdateViewModel()
                    {
                        Id = driverStoredInDB.Id,
                        FirstName = driverStoredInDB.FirstName,
                        LastName = driverStoredInDB.LastName,
                        Status = driverStoredInDB.Status
                    }).FirstOrDefault();

            if (driverToUpdate != null)
            {
                return driverToUpdate;
            }
            else
            {
                return new DriverUpdateViewModel();
            }
        }

        public void UpdateDriverData(DriverUpdateViewModel driver, string driverId)
        {
            var driverToUpdate = _driverContext.Find(driverId);
            if (driverToUpdate != null)
            {
                driverToUpdate.FirstName = driver.FirstName;
                driverToUpdate.LastName = driver.LastName;
                driverToUpdate.Status = driver.Status;

                _driverContext.Commit();
            }
        }

        public Driver RetrieveDriverToDelete(string driverId)
        {
            var driverToDelete = _driverContext.Find(driverId);
            if (driverToDelete != null)
            {
                return driverToDelete;
            }
            else
            {
                return new Driver();
            }
        }

        public void DeleteDriver(string driverId)
        {
            var driverToDelete = _driverContext.Find(driverId);
            if (driverToDelete != null)
            {
                _driverContext.Delete(driverId);
                _driverContext.Commit();
            }
        }
    }
}
