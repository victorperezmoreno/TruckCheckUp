using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.Models
{
    public class TruckYear : BaseEntity
    {
        [DisplayName("Truck Year")]
        [Required]
        public int ModelYear { get; set; }

        //Link or connection to "join" table
        public virtual ICollection<Truck> Truck { get; set; }

        //Initialize list of Trucks on class creation
        public TruckYear()
        {
            this.Truck = new List<Truck>();
        }
    }
}
