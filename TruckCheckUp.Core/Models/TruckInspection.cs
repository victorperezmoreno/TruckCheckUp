using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.Models
{
    public class TruckInspection :BaseEntity
    {
        // Additional fields needed
        [Required]
        public int Mileage { get; set; }

        [Required]
        [Display(Name = "Ticker Number")]
        public int TicketNumber { get; set; }

        [Required]
        public bool IsOK { get; set; }

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

        public virtual ICollection<MechanicComment> MechanicComment { get; set; }

        //Initialize lists for each 1:N relationship
        public TruckInspection()
        {
            this.Situation = new List<Situation>();
            this.MechanicComment = new List<MechanicComment>();
        }
    }
}
