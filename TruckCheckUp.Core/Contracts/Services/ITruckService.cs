using System.Collections.Generic;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.ViewModels.TruckUI;

namespace TruckCheckUp.Core.Contracts.Services
{
    public interface ITruckService
    {
        TruckSaveUpdateViewModel AddTruck(TruckSaveUpdateViewModel truck);
        void DeleteTruck(string truckId);
        List<ModelDropDownListViewModel> MoveListOfModelsToModelDropDownListViewModel(List<TruckModel> modelsFromQueryResult);
        void PostNewTruckModelToDB(TruckSaveUpdateViewModel truck);
        TruckSaveUpdateViewModel RetrieveAllTruckManufacturers();
        List<TruckViewModel> RetrieveAllTrucks();
        List<ManufacturerDropDownListViewModel> RetrieveManufacturersFromDatabase();
        TruckSaveUpdateViewModel RetrieveModelAndYearListsFromDB(string Id);
        List<ModelDropDownListViewModel> RetrieveModelsFromDatabase();
        List<ModelDropDownListViewModel> RetrieveModelsFromDatabaseBasedOnManufacturerID(string Id);
        TruckSaveUpdateViewModel RetrieveTruckById(string Id);
        bool RetrieveTruckNumber(int truckNumber);
        List<YearDropDownListViewModel> RetrieveYearsFromDatabase();
        TruckSaveUpdateViewModel SearchTruck(TruckSaveUpdateViewModel truck);
        TruckSaveUpdateViewModel UpdateTruck(TruckSaveUpdateViewModel truck);
        void UpdateTruckData(TruckSaveUpdateViewModel truck);
    }
}