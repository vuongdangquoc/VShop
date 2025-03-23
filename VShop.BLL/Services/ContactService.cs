using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VShop.BLL.DTO;
using VShop.BLL.ServiceContracts;
using VShop.DAL.Models.Db;
using VShop.DAL.UnitOfWork;

namespace VShop.BLL.Services
{
    public class ContactService : IContactService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContactService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> AddContactAsync(AddContactDTO addContactDTO)
        {
            Contact contact = new Contact()
            {
                FullName = addContactDTO.FullName,
                Email = addContactDTO.Email,
                PhoneNumber = addContactDTO.PhoneNumber,
                Content = addContactDTO.Content,
            };
            _unitOfWork.ContactRepository.Add(contact);
            var result = await _unitOfWork.SaveChangesAsync();
            return result>0;
        }
    }
}
