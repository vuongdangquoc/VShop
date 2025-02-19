using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VShop.BLL.DTO;

namespace VShop.BLL.ServiceContracts
{
    public interface IUserService
    {
        public Task<bool> Login(LoginDTO loginDTO);
        public Task<bool> Register(RegisterDTO registerDTO);
    }
}
