using System.Data.Entity;
using System.Linq;
using TruckCheckUp.Core.Contracts.DataAccess;
using TruckCheckUp.Core.Models;

namespace TruckCheckUp.DataAccess.SQL
{
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity 
    {
        internal DataContext _truckCheckUpContext;

        DbSet<T> _dbSet;

        public SQLRepository(DataContext truckCheckUpContext)
        {
            this._truckCheckUpContext = truckCheckUpContext;
            this._dbSet = truckCheckUpContext.Set<T>();
        }

        public IQueryable<T> Collection()
        {
            return _dbSet;
        }

        public void Commit()
        {
            _truckCheckUpContext.SaveChanges();
        }

        public void Delete(string Id)
        {
            //Use Find method below to look for record in table
            var record = Find(Id);
            if (_truckCheckUpContext.Entry(record).State == EntityState.Detached)
            {
                _dbSet.Attach(record);
            }

            _dbSet.Remove(record);

        }

        public T Find(string Id)
        {
            return _dbSet.Find(Id);
        }

        public void Insert(T objectClass)
        {
            _dbSet.Add(objectClass);
        }

        public void Update(T ObjectClass)
        {
            _dbSet.Attach(ObjectClass);
            _truckCheckUpContext.Entry(ObjectClass).State = EntityState.Modified;
        }

    }
}
