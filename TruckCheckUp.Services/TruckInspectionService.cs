using System;
using System.Collections.Generic;
using System.Linq;
using TruckCheckUp.Contracts.Services;
using TruckCheckUp.Core.Contracts.DataAccess;
using TruckCheckUp.Core.Contracts.Logger;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.ViewModels;
using TruckCheckUp.Core.ViewModels.TruckInspection;

namespace TruckCheckUp.Services
{
    public class TruckInspectionService : ITruckInspectionService
    {
        private IRepository<PartCatalog> _partCatalogContext;
        private IRepository<TruckInspection> _truckInspectionContext;
        private IRepository<Driver> _driverContext;
        private IRepository<Truck> _truckContext;
        private IRepository<DriverComment> _driverCommentContext;
        private ILogger _logger;
        private string truckInspectionTableNameUsedByLogger;
        private string driverCommentsTableNameUsedByLogger;

        public TruckInspectionService(IRepository<PartCatalog> partCatalogContext, IRepository<TruckInspection> truckInspectionContext,
                                      IRepository<Driver> driverContext, IRepository<Truck> truckContext,
                                      IRepository<DriverComment> driverCommentContext, ILogger logger)
        {
            _partCatalogContext = partCatalogContext;
            _truckInspectionContext = truckInspectionContext;
            _driverContext = driverContext;
            _truckContext = truckContext;
            _driverCommentContext = driverCommentContext;
            _logger = logger;
            truckInspectionTableNameUsedByLogger = "TruckInspection";
            driverCommentsTableNameUsedByLogger = "DriverComments";
        }

        public TruckInspectionViewModel CreateNewTruckInspectionObject()
        {
            return PopulateDriversTrucksAndPartsCatalog(new TruckInspectionViewModel());
        }

        public TruckInspectionViewModel PopulateDriversTrucksAndPartsCatalog(TruckInspectionViewModel inspectionToPopulate)
        {
            inspectionToPopulate.DriverList = ConvertDriverNamesToDropDownListView(_driverContext.Collection().OrderBy(n => n.FirstName).ToList());
            inspectionToPopulate.TruckList = ConvertTruckNumbersToDropDownListView(_truckContext.Collection().OrderBy(t => t.TruckNumber).ToList());
            inspectionToPopulate.GeneralCatalog = ConvertGeneralCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(g => g.PartCategoryId == "607412fa-729b-4dfd-94ee-9a8d84d1eed8").OrderBy(p => p.PartName).ToList());
            inspectionToPopulate.LightsCatalog = ConvertLightsCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(l => l.PartCategoryId == "43d29546-83c4-4a92-bafe-262aa58d6496").OrderBy(c => c.PartName).ToList());
            inspectionToPopulate.FluidsCatalog = ConvertFluidsCatalogToCheckBoxListView(_partCatalogContext.Collection().Where(f => f.PartCategoryId == "3897c106-7357-4ab8-b594-02020a7ce9c6").OrderBy(c => c.PartName).ToList());

