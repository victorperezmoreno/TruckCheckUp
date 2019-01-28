using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.Models
{
    public class PartReported :BaseEntity
    {
        // Additional fields needed
        [DataType(DataType.Date)]
        public DateTime ReportedDate { get; set; }

        [Required]
        public int Mileage { get; set; }

        public bool IsChecked { get; set; }

        [ForeignKey("Driver")]
        public string DriverId { get; set; }

        [ForeignKey("Truck")]
        public string TruckId { get; set; }

        [ForeignKey("PartCatalog")]
        public string PartCatalogId { get; set; }

        // Navigation properties
        public virtual Driver Driver { get; set; }

        public virtual Truck Truck { get; set; }

        public virtual PartCatalog PartCatalog { get; set; }

        public virtual ICollection<Situation> Situation { get; set; }

        public virtual ICollection<DriverComment> DriverComment { get; set; }

        public virtual ICollection<MechanicComment> MechanicComment { get; set; }

        //Initialize lists for each 1:N relationship with PartReported
        public PartReported()
        {
            this.Situation = new List<Situation>();
            this.DriverComment = new List<DriverComment>();
            this.MechanicComment = new List<MechanicComment>();
        }
    }
}
