using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.ViewModels.TruckUI
{
    public class TruckSaveUpdateViewModel
    {
        /***This view model helps to validate that Status checkbox is checked ***/
        public string Id { get; set; }
        
        public string VIN { get; set; }

        public string TruckNumber { get; set; }

        public string Manufacturer { get; set; }

        public string Model { get; set; }

        public string Year { get; set; }

        //Indicate whether a truck is active nor inactive
        public bool Status { get; set; } = true;

        //Helps to display a message based on user selection
        public string StatusLabel { get; set; }

        public bool TruckNumberIsValid { get; set; } = true;

        public bool VinNumberIsValid { get; set; } = true;

        public bool ExistInDB { get; set; } = true;
        
        public IEnumerable<ManufacturerDropDownListViewModel> ManufacturerDropDownList { get; set; }

        public IEnumerable<ModelDropDownListViewModel> ModelDropDownList { get; set; }

        public IEnumerable<YearDropDownListViewModel> YearDropDownList { get; set; }

        public TruckSaveUpdateViewModel()
        {

        }

    }
}
