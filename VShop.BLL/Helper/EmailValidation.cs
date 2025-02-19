using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VShop.DAL.Models.Db;

namespace VShop.BLL.Helper
{
    public class EmailValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if(value != null)
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                var email = value.ToString();
                Match match = regex.Match(email);
                return match.Success;
            }
            return false;
        }
    }
}
