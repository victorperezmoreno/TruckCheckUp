using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.Models
{
    public class PartSituation : BaseEntity
    {
        [ForeignKey("Situation")]
        public string SituationId { get; set; }

        [ForeignKey("PartInspected")]
        public string PartInspectedId { get; set; }

        // Navigation properties
        public virtual Situation Situation { get; set; }

        public virtual PartsInspected PartInspected { get; set; }
    }
}