            return inspectionToPopulate;
        }

        public TruckInspectionViewModel CreateTruckInspection(TruckInspectionViewModel inspectionToCreate)
        {
            var inspectionToValidate = CreateANewTruckInspectionObjectWithDataFromDBThatHasToBeValidated(inspectionToCreate);
            if (ValidateTruckInspectionData(inspectionToValidate))
            {
                PostNewTruckInspectionToDB(inspectionToValidate);
                AddSuccessMessage(inspectionToValidate, "Success!!", true);
            }
            else
            {
                if (InspectionReportHasNOTBeenSubmittedForThisTruckToday(inspectionToValidate) == false)
                {
                    AddDangerMessage(inspectionToValidate, "Report already submitted for this truck", true);
                }
            }
            return inspectionToValidate;
        }

        private TruckInspectionViewModel CreateANewTruckInspectionObjectWithDataFromDBThatHasToBeValidated(TruckInspectionViewModel inspectionObjectFromView)
        {
            inspectionObjectFromView.LastMileageReported = RetrieveLastMileageReportedPerTruckAndDriver(inspectionObjectFromView);
            inspectionObjectFromView.LastTimeAReportWasSubmitted = RetrieveDateLatestReportWasSubmittedForThisTruck(inspectionObjectFromView);
            //Read maximum ticket number in TruckInspection Table and add 1 to generate new one
            //inspectionObjectFromView.ReportId = RetrieveLastReportId(inspectionObjectFromView);

            var NewTruckInspectionObject = new TruckInspectionViewModel()
            {
                DriverId = inspectionObjectFromView.DriverId,
                TruckId = inspectionObjectFromView.TruckId,
                CurrentMileage = inspectionObjectFromView.CurrentMileage,
                LastMileageReported = inspectionObjectFromView.LastMileageReported,
                ReportId = inspectionObjectFromView.ReportId,
                LastTimeAReportWasSubmitted = inspectionObjectFromView.LastTimeAReportWasSubmitted,
                DriverList = inspectionObjectFromView.DriverList,
                TruckList = inspectionObjectFromView.TruckList,
                GeneralCatalog = inspectionObjectFromView.GeneralCatalog,
                LightsCatalog = inspectionObjectFromView.LightsCatalog,
                FluidsCatalog = inspectionObjectFromView.FluidsCatalog,
                Comments = inspectionObjectFromView.Comments
            };

            return NewTruckInspectionObject;
        }

        private bool ValidateTruckInspectionData(TruckInspectionViewModel inspectionToValidate)
        {
            if (InspectionReportHasNOTBeenSubmittedForThisTruckToday(inspectionToValidate) && MileageReportedIsGreaterThanPreviousOne(inspectionToValidate))
            {               
                return true;
            }
            
            return false;
        }

        private void AddSuccessMessage(TruckInspectionViewModel inspectionToCreate, string message, bool dismissable)
        {
            inspectionToCreate.AlertStyle = AlertStyle.Success;
            inspectionToCreate.Message = message;
            inspectionToCreate.Dismissable = dismissable;
        }

        private void AddDangerMessage(TruckInspectionViewModel inspectionCreated, string message, bool dismissable)
        {
            inspectionCreated.AlertStyle = AlertStyle.Danger;
            inspectionCreated.Message = message;
            inspectionCreated.Dismissable = dismissable;
        }

        private bool MileageReportedIsGreaterThanPreviousOne(TruckInspectionViewModel mileageToValidate)
        {
            var intCurrentMileage = Convert.ToInt32(mileageToValidate.CurrentMileage);
            if (intCurrentMileage > mileageToValidate.LastMileageReported)
            {
                return true;
            }
            return false;
        }

        private bool InspectionReportHasNOTBeenSubmittedForThisTruckToday(TruckInspectionViewModel InspectionObject)
        {
            if (DateTime.Today.Date > InspectionObject.LastTimeAReportWasSubmitted)
            {
                return true;
            }
            return false;
        }

        private void PostNewTruckInspectionToDB(TruckInspectionViewModel inspectionFromUser)
        {
            //Insert data from General Catalog into TruckInspection Table 
            AddCatalogItemsIntoTruckInspectionTable(inspectionFromUser, inspectionFromUser.GeneralCatalog);

            //Insert data from Lights Catalog into TruckInspection Table 
            AddCatalogItemsIntoTruckInspectionTable(inspectionFromUser, inspectionFromUser.LightsCatalog);

            //Insert data from Fluids Catalog into TruckInspection Table
            AddCatalogItemsIntoTruckInspectionTable(inspectionFromUser, inspectionFromUser.FluidsCatalog);

            //Log TruckInspection insertion notification and save the data into DB
            _logger.Info("Inserted report number " + inspectionFromUser.ReportId.ToString() + " into Table " + truckInspectionTableNameUsedByLogger);
            _truckInspectionContext.Commit();

            //Insert comments from Driver into DriverComment table
            AddTruckInspectionCommentsToDriverCommentsTable(inspectionFromUser);

            //Log Comments insertion notification and save the data into DB
            _logger.Info("Inserted report number " + inspectionFromUser.ReportId.ToString() + " into Table " + driverCommentsTableNameUsedByLogger);
            _driverCommentContext.Commit();
        }

        private void AddCatalogItemsIntoTruckInspectionTable(TruckInspectionViewModel inspectionObject, List<CheckBoxListViewModel> partsCatalog)
        {
            foreach (CheckBoxListViewModel part in partsCatalog)
            {
                var truckInspectionToInsert = new TruckInspection();
                truckInspectionToInsert.DriverId = inspectionObject.DriverId;
                truckInspectionToInsert.TruckId = inspectionObject.TruckId;
                truckInspectionToInsert.Mileage = Convert.ToInt32(inspectionObject.CurrentMileage);
                //truckInspectionToInsert.IsOK = part.IsChecked;
                //truckInspectionToInsert.ReportId = inspectionObject.ReportId;
                //truckInspectionToInsert.PartCatalogId = part.Id;
                _truckInspectionContext.Insert(truckInspectionToInsert);
            }
        }

        private void AddTruckInspectionCommentsToDriverCommentsTable(TruckInspectionViewModel truckInspection)
        {
            var driverComments = new DriverComment();
            driverComments.CommentDriver = truckInspection.Comments;
            driverComments.ReportId = truckInspection.ReportId;
            _driverCommentContext.Insert(driverComments);
        }

        private List<DropDownListViewModel> ConvertDriverNamesToDropDownListView(List<Driver> driversRetrievedFromDB)
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

        private List<DropDownListViewModel> ConvertTruckNumbersToDropDownListView(List<Truck> trucksRetrievedFromDB)
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

        private List<CheckBoxListViewModel> ConvertGeneralCatalogToCheckBoxListView(List<PartCatalog> generalCatalogFromDB)
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

        private List<CheckBoxListViewModel> ConvertLightsCatalogToCheckBoxListView(List<PartCatalog> lightsCatalogFromDB)
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

        private List<CheckBoxListViewModel> ConvertFluidsCatalogToCheckBoxListView(List<PartCatalog> fluidsCatalogFromDB)
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

        private int RetrieveLastMileageReportedPerTruckAndDriver(TruckInspectionViewModel inspectionFromUser)
        {
            return _truckInspectionContext.Collection().Where(t => t.TruckId == inspectionFromUser.TruckId).DefaultIfEmpty().Max(m => m == null ? 0 : m.Mileage);
        }

        private DateTime RetrieveDateLatestReportWasSubmittedForThisTruck(TruckInspectionViewModel inspectionFromUser)
        {
            return _truckInspectionContext.Collection().Where(d => d.TruckId == inspectionFromUser.TruckId).DefaultIfEmpty().Max(d => d == null ? DateTime.Now : d.CreationDate).Date;
        }

        //private int RetrieveLastReportId(TruckInspectionViewModel inspectionFromUser)
        //{
        //    return (_truckInspectionContext.Collection().DefaultIfEmpty().Max(t => t == null ? 0 : t.ReportId)) +1;
        //}
    }
}
