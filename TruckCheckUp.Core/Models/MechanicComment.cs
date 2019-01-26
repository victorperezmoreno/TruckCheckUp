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
    public class MechanicComment : BaseEntity
    {
        public DateTime CommentDate { get; set; }

        [StringLength(100)]
        [DisplayName("Comments")]
        public string CommentMechanic { get; set; }

        [ForeignKey("PartReported")]
        public string PartReportedId { get; set; }

        // Navigation properties
        public virtual PartReported PartReported { get; set; }
    }
}
