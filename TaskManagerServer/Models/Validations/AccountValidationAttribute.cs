using System.ComponentModel.DataAnnotations;
using TaskManagerServer.BLL.Interfaces;

namespace TaskManagerServer.Models.Validations
{
    public class AccountValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var account = validationContext.ObjectInstance as AccountModel;
            if (account != null)
            {
                if (string.IsNullOrWhiteSpace(account.Username))
                    return new ValidationResult("Username field is empty");
                else if (string.IsNullOrWhiteSpace(account.Password))
                    return new ValidationResult("Password is empty");
            }
            else
            {
                return new ValidationResult("Something went wrong");
            }
            return ValidationResult.Success;
        }
    }
}
