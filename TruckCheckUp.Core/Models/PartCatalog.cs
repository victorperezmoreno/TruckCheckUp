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
    public class PartCatalog : BaseEntity
    {
        [StringLength(30)]
        [DisplayName("Part Name")]
        [Required]
        public string PartName { get; set; }

        [ForeignKey("PartCategory")]
        [Required(ErrorMessage = "Part Category is Required")] 
        public string PartCategoryId { get; set; }

        // Navigation properties
        public virtual PartCategory PartCategory { get; set; }

        //Link or connection to "join" table
        public virtual ICollection<PartReported> PartReported { get; set; }

        //Initialize list of part reported on class creation
        public PartCatalog()
        {
            this.PartReported = new List<PartReported>();
        }

    }
}
