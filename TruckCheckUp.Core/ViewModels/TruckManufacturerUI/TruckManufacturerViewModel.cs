using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TruckCheckUp.Core.ViewModels.TruckManufacturerUI
{
    public class TruckManufacturerViewModel
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public bool ExistInDB { get; set; } = true;

        public bool IsValid { get; set; } = true;

        public TruckManufacturerViewModel()
        {

        }
    }
}
