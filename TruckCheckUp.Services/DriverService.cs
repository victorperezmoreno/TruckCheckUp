using System.Collections.Generic;
using System.Linq;
using TruckCheckUp.Core.Contracts;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.ViewModels;
using TruckCheckUp.Core.ViewModels.DriverUI;

namespace TruckCheckUp.Services
{
    public class DriverService
    {
        private IRepository<Driver> _driverContext;

        public DriverService(IRepository<Driver> driverContext)
        {
            _driverContext = driverContext;
        }

        // GET: User
        public List<Driver> ListDrivers()
        {
            //Get a list of drivers
            var users = _driverContext.Collection().ToList();

            return users;
        }

        public DriverInsertViewModel CreateNewDriver()
        {
            var driver = new DriverInsertViewModel();

            return driver;
        }

        public void CreateNewDriverPost(DriverInsertViewModel driver)
        {
            var driverToInsert = new Driver();
            driverToInsert.FirstName = driver.FirstName;
            driverToInsert.LastName = driver.LastName;
            driverToInsert.Status = driver.Status; 

            _driverContext.Insert(driverToInsert);
            _driverContext.Commit();
        }

        public DriverEditViewModel EditDriver(string driverId)
        {
            //Look for driver and return her data
            var driver = (
                    from d in _driverContext.Collection()
                    where d.Id == driverId
                    select new DriverEditViewModel()
                    {
                        Id = d.Id,
                        FirstName = d.FirstName,
                        LastName = d.LastName,
                        Status = d.Status
                    }).FirstOrDefault();

            if (driver != null)
            {
                return driver;
            }
            else
            {
                return new DriverEditViewModel();
            }
        }

        public void ConfirmEditDriver(DriverEditViewModel driver, string driverId)
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

        public Driver RemoveDriver(string driverId)
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

        public void ConfirmRemoveDriver(string driverId)
        {
            var userToDelete = _driverContext.Find(driverId);
            if (userToDelete != null)
            {
                _driverContext.Delete(driverId);
                _driverContext.Commit();
            }
        }
    }
}
