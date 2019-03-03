using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruckCheckUp.Core.Contracts.DataAccess;
using TruckCheckUp.Core.Models;

namespace TruckCheckUp.WebUI.Tests.Mocks
{
    public class MockTruckCheckUpContext<T> : IRepository<T> where T : BaseEntity
    {
        List<T> items;
        string className;

        public MockTruckCheckUpContext()
        {
            className = typeof(T).Name;
            items = new List<T>();
            
        }

        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }

        public void Commit()
        {
            return;
        }

        public void Delete(T objectToDelete)
        {
            //T objectToDelete = items.Find(item => item.Id == Id);
            if (objectToDelete != null)
            {
                items.Remove(objectToDelete);
            }
            else
            {
                throw new Exception(className + " Not found");
            }
           

        }

        public T Find(string Id)
        {
            T objectToFind = items.Find(item => item.Id == Id);
            if (objectToFind != null)
            {
                return objectToFind;
            }
            else
            {
                throw new Exception(className + " Not found");
            }
        }

        public void Insert(T objectClass)
        {
            items.Add(objectClass);
        }

        public void Update(T objectClass)
        {
            T objectToUpdate = items.Find(item => item.Id == objectClass.Id);
            if (objectToUpdate != null)
            {
                objectToUpdate = objectClass;
            }
            else
            {
                throw new Exception(className + " Not found");
            }
        }
    }
}
