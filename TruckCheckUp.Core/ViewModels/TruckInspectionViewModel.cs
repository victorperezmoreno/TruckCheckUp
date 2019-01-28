using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruckCheckUp.Core.Models;

namespace TruckCheckUp.Core.ViewModels
{
    public class TruckInspectionViewModel
    {            
        //To save selected driver Id from dropdownlist
        public string DriverId { get; set; }
        //To save selected truck Id from dropdownlist
        public string TruckId { get; set; } 
        //To save truck mileage from textbox
        public int TruckMileage { get; set; }
        //To populate Driver DropDownList in view
        public IEnumerable<Driver> Drivers { get; set; }
        //To populate Truck # DropDownList in view
        public IEnumerable<Truck> Trucks { get; set; }
        
        //List of General parts to display as checkboxes 
        public IEnumerable<PartCatalog> GeneralTruckParts { get; set; }

        //List of Lights parts to display as checkboxes 
        public IEnumerable<PartCatalog> LightsTruckParts { get; set; }

        //List of Fluid parts to display as checkboxes 
        public IEnumerable<PartCatalog> FluidTruckParts { get; set; }

        //List of Other parts to display as checkboxes 
        public IEnumerable<PartCatalog> OtherTruckParts { get; set; }

        //To save list of parts reported that needs attention
        public IEnumerable<string> NeedsAttentionParts { get; set; }
    }
}
