using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VShop.BLL.DTO;

namespace VShop.BLL.ServiceContracts
{
    public interface IOrderService
    {
        public Task<string> Checkout(OrderDTO orderDTO,List<ProductCartDTO> products);

        public Task<bool> CheckPaymentSuccess(Guid userId, string vnp_ResponseCode);

        public Task<GetOrderByUserIdDTO?> GetNewByUserID(Guid userId);
    }
}
