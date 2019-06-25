using System.Text.RegularExpressions;

namespace TruckCheckUp.Core.Contracts.InputValidation
{
    public class ValidateUserInput : IValidateUserInput
    {
        public bool Alphanumeric(string inputToValidate)
        {
            Regex regexDefinition = new Regex(@"^[a-zA-Z0-9]+$");
            return (regexDefinition.IsMatch(inputToValidate));
        }

        public bool Numeric(string inputToValidate)
        {
            Regex regexDefinition = new Regex(@"^[0-9]+$");
            return (regexDefinition.IsMatch(inputToValidate));
        }
    }
}
