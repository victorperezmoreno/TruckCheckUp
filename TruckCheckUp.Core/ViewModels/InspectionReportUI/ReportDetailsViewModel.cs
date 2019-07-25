using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckCheckUp.Core.ViewModels.InspectionReportUI
{
    public class ReportDetailsViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Driver")]
        public string DriverName { get; set; }

        [Display(Name = "Truck")]
        public string TruckNumber { get; set; }

        [Display(Name = "Date")]
        public string SubmissionDate { get; set; }

        [Display(Name = "Mileage")]
        public string CurrentMileage { get; set; }

        //List of General parts to display as checkboxes 
        public List<CheckBoxListViewModel> GeneralCatalog { get; set; }

        //List of Lights parts to display as checkboxes 
        public List<CheckBoxListViewModel> LightsCatalog { get; set; }

        //List of Fluid parts to display as checkboxes 
        public List<CheckBoxListViewModel> FluidsCatalog { get; set; }

        public string Comments { get; set; }

        public ReportDetailsViewModel()
        {
            GeneralCatalog = new List<CheckBoxListViewModel>();
            LightsCatalog = new List<CheckBoxListViewModel>();
            FluidsCatalog = new List<CheckBoxListViewModel>();
        }
    }
}
