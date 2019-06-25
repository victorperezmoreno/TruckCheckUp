using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.ViewModels.TruckInspection
{
    public class TruckInspectionDTO
    {
        public string DriverId { get; set; }
        public string TruckId { get; set; }
        public int Mileage { get; set; }
        public int TicketNumber { get; set; }
        public string CommentDriver { get; set; }
    }
}
