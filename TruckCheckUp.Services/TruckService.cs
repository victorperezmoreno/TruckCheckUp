using System.Collections.Generic;
using System.Linq;
using TruckCheckUp.Core.Contracts.DataAccess;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.ViewModels.TruckUI;

namespace TruckCheckUp.Services
{
    public class TruckService
    {
        private IRepository<Truck> _truckContext;

        public TruckService(IRepository<Truck> truckContext)
        {
            _truckContext = truckContext;
        }

        public List<Truck> RetrieveAllTrucks()
        {
            //Get a list of Trucks
            var trucksRetrieved = _truckContext.Collection().ToList();

            return trucksRetrieved;
        }

        public TruckInsertViewModel CreateNewTruckObject()
        {
            var truck = new TruckInsertViewModel();

            return truck;
        }

        public void PostNewTruckToDB(TruckInsertViewModel truck)
        {
            var truckToInsert = new Truck();
            truckToInsert.VIN          = truck.VIN;
            truckToInsert.TruckNumber  = truck.TruckNumber;
            truckToInsert.Manufacturer = truck.Manufacturer;
            truckToInsert.Model        = truck.Model;
            truckToInsert.Year         = truck.Year;
            truckToInsert.Status       = truck.Status;

            _truckContext.Insert(truckToInsert);
            _truckContext.Commit();
        }

        public TruckUpdateViewModel RetrieveTruckDataToUpdate(string truckId)
        {
            //Look for truck and return her data
            var truckToUpdate = (
                    from truckStoredInDB in _truckContext.Collection()
                    where truckStoredInDB.Id == truckId
                    select new TruckUpdateViewModel()
                    {
                        Id           = truckStoredInDB.Id,
                        VIN          = truckStoredInDB.VIN,
                        TruckNumber  = truckStoredInDB.TruckNumber,
                        Manufacturer = truckStoredInDB.Manufacturer,
                        Model        = truckStoredInDB.Model,
                        Year         = truckStoredInDB.Year,
                        Status       = truckStoredInDB.Status
                    }).FirstOrDefault();

            if (truckToUpdate != null)
            {
                return truckToUpdate;
            }
            else
            {
                return new TruckUpdateViewModel();
            }
        }

        public void UpdateTruckData(TruckUpdateViewModel truck, string truckId)
        {
            var truckToUpdate = _truckContext.Find(truckId);
            if (truckToUpdate != null)
            {
                truckToUpdate.VIN          = truck.VIN;
                truckToUpdate.TruckNumber  = truck.TruckNumber;
                truckToUpdate.Manufacturer = truck.Manufacturer;
                truckToUpdate.Model        = truck.Model;
                truckToUpdate.Year         = truck.Year;
                truckToUpdate.Status       = truck.Status;

                _truckContext.Commit();
            }
        }

        public Truck RetrieveTruckToDelete(string truckId)
        {
            var truckToDelete = _truckContext.Find(truckId);
            if (truckToDelete != null)
            {
                return truckToDelete;
            }
            else
            {
                return new Truck();
            }
        }

        public void DeleteTruck(string truckId)
        {
            var truckToDelete = _truckContext.Find(truckId);
            if (truckToDelete != null)
            {
                _truckContext.Delete(truckId);
                _truckContext.Commit();
            }
        }

    }
}
