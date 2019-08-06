using System.Collections.Generic;
using TruckCheckUp.Core.Models;

namespace TruckCheckUp.Core.ViewModels.TruckModelUI
{
    public class TruckModelSaveUpdateViewModel
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public string ManufacturerId { get; set; }

        public string ManufacturerDescription { get; set; }

        public bool ExistInDB { get; set; } = true;

        public bool IsValid { get; set; } = true;

        public IEnumerable<TruckManufacturerDropDownListViewModel> ManufacturerDropDownList { get; set; }

        public TruckModelSaveUpdateViewModel()
        {
            ManufacturerDropDownList = new List<TruckManufacturerDropDownListViewModel>();
        }
    }
}
