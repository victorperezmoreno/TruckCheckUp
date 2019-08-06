using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TruckCheckUp.Core.Contracts.Services;
using TruckCheckUp.Core.ViewModels.InspectionReportUI;
using TruckCheckUp.Services;

namespace TruckCheckUp.WebUI.Controllers
{
    public class InspectionReportController : Controller
    {
        private IInspectionReportService _inspectionReportService;

        public InspectionReportController(IInspectionReportService inspectionReportService)
        {
            _inspectionReportService = inspectionReportService;
        }

        // GET: InspectionReport
        public ActionResult Index()
        {
            try
            {
                var reportViewModel = _inspectionReportService.ReturnNewTruckNumberViewModelObject();
                return View(reportViewModel);
            }
            catch (Exception)
            {
                throw;
            } 
        }

        //Retrieve results based on the selection from user
        //[OutputCache(CacheProfile = "CredibleCache1Week", VaryByParam = "userSortingRequest", Location = OutputCacheLocation.Server, NoStore = false)]
        public ActionResult RetrieveInspectionReportsListSortedByUserRequest(ReportListViewModel userSortingRequest)
        {  
            try
            {
                //Order list of students based on user "orderby" column selection by user
                var InspectionReportListToDisplayInView = _inspectionReportService.RetrieveInspectionReportsListSortedByUserRequest(userSortingRequest);

                return PartialView("_InspectionReportsList", InspectionReportListToDisplayInView);
            }
            catch (Exception)
            {
                throw;
            }  
        }

        public ActionResult ReportDetails(string Id)
        {
            try
            {
                var inspectionReport = _inspectionReportService.RetrieveCompleteInspectionReportForTruckNumberSelectedbyUser(Id);
                return View(inspectionReport);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}