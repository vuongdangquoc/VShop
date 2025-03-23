using Microsoft.EntityFrameworkCore;
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

        public async Task DeleteVoucher(int id)
        {
            var v = await _context.Vouchers.SingleOrDefaultAsync(x => x.Id == id);
            _context.Vouchers.Remove(v);
        }

        public async Task<IEnumerable<Voucher>> GetAllVoucherAsync(string? search, bool? status, DateTime? start, DateTime? end)
        {
            var query = _context.Vouchers
                                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.DiscountCode.Contains(search));
            }

            if (status != null)
            {
                query = query.Where(p => p.Status == status);
            }

            if(start != null)
            {
                query = query.Where(p => p.StartDate.Value > start);
            }

            if (end != null)
            {
                query = query.Where(p => p.EndDate.Value < end);
            }

            return await query.ToListAsync();
        }

        public async Task<Voucher> GetVoucherByIdAsync(int Id)
        {
            var v = await _context.Vouchers.SingleOrDefaultAsync(p => p.Id == Id);
            return v;
        }

        public async Task<bool> ReduceQuantityVoucher(int idVoucher)
        {
            var voucherById = await _context.Vouchers.SingleOrDefaultAsync(v => v.Id == idVoucher);

            if (voucherById != null)
            {
                voucherById.Quantity -= 1;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
