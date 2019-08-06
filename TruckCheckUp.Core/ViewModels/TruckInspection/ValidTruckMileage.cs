using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruckCheckUp.Core.Models;
using TruckCheckUp.Core.Contracts.DataAccess;
using TruckCheckUp.Core.ViewModels.TruckInspection;

namespace TruckCheckUp.ViewModels.TruckInspection
{
    public class ValidTruckMileage : ValidationAttribute
    {
       protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var truckModel = (TruckInspectionViewModel)validationContext.ObjectInstance;
            int currentMileageReported = Convert.ToInt32(value);
            int mileageEnteredInPreviousReport = Convert.ToInt32(truckModel.CurrentMileage);

            if (mileageEnteredInPreviousReport > currentMileageReported)
            {
                return new ValidationResult
                    ("Mileage entered must be greater than " + mileageEnteredInPreviousReport.ToString());
            }
            else if (currentMileageReported <= 0)
            {
                return new ValidationResult
                    ("Mileage reported must be greater than 0");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
