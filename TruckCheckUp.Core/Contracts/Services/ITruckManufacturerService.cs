using System.Collections.Generic;
using TruckCheckUp.Core.Contracts.Logger;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.ViewModels.TruckManufacturerUI;

namespace TruckCheckUp.Core.Contracts.Services
{
    public interface ITruckManufacturerService
    {
        TruckManufacturerViewModel ValidateTruckManufacturerToAdd(TruckManufacturerViewModel truckManufacturer);
        void ValidateTruckManufacturerToDelete(string Id);
        void DeleteTruckManufacturerData(TruckManufacturer truckManufacturerToDelete, ILogger logger);
        void SaveNewTruckManufacturerData(TruckManufacturerViewModel truckManufacturer, ILogger logger);
        List<TruckManufacturerViewModel> RetrieveAllTruckManufacturers();
        bool RetrieveTruckManufacturerName(string manufacturerName);
        TruckManufacturerViewModel SearchTruckManufacturer(TruckManufacturerViewModel manufacturer);
        TruckManufacturerViewModel ValidateTruckManufacturerToUpdate(TruckManufacturerViewModel truckManufacturer);
        void UpdateTruckManufacturerData(TruckManufacturerViewModel truckManufacturer, ILogger logger);
        bool ValidateManufacturerString(string manufacturer);
    }
}