using System.ComponentModel.DataAnnotations;

namespace TaskManagerServer.Models.Validations
{
    public class TaskValidationAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var task = validationContext.ObjectInstance as TaskModel;

            if(task == null)
            {
                return new ValidationResult("Something went wrong");
            }

            if(string.IsNullOrWhiteSpace(task.Task))
            {
                return new ValidationResult("Task is empty");
            }

            if(task.AccountId < 0)
            {
                return new ValidationResult("Wrong account id");
            }
            if(task.SortId < 0)
            {
                return new ValidationResult("SortId cannot be less than 0");
            }

            return ValidationResult.Success;
        }
    }
}
