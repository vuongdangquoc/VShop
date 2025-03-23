using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VShop.BLL.ServiceContracts;
using VShop.DAL.Models.Db;
using VShop.DAL.RepositoryContracts;
using VShop.DAL.UnitOfWork;

namespace VShop.BLL.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VoucherService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ChangeStatus(int id)
        {
            var v = await _unitOfWork.VoucherRepository.GetVoucherByIdAsync(id);
            if (v == null) return;
            v.Status = !v.Status;
            _unitOfWork.VoucherRepository.Update(v);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> Create(Voucher voucher)
        {
            _unitOfWork.VoucherRepository.Add(voucher);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> Delete(int id)
        {
            await _unitOfWork.VoucherRepository.DeleteVoucher(id);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<Voucher>> GetAllVoucherAsync(string? search, bool? status, DateTime? start, DateTime? end)
        {
            return await _unitOfWork.VoucherRepository.GetAllVoucherAsync(search, status, start, end);
        }

        public async Task<bool> Update(Voucher voucher)
        {
            var v = await _unitOfWork.VoucherRepository.GetVoucherByIdAsync(voucher.Id);
            if (v == null) return false;
            v.DiscountCode = voucher.DiscountCode;  
            v.DiscountValue = voucher.DiscountValue;
            v.MinimumValue = voucher.MinimumValue;
            v.Quantity = voucher.Quantity;
            v.StartDate = voucher.StartDate;
            v.EndDate = voucher.EndDate;
            v.Describe = voucher.Describe;
            v.Status = voucher.Status;
            v.IsDiscountPercentage = voucher.IsDiscountPercentage;
            v.UpdatedBy = voucher.UpdatedBy;
            v.UpdatedDate = voucher.UpdatedDate;

            _unitOfWork.VoucherRepository.Update(v);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }
    }
}
