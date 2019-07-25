using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruckCheckUp.Core.Contracts.DataAccess;
using TruckCheckUp.Core.Contracts.Logger;
using TruckCheckUp.Core.Contracts.Services;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.ViewModels;
using TruckCheckUp.Core.ViewModels.InspectionReportUI;
using TruckCheckUp.Core.ViewModels.TruckInspection;

namespace TruckCheckUp.Services
{
    public class InspectionReportService : IInspectionReportService
    {
        private IRepository<TruckInspection> _truckInspectionContext;
        private IRepository<Driver> _driverContext;
        private IRepository<Truck> _truckContext;
        private IRepository<PartsInspected> _partsInspectedContext;
        private IRepository<PartCatalog> _partCatalog;
        private IRepository<Situation> _situationCatalog;
        private ILogger _logger;
        private string generalCategoryId = "607412fa-729b-4dfd-94ee-9a8d84d1eed8";
        private string lightCategoryId = "43d29546-83c4-4a92-bafe-262aa58d6496";
        private string fluidCategoryId = "3897c106-7357-4ab8-b594-02020a7ce9c6";

        public InspectionReportService(IRepository<TruckInspection> truckInspectionContext,
                                       IRepository<Driver> driverContext,
                                       IRepository<Truck> truckContext,
                                       IRepository<PartsInspected> partsInspectedContext,
                                       IRepository<PartCatalog> partCatalog,
                                       IRepository<Situation> situationCatalog,
                                       ILogger logger)
        {
            _truckInspectionContext = truckInspectionContext;
            _driverContext = driverContext;
            _truckContext = truckContext;
            _partsInspectedContext = partsInspectedContext;
            _partCatalog = partCatalog;
            _situationCatalog = situationCatalog;
            _logger = logger;
        }

        //Return ReportViewmodel to Controller => View
        public TruckNumberViewmodel ReturnNewTruckNumberViewModelObject()
        {
            return CreateANewTruckNumberViewModelObject();
        }

        //Create New ReportInspection viewmodel and populate it with truck numbers
        private TruckNumberViewmodel CreateANewTruckNumberViewModelObject()
        {
            var reportViewModelObject = new TruckNumberViewmodel();
            reportViewModelObject.TruckNumbers = RetrieveTruckNumbersInAscendingOrder();

            return reportViewModelObject;
        }

        //Populate a DropDownList with TruckNumbers
        private List<DropDownListViewModel> RetrieveTruckNumbersInAscendingOrder()
        {  
            var truckListRetrievedFromDB = _truckContext.Collection().OrderBy(t => t.TruckNumber).Where(s => s.Status == true).ToList();
            return ConvertTruckNumbersToDropDownListView(truckListRetrievedFromDB);
        }

        private List<DropDownListViewModel> ConvertTruckNumbersToDropDownListView(List<Truck> truckList)
        {
            if (truckList != null && truckList.Any())
            {
                return truckList.Select(truck => new DropDownListViewModel
                {
                    Id = truck.Id,
                    Text = truck.TruckNumber.ToString()
                }).ToList();
            }

            return new List<DropDownListViewModel>();
        }

        //Return list of reports available for truck number selected by user
        public List<ReportsViewModel> RetrieveFromDataBaseListOfReportsByTruckNumber(string truckNumberId)
        {
            //Retrieve all reports from DB based on truck number selected by user
            var reportsList = _truckInspectionContext.Collection().Where(t => t.TruckId == truckNumberId).OrderBy(d => d.CreationDate).ToList();
            return ConvertListOfReportsRetrievedToReportsListViewModel(reportsList);
        }
        
        //Move the data to ViewModel to send it over to View
        private List<ReportsViewModel> ConvertListOfReportsRetrievedToReportsListViewModel(List<TruckInspection> inspectionsList)
        {
            if (inspectionsList != null && inspectionsList.Any())
            {
                return inspectionsList.Select(inspection => new ReportsViewModel
                {
                    Id = inspection.Id,
                    DateCreated = inspection.CreationDate.Date.ToShortDateString(),
                    Driver = inspection.Driver.FullName,
                    Comments = inspection.Comments
                }).ToList();
            }
            return new List<ReportsViewModel>();
        }

