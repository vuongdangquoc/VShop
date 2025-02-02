using VShop.Models.Db;
using VShop.RepositoryContracts;

namespace VShop.Repositories
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
