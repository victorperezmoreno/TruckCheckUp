using System.Data.Entity;
using System.Linq;
using TruckCheckUp.Core.Contracts.DataAccess;
using TruckCheckUp.Core.Contracts.Logger;
using TruckCheckUp.Core.Models;
using System;

namespace TruckCheckUp.DataAccess.SQL
{
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity 
    {
        internal DataContext _truckCheckUpContext;
        internal ILogger _logger;
        string tableName = "";
        DbSet<T> _dbSet;

        public SQLRepository(DataContext truckCheckUpContext, ILogger logger)
        {
            this._truckCheckUpContext = truckCheckUpContext;
            this._logger = logger;
            tableName = typeof(T).Name;
            this._dbSet = truckCheckUpContext.Set<T>();
        } 
        
        public IQueryable<T> Collection()
        {
            try
            {
                return _dbSet;               
            }
            catch (Exception ex)
            { 
                _logger.Error("Application unable to return data from Table: " + tableName , ex);
                throw;
            }  
        }

        public void Commit()
        {
            try
            {
                _truckCheckUpContext.SaveChanges();
            }
            catch (Exception ex) 
            {
                _logger.Fatal("Application unable to Save record in Table " + tableName, ex);
                throw;
            }
            
        }

        public void Delete(T objectClass)
        {
            try
            {
                //Use Find method below to look for record in table
                if (_truckCheckUpContext.Entry(objectClass).State == EntityState.Detached)
                {
                    _dbSet.Attach(objectClass);
                } 
                _dbSet.Remove(objectClass); 
            }
            catch (Exception ex)
            {
                _logger.Error("Application unable to Delete Id " + objectClass.Id + " record in Table " + tableName, ex);
                throw;
            }
        }

        public T Find(string Id)
        {
            try
            {
                return _dbSet.Find(Id);       
            }
            catch (Exception ex)
            {
                _logger.Error("Error looking for Id " + Id + " record in Table " + tableName, ex);
                throw;
            }
        }

        public void Insert(T objectClass)
        {
            try
            {
                _dbSet.Add(objectClass); 
            }
            catch (Exception ex)
            {
                _logger.Error("Application unable to Insert Id " + objectClass.Id + " record into Table: " + tableName, ex);
                throw;
            }
        }

        public void Update(T objectClass)
        {
            try
            {
                _dbSet.Attach(objectClass);
                _truckCheckUpContext.Entry(objectClass).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                _logger.Error("Application unable to Update Id " + objectClass.Id + " record in Table: " + tableName, ex);
                throw;
            }
           
        }

    }
}