        //Retrieve data specific to a report requested by truck number
        public ReportDetailsViewModel RetrieveCompleteInspectionReportForTruckNumberSelectedbyUser(string truckInspectionId)
        {

            //Retrieve the inspection master data from inspection table into a ReportDetailsViewModel object 
            var inspectionReportMasterDetails = RetrieveReportMasterData(_truckInspectionContext.Collection().Where(t => t.Id == truckInspectionId).SingleOrDefault());

            //Retrieve the general parts reported and their status as reported by driver and assign it to ReportDetailsViewModel object already created in first step
            inspectionReportMasterDetails.GeneralCatalog = RetrieveGeneralPartsInpected(_partsInspectedContext.Collection().Where(t => t.TruckInspectionId == truckInspectionId && t.PartCatalog.PartCategoryId == generalCategoryId).ToList());

            //Retrieve the light parts reported and their status as reported by driver and assign it to ReportDetailsViewModel object already created in first step
            inspectionReportMasterDetails.LightsCatalog = RetrieveLightPartsInspected(_partsInspectedContext.Collection().Where(t => t.TruckInspectionId == truckInspectionId && t.PartCatalog.PartCategoryId == lightCategoryId).ToList());

            //Retrieve the fluid parts reported and their status as reported by driver and assign it to ReportDetailsViewModel object already created in first step
            inspectionReportMasterDetails.FluidsCatalog = RetrieveFluidPartsInspected(_partsInspectedContext.Collection().Where(t => t.TruckInspectionId == truckInspectionId && t.PartCatalog.PartCategoryId == fluidCategoryId).ToList());

            return inspectionReportMasterDetails;
        }

        private ReportDetailsViewModel RetrieveReportMasterData(TruckInspection inspectionRecord)
        {
            if (inspectionRecord != null)
            {
                return new ReportDetailsViewModel
                {
                    Id = inspectionRecord.Id,
                    DriverName = inspectionRecord.Driver.FullName,
                    TruckNumber = inspectionRecord.Truck.TruckNumber.ToString(),
                    SubmissionDate = inspectionRecord.CreationDate.ToString(),
                    CurrentMileage = inspectionRecord.Mileage.ToString(),
                    Comments = inspectionRecord.Comments
                };
            }

            return new ReportDetailsViewModel();
        }

        private List<CheckBoxListViewModel> RetrieveGeneralPartsInpected(List<PartsInspected> generalPartsList)
        {
            if (generalPartsList != null && generalPartsList.Any())
            {
                return generalPartsList.Select(gParts => new CheckBoxListViewModel
                {
                    Id = gParts.Id,
                    Name = gParts.PartCatalog.PartName,
                    IsChecked = gParts.IsOK
                }).ToList();
            }

            return new List<CheckBoxListViewModel>();
        }

        private List<CheckBoxListViewModel> RetrieveLightPartsInspected(List<PartsInspected> lightPartsList)
        {
            if (lightPartsList != null && lightPartsList.Any())
            {
                return lightPartsList.Select(lParts => new CheckBoxListViewModel
                {
                    Id = lParts.Id,
                    Name = lParts.PartCatalog.PartName,
                    IsChecked = lParts.IsOK
                }).ToList();
            }

            return new List<CheckBoxListViewModel>();
        }


        private List<CheckBoxListViewModel> RetrieveFluidPartsInspected(List<PartsInspected> fluidPartsList)
        {
            if (fluidPartsList != null && fluidPartsList.Any())
            {
                return fluidPartsList.Select(fParts => new CheckBoxListViewModel
                {
                    Id = fParts.Id,
                    Name = fParts.PartCatalog.PartName,
                    IsChecked = fParts.IsOK
                }).ToList();
            }

            return  new List<CheckBoxListViewModel>();
        }

        private int ReturnNumberOfPages(ReportListViewModel reportsListObject)
        {
            int pages = reportsListObject.ReportsList.Count() % reportsListObject.PageSize;
            int pageCount = (pages == 0 ?
                            Convert.ToInt32(Math.Ceiling((double)(reportsListObject.ReportsList.Count()
                           / reportsListObject.PageSize))) :
                            Convert.ToInt32(Math.Ceiling((double)(reportsListObject.ReportsList.Count()
                           / reportsListObject.PageSize) + 1)));

            return pageCount;
        }

