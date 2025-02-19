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
        private readonly string _password;
        public ConfirmPasswordValidation(string password)
        {
            _password = password;
            ErrorMessage = "Password do not match";
        }

        public override bool IsValid(object? value)
        {
            var passwordProperty = GetType().GetProperty(_password)?.GetValue(this,null) as string;
            var confirmPassword = value as string;
            if (confirmPassword != null) 
            {
                return passwordProperty!.Equals(confirmPassword);
            }
            return false;
        }
    }
}
