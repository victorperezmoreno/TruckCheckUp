using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruckCheckUp.Core.Models;

namespace TruckCheckUp.Core.Models
{
    public class MechanicComment : BaseEntity
    {
        [StringLength(100)]
        [DisplayName("Comments")]
        public string CommentMechanic { get; set; }

        [ForeignKey("Situation")]
        public string SituationId { get; set; }

        [ForeignKey("PartsInspected")]
        public string PartsInspectedId { get; set; }

        // Navigation properties
        public virtual Situation Situation { get; set; }

        public virtual  PartsInspected PartsInspected { get; set; }
    }
}
