using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.Models
{
    public class Driver : BaseEntity
    {
        [StringLength(30)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [StringLength(30)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        //Indicate whether a driver is active nor inactive, default value is true
        public bool Status { get; set; }

        //Helps to display a message based on user selection
        [NotMapped]
        public string MessageBasedOnStatusSelection
        {
            get
            {
                return (bool)this.Status ? "Active" : "Inactive";
            }
        }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return FirstName + ", " + LastName; }
        }

        //Link or connection to "join" table
        public virtual ICollection<TruckInspection> TruckInspection { get; set; }

        //Initialize list of part reported on class creation
        public Driver()
        {
           this.TruckInspection = new List<TruckInspection>();
        }
    }
}
