using System.Collections.Generic;
using TruckCheckUp.Core.ViewModels.TruckManufacturerUI;

namespace TruckCheckUp.Core.Contracts.Services
{
    public interface ITruckManufacturerService
    {
        TruckManufacturerViewModel AddTruckManufacturer(TruckManufacturerViewModel truckManufacturer);
        void DeleteTruckManufacturer(string truckManufacturerId);
        void PostNewTruckManufacturerToDB(TruckManufacturerViewModel truckManufacturer);
        List<TruckManufacturerViewModel> RetrieveAllTruckManufacturers();
        bool RetrieveTruckManufacturerName(string manufacturerName);
        TruckManufacturerViewModel SearchTruckManufacturer(TruckManufacturerViewModel manufacturer);
        TruckManufacturerViewModel UpdateTruckManufacturer(TruckManufacturerViewModel truckManufacturer);
        void UpdateTruckManufacturerData(TruckManufacturerViewModel truckManufacturer);
        bool ValidateManufacturerString(string manufacturer);
    }
}