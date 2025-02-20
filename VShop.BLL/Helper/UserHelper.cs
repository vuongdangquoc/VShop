using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VShop.BLL.DTO;
using VShop.DAL.Models.Db;

namespace VShop.BLL.Helper
{
    public class UserHelper
    {
        public static string ConvertToStatusString(int status)
        {
            switch (status)
            {
                case 0:
                    return "InActive";
                case 1:
                    return "Active";
                case 2:
                    return "Banned";
            }
            return "Unknown";
        }

        public static int ConvertToStatusInt(string status)
        {
            if (status.Equals("InActive"))
            {
                return 0;
            }
            else if (status.Equals("Active"))
            {
                return 1;
            }
            else if (status.Equals("Banned"))
            {
                return 2;
            }
            return -1;
        }        
    }
}
