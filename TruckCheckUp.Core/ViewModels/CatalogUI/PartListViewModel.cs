using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.ViewModels.CatalogUI
{
    public class PartListViewModel
    {
        public string Id { get; set; }
        public string PartName { get; set; }
        public string CategoryName { get; set; }

        public PartListViewModel()
        {

        }
    }
}
