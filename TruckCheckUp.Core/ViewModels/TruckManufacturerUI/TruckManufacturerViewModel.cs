using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TruckCheckUp.Core.ViewModels.TruckManufacturerUI
{
    public class TruckManufacturerViewModel
    {
        public string Id { get; set; }

        public string ManufacturerDescription { get; set; }

        public bool ManufacturerExistInDB { get; set; } = true;

        public bool ManufacturerIsValid { get; set; } = true;

        public TruckManufacturerViewModel()
        {

        }
    }
}
