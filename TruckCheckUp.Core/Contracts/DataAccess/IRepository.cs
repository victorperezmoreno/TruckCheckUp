using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruckCheckUp.Core.Models;

namespace TruckCheckUp.Core.Contracts.DataAccess
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> Collection();
        void Commit();
        void Delete(T objectClass);
        T Find(string Id);
        void Insert(T objectClass);
        void Update(T objectClass);
    }
}
