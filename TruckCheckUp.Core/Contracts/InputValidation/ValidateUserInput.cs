﻿using System.Text.RegularExpressions;

namespace TruckCheckUp.Core.Contracts.InputValidation
{
    public class ValidateUserInput : IValidateUserInput
    {
        public ValidateUserInput()
        {

        }

        public bool Alphanumeric(string inputToValidate)
        {
            Regex regexDefinition = new Regex(@"^[a-zA-Z0-9]+$");
            return (regexDefinition.IsMatch(inputToValidate));
        }
    }
}
