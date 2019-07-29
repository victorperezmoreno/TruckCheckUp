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
    public class Situation : BaseEntity
    {
        //[Required]
        //Will not use it now, will search for the "Open" option in a query
        //public int StatusCode { get; set; } 
        
        [Required]
        [StringLength(20)]
        public string Description { get; set; }

        //Indicate whether a situation is active nor inactive
        [Required]
        public bool Status { get; set; }

        public virtual ICollection<PartSituation> PartSituation { get; set; }

        public Situation()
        {
            PartSituation = new List<PartSituation>();
        }
    }
}
