using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.Models
{
    public abstract class BaseEntity
    {
        public string Id { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        //Constructor
        public BaseEntity()
        {
            //Generate Guid for new records
            this.Id = Guid.NewGuid().ToString();
            //Register date of row creation
            this.CreationDate = DateTime.Now;
        }
    }

}
