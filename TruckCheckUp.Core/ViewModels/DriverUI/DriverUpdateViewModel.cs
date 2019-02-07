using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.ViewModels
{
    public class DriverUpdateViewModel
    {
        /***This view model helps to avoid the checkbox required validation***/
        public string Id { get; set; }

        [StringLength(30)]
        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }

        [StringLength(30)]
        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }

        public bool Status { get; set; }

        public string StatusLabel
        {
            get
            {
                return (bool)this.Status ? "Active" : "Inactive";
            }
        }
    }
}
