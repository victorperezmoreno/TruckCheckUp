namespace TruckCheckUp.Core.Contracts.InputValidation
{
    public interface IValidateUserInput
    {
        bool Alphanumeric(string inputToValidate);
        bool Numeric(string inputToValidate);
    }
}