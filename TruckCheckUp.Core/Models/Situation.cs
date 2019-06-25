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
        [Required]
        public bool Status { get; set; }

        public DateTime StatusDate { get; set; }

        [ForeignKey("TruckInspection")]
        public string PartReportedId { get; set; }

        // Navigation properties
        public virtual TruckInspection TruckInspection { get; set; }
    }
}
