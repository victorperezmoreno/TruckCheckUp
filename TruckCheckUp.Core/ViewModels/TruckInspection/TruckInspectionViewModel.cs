using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TruckCheckUp.Core.Models;
using TruckCheckUp.ViewModels.TruckInspection;

namespace TruckCheckUp.Core.ViewModels.TruckInspection
{
    public class TruckInspectionViewModel : Response
    {
        //To save selected driver Id from dropdownlist
        [Required]
        public string DriverId { get; set; }
        //To save selected truck Id from dropdownlist
        [Required]
        public string TruckId { get; set; }
        //To save truck mileage from textbox
        //[ValidTruckMileage] 
        [Required]
        //[Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Only positive numbers allowed")]
        [Display(Name = "Mileage")]
        public string CurrentMileage { get; set; }

        public int LastMileageReported { get; set; }
        
        public int TicketNumber { get; set; }

        public DateTimeOffset LastTimeAReportWasSubmitted { get; set; }

        //To populate Crew Leader DropDownList in view
        [Display(Name = "Driver")]
        public List<DropDownListViewModel> DriverList { get; set; }

        //To populate Truck # DropDownList in view
        [Display(Name = "Truck")]
        public List<DropDownListViewModel> TruckList { get; set; }
        
        //List of General parts to display as checkboxes 
        public List<CheckBoxListViewModel> GeneralCatalog { get; set; }

        //List of Lights parts to display as checkboxes 
        public List<CheckBoxListViewModel> LightsCatalog { get; set; }

        //List of Fluid parts to display as checkboxes 
        public List<CheckBoxListViewModel> FluidsCatalog { get; set; }

        //To save comments on parts reported
        [RegularExpression("^[\\w. ,'#&$~@!&quot;-]*$", ErrorMessage = "Only letters and numbers allowed")]
        public string Comments { get; set; }

        public TruckInspectionViewModel()
        {
            DriverList = new List<DropDownListViewModel>();
            TruckList = new List<DropDownListViewModel>();
            GeneralCatalog = new List<CheckBoxListViewModel>();
            LightsCatalog = new List<CheckBoxListViewModel>();
            FluidsCatalog = new List<CheckBoxListViewModel>();
        }
    }
}
