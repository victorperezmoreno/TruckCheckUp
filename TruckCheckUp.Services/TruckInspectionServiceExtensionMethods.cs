using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruckCheckUp.Contracts.Services;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.ViewModels.TruckInspection;

namespace TruckCheckUp.Services
{
    public class TruckInspectionServiceExtensionMethods : ITruckInspectionServiceExtensionMethods
    {
        public List<DropDownListViewModel> ConvertDriverNamesToDropDownListView(List<Driver> driversRetrievedFromDB)
        {
            if (driversRetrievedFromDB != null && driversRetrievedFromDB.Any())
            {
                return driversRetrievedFromDB.Select(driver => new DropDownListViewModel
                {
                    Id = driver.Id,
                    Text = driver.FullName
                }).ToList();
            }
            return new List<DropDownListViewModel>();
        }

        public List<DropDownListViewModel> ConvertTruckNumbersToDropDownListView(List<Truck> trucksRetrievedFromDB)
        {
            if (trucksRetrievedFromDB != null && trucksRetrievedFromDB.Any())
            {
                return trucksRetrievedFromDB.Select(truck => new DropDownListViewModel
                {
                    Id = truck.Id,
                    Text = truck.TruckNumber.ToString()
                }).ToList();
            }
            return new List<DropDownListViewModel>();
        }

        public List<CheckBoxListViewModel> ConvertGeneralCatalogToCheckBoxListView(List<PartCatalog> generalCatalogFromDB)
        {
            if (generalCatalogFromDB != null && generalCatalogFromDB.Any())
            {
                return generalCatalogFromDB.Select(generalPart => new CheckBoxListViewModel
                {
                    Id = generalPart.Id,
                    Name = generalPart.PartName,
                    IsChecked = true
                }).ToList();
            }
            return new List<CheckBoxListViewModel>();
        }

        public List<CheckBoxListViewModel> ConvertLightsCatalogToCheckBoxListView(List<PartCatalog> lightsCatalogFromDB)
        {
            if (lightsCatalogFromDB != null && lightsCatalogFromDB.Any())
            {
                return lightsCatalogFromDB.Select(light => new CheckBoxListViewModel
                {
                    Id = light.Id,
                    Name = light.PartName,
                    IsChecked = true
                }).ToList();
            }
            return new List<CheckBoxListViewModel>();
        }

        public List<CheckBoxListViewModel> ConvertFluidsCatalogToCheckBoxListView(List<PartCatalog> fluidsCatalogFromDB)
        {
            if (fluidsCatalogFromDB != null && fluidsCatalogFromDB.Any())
            {
                return fluidsCatalogFromDB.Select(fluid => new CheckBoxListViewModel
                {
                    Id = fluid.Id,
                    Name = fluid.PartName,
                    IsChecked = true
                }).ToList();
            }
            return new List<CheckBoxListViewModel>();
        }
    }
}
