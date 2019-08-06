using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.ViewModels.InspectionReportUI
{
    public class ReportListViewModel
    {
        //These properties handle the sorting, paging and filtering data in table
        public string SortField { get; set; }
        public string SortDirection { get; set; }
        public int PageSize { get; } = 10; //Change if user need to see more records in table
        public int PageCount { get; set; }
        public int CurrentPageIndex { get; set; }
        public string TruckNumberId { get; set; }

        public IEnumerable<ReportsViewModel> ReportsList { get; set; }
    }
}
