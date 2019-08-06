using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.ViewModels.InspectionReportUI
{
    public class IssuesListViewModel
    {
        public DateTime CreationDate { get; set; }
        public string PartReported { get; set; }
        public string Mileage { get; set; }
        public bool Status { get; set; }
        public string Driver { get; set; }
        public string Situation { get; set; }
    }
}
