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

        // Navigation property
        public virtual PartCatalog PartCatalog { get; set; }
        // Navigation property
        public virtual TruckInspection TruckInspection { get; set; }

        public virtual ICollection<MechanicComment> MechanicComment { get; set; }

        public PartsInspected()
        {
            MechanicComment = new List<MechanicComment>();
        }
    }
}
