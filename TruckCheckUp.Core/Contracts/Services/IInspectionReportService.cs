using System.Collections.Generic;
using TruckCheckUp.Core.ViewModels;
using TruckCheckUp.Core.ViewModels.InspectionReportUI;

namespace TruckCheckUp.Core.Contracts.Services
{
    public interface IInspectionReportService
    {
        ReportDetailsViewModel RetrieveCompleteInspectionReportForTruckNumberSelectedbyUser(string truckInspectionId);
        List<ReportsViewModel> RetrieveFromDataBaseListOfReportsByTruckNumber(string truckNumberId);
        TruckNumberViewmodel ReturnNewTruckNumberViewModelObject();
        ReportListViewModel RetrieveInspectionReportsListSortedByUserRequest(ReportListViewModel userRequests);
    }
}