        private ReportListViewModel RetrieveInspectionReportListInAscendingOrDescendingOrder(ReportListViewModel inspectionListToOrder)
        { 
            //Create a copy of ReportListViewModel using parameter object
            var newOrderedListOfInspectionReports = new ReportListViewModel();

            switch (inspectionListToOrder.SortField)
            {
              case "Id": newOrderedListOfInspectionReports.ReportsList = (inspectionListToOrder.SortDirection == "ascending" ?
                            inspectionListToOrder.ReportsList.OrderBy(i => i.Id).ToList() :
                            inspectionListToOrder.ReportsList.OrderByDescending(i => i.Id).ToList());
                    break;
              case "DateCreated": newOrderedListOfInspectionReports.ReportsList = (inspectionListToOrder.SortDirection == "ascending" ?
                             inspectionListToOrder.ReportsList.OrderBy(d => d.DateCreated).ToList() :
                             inspectionListToOrder.ReportsList.OrderByDescending(d => d.DateCreated).ToList());
                    break;
              case "Driver": newOrderedListOfInspectionReports.ReportsList = (inspectionListToOrder.SortDirection == "ascending" ?
                             inspectionListToOrder.ReportsList.OrderBy(d => d.Driver).ToList() :
                             inspectionListToOrder.ReportsList.OrderByDescending(d => d.Driver).ToList());
                    break;
              case "Comments": newOrderedListOfInspectionReports.ReportsList = (inspectionListToOrder.SortDirection == "ascending" ?
                             inspectionListToOrder.ReportsList.OrderBy(c => c.Comments).ToList() :
                             inspectionListToOrder.ReportsList.OrderByDescending(c => c.Comments).ToList());
                    break;
            }

            newOrderedListOfInspectionReports.CurrentPageIndex = inspectionListToOrder.CurrentPageIndex;
            newOrderedListOfInspectionReports.PageCount = inspectionListToOrder.PageCount;
            //newOrderedListOfInspectionReports.SearchString = inspectionListToOrder.SearchString;
            newOrderedListOfInspectionReports.SortDirection = inspectionListToOrder.SortDirection;
            newOrderedListOfInspectionReports.SortField = inspectionListToOrder.SortField;
            newOrderedListOfInspectionReports.TruckNumberId = inspectionListToOrder.TruckNumberId;

            return newOrderedListOfInspectionReports;
        }

        //Return ordered list of reports on page selectd by user
        private ReportListViewModel ReturnUserToPageWhereOrderByWasTriggeredWithListOfReportsSorted(ReportListViewModel orderedListOfInspectionReports)
        {
            //Create a copy of ReportListViewModel using parameter object
            var orderedListOfReportsWherePageWasSelectedByUser = new ReportListViewModel();
            if (orderedListOfInspectionReports.CurrentPageIndex == 0)
            {
                orderedListOfReportsWherePageWasSelectedByUser.ReportsList = orderedListOfInspectionReports.ReportsList.Take(orderedListOfInspectionReports.PageSize).ToList();
            }
            else
            {
                orderedListOfReportsWherePageWasSelectedByUser.ReportsList = orderedListOfInspectionReports.ReportsList.Skip(orderedListOfInspectionReports.CurrentPageIndex
                            * orderedListOfInspectionReports.PageSize).Take(orderedListOfInspectionReports.PageSize).ToList();
            }

            orderedListOfReportsWherePageWasSelectedByUser.CurrentPageIndex = orderedListOfInspectionReports.CurrentPageIndex;
            orderedListOfReportsWherePageWasSelectedByUser.PageCount = orderedListOfInspectionReports.PageCount;
            //orderedListOfReportsWherePageWasSelectedByUser.SearchString = orderedListOfInspectionReports.SearchString;
            orderedListOfReportsWherePageWasSelectedByUser.SortDirection = orderedListOfInspectionReports.SortDirection;
            orderedListOfReportsWherePageWasSelectedByUser.SortField = orderedListOfInspectionReports.SortField;
            orderedListOfReportsWherePageWasSelectedByUser.TruckNumberId = orderedListOfInspectionReports.TruckNumberId;


            return orderedListOfReportsWherePageWasSelectedByUser;
        }

        public ReportListViewModel RetrieveInspectionReportsListSortedByUserRequest(ReportListViewModel userRequests)
        {
            //Get list of reports from Database by Truck Number and add it to ReportList Class coming from Controller
            userRequests.ReportsList = RetrieveFromDataBaseListOfReportsByTruckNumber(userRequests.TruckNumberId);

            //Calculate number of pages
            userRequests.PageCount = ReturnNumberOfPages(userRequests);

            //Create a new ordered list of reports based on user's request
            var orderedListOfInspectionReports = RetrieveInspectionReportListInAscendingOrDescendingOrder(userRequests);
            
            //Once list of reports ordered per user request, return the ordered list to the page
            //where the user requested to order the data
            return ReturnUserToPageWhereOrderByWasTriggeredWithListOfReportsSorted(orderedListOfInspectionReports);
        }

    }
}
