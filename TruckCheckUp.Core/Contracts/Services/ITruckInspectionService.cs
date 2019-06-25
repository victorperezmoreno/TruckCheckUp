using TruckCheckUp.Core.ViewModels.TruckInspection;

namespace TruckCheckUp.Contracts.Services
{
    public interface ITruckInspectionService
    {
        TruckInspectionViewModel CreateNewTruckInspectionObject();
        TruckInspectionViewModel CreateTruckInspection(TruckInspectionViewModel inspectionToCreate);
        TruckInspectionViewModel PopulateDriversTrucksAndPartsCatalog(TruckInspectionViewModel truckInspection);
    }
}