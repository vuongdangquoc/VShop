using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VShop.DAL.Models.Db;

namespace VShop.BLL.ServiceContracts
{
    public interface IVoucherService
    {
        public Task<bool> Create(Voucher voucher);
        public Task<bool> Update(Voucher voucher);
        public Task<bool> Delete(int id);
        Task ChangeStatus(int id);

        Task<IEnumerable<Voucher>> GetAllVoucherAsync(string? search, bool? status, DateTime? start, DateTime? end);
    }
}
