using VShop.DAL.Models.Db;
using VShop.DAL.RepositoryContracts;

namespace VShop.DAL.Repositories
{
    public class VoucherRepository : GenericRepository<Voucher>, IVoucherRepository
    {
        private readonly VShopContext _context;

        public VoucherRepository(VShopContext db) : base(db)
        {
            _context = db;
        }
    }
}
