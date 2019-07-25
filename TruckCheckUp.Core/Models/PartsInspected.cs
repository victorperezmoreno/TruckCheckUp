using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.Models
{
    public class PartsInspected : BaseEntity
    {
        //Additional field
        public bool IsOK { get; set; }

        [ForeignKey("PartCatalog")]
        public string PartCatalogId { get; set; }

        [ForeignKey("TruckInspection")]
        public string TruckInspectionId { get; set; }

        // Navigation properties
        public virtual PartCatalog PartCatalog { get; set; }
    
        public virtual TruckInspection TruckInspection { get; set; }
        
        //Link or connection to "join" table
        public virtual ICollection<MechanicComment> MechanicComment { get; set; }
        
        public virtual ICollection<PartSituation> PartSituation { get; set; }

        public PartsInspected()
        {
            MechanicComment = new List<MechanicComment>();
            PartSituation = new List<PartSituation>();

        }
    }
}
