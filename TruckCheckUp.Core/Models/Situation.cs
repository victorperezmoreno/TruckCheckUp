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

        [ForeignKey("PartReported")]
        public string PartReportedId { get; set; }

        // Navigation properties
        public virtual PartReported PartReported { get; set; }
    }
}
