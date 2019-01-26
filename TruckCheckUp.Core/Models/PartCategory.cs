using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.Models
{
    public class PartCategory : BaseEntity
    {
        public string CategoryPart { get; set; }

        //Link or connection to "join" table
        public virtual ICollection<PartCatalog> PartCatalog { get; set; }

        //Initialize list of part reported on class creation
        public PartCategory()
        {
            this.PartCatalog = new List<PartCatalog>();
        }
    }
}
