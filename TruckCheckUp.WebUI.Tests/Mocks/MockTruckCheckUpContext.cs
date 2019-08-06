using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruckCheckUp.Core.Contracts.DataAccess;
using TruckCheckUp.Core.Contracts.Logger;
using TruckCheckUp.Core.Models;

namespace TruckCheckUp.WebUI.Tests.Mocks
{
    public class MockTruckCheckUpContext<T> : IRepository<T> where T : BaseEntity
    {
        List<T> items;
        string className;
        private ILogger _logger = new MockTruckCheckUpLogger();

        public MockTruckCheckUpContext()
        {
            className = typeof(T).Name;
            items = new List<T>();
            
        }

        public IQueryable<T> Collection()
        {
            try
            {
                return items.AsQueryable();
            }
            catch (Exception ex)
            {
                _logger.Error("Application unable to return data from Table: " + className, ex);
                throw;
            }
           
        }

        public void Commit()
        {
            return;
        }

        public void Delete(T objectToDelete)
        {
           try
            {
                //Use Find method below to look for record in table
                if (objectToDelete != null)
                {
                    items.Remove(objectToDelete);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Application unable to Delete Id " + objectToDelete.Id + " record in Table " + className, ex);
                throw;
            }


        }

        public T Find(string Id)
        {
            try
            {
                T objectToFind = items.Find(item => item.Id == Id);
                return objectToFind;
            }
            catch (Exception ex)
            {
                _logger.Error("Error looking for Id " + Id + " record in Table " + className, ex);
                throw;
            }
        }

        public void Insert(T objectClass)
        {
            items.Add(objectClass);
        }

        public void Update(T objectClass)
        {

            try
            {
                T objectToUpdate = items.Find(item => item.Id == objectClass.Id);
                objectToUpdate = objectClass;
            }
            catch (Exception ex)
            {
                _logger.Error("Application unable to Update Id " + objectClass.Id + " record in Table: " + className, ex);
                throw;
            }
        }
    }
}
