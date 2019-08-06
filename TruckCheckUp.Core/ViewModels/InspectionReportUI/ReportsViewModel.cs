using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.ViewModels.InspectionReportUI
{
    public class ReportsViewModel
    {
        public string Id { get; set; }
        public string DateCreated { get; set; }
        public string Driver { get; set; }
        public string Comments { get; set; }
    }
}
