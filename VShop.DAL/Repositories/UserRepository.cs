using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VShop.DAL.Models.Db;
using VShop.DAL.RepositoryContracts;

namespace VShop.DAL.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly VShopContext _context;

        public UserRepository(VShopContext db) : base(db)
        {
            _context = db;
        }

        public async Task<bool> CheckEmailExistAsync(string email)
        {
            var result = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
            if(result == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckPhoneExistAsync(string phone)
        {
            var result = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber.Equals(phone));
            if (result == null)
            {
                return false;
            }
            return true;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.Include(x=>x.Role).SingleOrDefaultAsync(x => x.Email.Equals(email) && x.Status != 2);
            return user;
        }
    }
}
