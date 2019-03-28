using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.Models
{
    public class TruckManufacturer : BaseEntity
    {
        [StringLength(30)]
        [DisplayName("Manufaturer")]
        [Required]
        public string ManufacturerDescription { get; set; }

        //Link or connection to "join" table
        public virtual ICollection<Truck> Truck { get; set; }

        public virtual ICollection<TruckModel> TruckModel { get; set; }

        //Initialize list of Trucks on class creation
        public TruckManufacturer()
        {
            Truck = new List<Truck>();
            TruckModel = new List<TruckModel>();
        }
    }
}
