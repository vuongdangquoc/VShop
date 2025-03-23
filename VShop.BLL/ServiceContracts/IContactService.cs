using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VShop.BLL.DTO;

namespace VShop.BLL.ServiceContracts
{
    public interface IContactService
    {
        public Task<bool> AddContactAsync(AddContactDTO addContactDTO);
    }
}
