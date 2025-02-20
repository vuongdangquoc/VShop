using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VShop.BLL.Helper
{
    public class ConfirmPasswordValidation : ValidationAttribute
    {
        private readonly string _passwordPropertyName;

        public ConfirmPasswordValidation(string passwordPropertyName)
        {
            _passwordPropertyName = passwordPropertyName;
            ErrorMessage = "Password do not match";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var passwordProperty = validationContext.ObjectType.GetProperty(_passwordPropertyName);
            if (passwordProperty == null)
            {
                return new ValidationResult($"Property '{_passwordPropertyName}' not found.");
            }

            var passwordValue = passwordProperty.GetValue(validationContext.ObjectInstance) as string;
            var confirmPasswordValue = value as string;

            if (passwordValue != confirmPasswordValue)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
