using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruckCheckUp.Core.Models;

namespace TruckCheckUp.Core.ViewModels.CatalogUI
{
    public class PartViewModel
    {
        //A class to create a view that allows to add parts to DB
        public PartCatalog PartCatalog { get; set; }

        //Will display this data in Category DropDownList
        public IEnumerable<PartCategory> Categories { get; set; }
    }
}
