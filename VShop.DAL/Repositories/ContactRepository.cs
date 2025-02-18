using VShop.DAL.Models.Db;
using VShop.DAL.RepositoryContracts;

namespace VShop.DAL.Repositories
{
    public class ContactRepository : GenericRepository<Contact>, IContactRepository
    {
        private readonly VShopContext _context;

        public ContactRepository(VShopContext db) : base(db)
        {
            _context = db;
        }
    }
}
