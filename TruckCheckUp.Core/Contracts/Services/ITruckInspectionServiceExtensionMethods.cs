using System.Collections.Generic;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.ViewModels.TruckInspection;

namespace TruckCheckUp.Contracts.Services
{
    public interface ITruckInspectionServiceExtensionMethods
    {
        List<DropDownListViewModel> ConvertDriverNamesToDropDownListView(List<Driver> driversRetrievedFromDB);
        List<DropDownListViewModel> ConvertTruckNumbersToDropDownListView(List<Truck> trucksRetrievedFromDB);
        List<CheckBoxListViewModel> ConvertFluidsCatalogToCheckBoxListView(List<PartCatalog> fluidsCatalogFromDB);
        List<CheckBoxListViewModel> ConvertGeneralCatalogToCheckBoxListView(List<PartCatalog> generalCatalogFromDB);
        List<CheckBoxListViewModel> ConvertLightsCatalogToCheckBoxListView(List<PartCatalog> lightsCatalogFromDB);
    }
}