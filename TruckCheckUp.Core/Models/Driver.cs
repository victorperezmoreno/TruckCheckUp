using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.Models
{
    public class Driver : BaseEntity
    {
        [StringLength(30)]
        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }

        [StringLength(30)]
        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return FirstName + ", " + LastName; }
        }

        //Link or connection to "join" table
        public virtual ICollection<PartReported> PartReported { get; set; }

        //Initialize list of part reported on class creation
        public Driver()
        {
           this.PartReported = new List<PartReported>();
        }
    }
}
