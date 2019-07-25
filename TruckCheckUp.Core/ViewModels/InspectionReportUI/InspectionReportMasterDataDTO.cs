using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.ViewModels.InspectionReportUI
{
    public class InspectionReportMasterDataDTO
    {
        public string Id { get; set; }
        public string DriverName { get; set; }
        public string TruckNumber { get; set; }
        public string DateSubmitted { get; set; }
        public string Mileage { get; set; }
        public string Comments { get; set; }
    }
}
