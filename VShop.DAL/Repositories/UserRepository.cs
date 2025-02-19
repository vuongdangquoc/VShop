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

        public async Task<string?> GetRoleByEmail(string email)
        {
            var user = await  _context.Users.Include(x => x.Role).SingleOrDefaultAsync(x => x.Email.Equals(email));
            if(user != null)
            {
                return user.Role.Name;
            }
            return null;
            
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email.Equals(email));
            return user;
        }
    }
}
