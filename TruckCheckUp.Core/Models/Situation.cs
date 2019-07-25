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
        public int StatusCode { get; set; }
        [Required]
        public string Description { get; set; }

        public virtual ICollection<PartSituation> PartSituation { get; set; }

        public Situation()
        {
            PartSituation = new List<PartSituation>();
        }
    }
}
