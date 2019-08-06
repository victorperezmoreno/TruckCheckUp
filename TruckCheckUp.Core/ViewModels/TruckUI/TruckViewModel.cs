using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.ViewModels.TruckUI
{
    public class TruckViewModel
    {
        public string Id { get; set; }

        public int TruckNumber { get; set; }

        public string VIN { get; set; }

        public string Manufacturer { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        //Indicate whether a truck is active nor inactive

        public bool Status { get; set; } = true;

        //Helps to display a message based on user selection
        public string StatusLabel { get; set; }
       
    }
}
