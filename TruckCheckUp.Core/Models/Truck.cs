using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.Models
{
    public class Truck
    {
        [StringLength(30)]
        [DisplayName("VIN Number")]
        [Required]
        public string VIN { get; set; }

        [DisplayName("Truck Number")]
        [Required]
        public int TruckNumber { get; set; }

        [StringLength(30)]
        [DisplayName("Manufacturer")]
        [Required]
        public string Manufacturer { get; set; }

        [StringLength(30)]
        [DisplayName("Truck Model")]
        [Required]
        public string Model { get; set; }

        [DisplayName("Model Year")]
        [Required]
        public int Year { get; set; }

        //Link or connection to "join" table
        public virtual ICollection<PartReported> PartReported { get; set; }

        //Initialize list of part reported on class creation
        public Truck()
        {
            this.PartReported = new List<PartReported>();
        }
    }
}
