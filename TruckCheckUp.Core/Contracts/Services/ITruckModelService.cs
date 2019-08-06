using System.Collections.Generic;
using TruckCheckUp.Core.ViewModels.TruckModelUI;

namespace TruckCheckUp.Core.Contracts.Services
{
    public interface ITruckModelService
    {
        TruckModelSaveUpdateViewModel AddTruckModel(TruckModelSaveUpdateViewModel truckModel);
        void DeleteTruckModel(string truckModelId);
        void PostNewTruckModelToDB(TruckModelSaveUpdateViewModel truckModel);
        TruckModelSaveUpdateViewModel RetrieveAllTruckManufacturers();
        List<TruckModelViewModel> RetrieveAllTruckModels();
        List<TruckManufacturerDropDownListViewModel> RetrieveManufacturersFromDatabase();
        TruckModelSaveUpdateViewModel RetrieveModelById(string Id);
        bool RetrieveTruckModelName(string modelName);
        TruckModelSaveUpdateViewModel SearchTruckModel(TruckModelSaveUpdateViewModel model);
        TruckModelSaveUpdateViewModel UpdateTruckModel(TruckModelSaveUpdateViewModel truckModel);
        void UpdateTruckModelData(TruckModelSaveUpdateViewModel truckModel);
    }
}