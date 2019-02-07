using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.ViewModels.TruckUI
{
    public class TruckInsertViewModel
    {
        /***This view model helps to validate that Status checkbox is checked ***/

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

        //Indicate whether a truck is active nor inactive

        [Range(typeof(bool), "true", "true", ErrorMessage = "Status field must be checked.")]
        public bool Status { get; set; } = true;

        //Helps to display a message based on user selection
        public string StatusLabel
        {
            get
            {
                return (bool)this.Status ? "Active" : "Inactive";
            }
        }
    }
}
