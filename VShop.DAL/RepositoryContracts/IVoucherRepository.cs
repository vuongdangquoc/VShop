using VShop.DAL.Models.Db;

namespace VShop.DAL.RepositoryContracts
{
    public interface IVoucherRepository : IGenericRepository<Voucher>
    {
        public Task<bool> ReduceQuantityVoucher(int idVoucher);

        public Task<Voucher> GetVoucherByIdAsync(int Id);

        Task<IEnumerable<Voucher>> GetAllVoucherAsync(string? search, bool? status, DateTime? start, DateTime? end);

        Task DeleteVoucher(int id);
    }
}
