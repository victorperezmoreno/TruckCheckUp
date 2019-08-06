using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.ViewModels.DriverUI
{
    public class DriverInsertViewModel
    {        
        /***This view model helps to validate that Status checkbox is checked ***/

        [StringLength(30)]
        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }

        [StringLength(30)]
        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Status field must be checked.")]
        public bool Status { get; set; } = true;

        public string StatusLabel
        {
            get
            {
                return (bool)this.Status ? "Active" : "Inactive";
            }
        }

    }
}
