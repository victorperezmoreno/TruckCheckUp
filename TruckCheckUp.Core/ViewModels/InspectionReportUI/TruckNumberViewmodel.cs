using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.ViewModels.InspectionReportUI
{
    public class TruckNumberViewmodel
    {
        //These properties handle the sorting, paging and filtering data in table
        public string SortField { get; set; }
        public string SortDirection { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int CurrentPageIndex { get; set; }
        public string SearchString { get; set; }

        //Selected item in Truck number DropDownList
        [Required]
        public string TruckNumberId { get; set; }

        //List of truck numbers in Truck Number DropDownList
        [Display(Name = "Truck Number")]
        public IEnumerable<DropDownListViewModel> TruckNumbers { get; set; }
    }
}
