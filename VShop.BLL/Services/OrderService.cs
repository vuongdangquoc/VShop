using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VShop.BLL.DTO;
using VShop.BLL.ServiceContracts;
using VShop.DAL.Models.Db;
using VShop.DAL.UnitOfWork;

namespace VShop.BLL.Services
{
    public class OrderService : IOrderService
    {

        public string[] PaymentType = { "VNPay", "Thanh toán khi nhận hàng" };
        public string[] OrderStatus = { "Đã hủy", "Chờ thanh toán", "Đang giao hàng", "Hoàn tất" };
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<string> Checkout(OrderDTO orderDTO, List<ProductCartDTO> products)
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var orderDetails = new List<OrderDetail>();
                    foreach (var product in products)
                    {
                        var temp = await _unitOfWork.ProductRepository.CheckExistProductAsync(product.Id);
                        if (!temp)
                        {
                            await transaction.RollbackAsync();
                            return "";
                        }
                        var orderDetail = new OrderDetail
                        {
                            ProductId = product.Id,
                            Price = product.Price.GetValueOrDefault(0),
                            Quantity = product.Count
                        };
                        orderDetails.Add(orderDetail);
                    }
                    var order = new Order
                    {
                        FullName = orderDTO.FullName,
                        Email = orderDTO.Email,
                        PhoneNumber = orderDTO.PhoneNumber,
                        Address = orderDTO.Address,
                        Note = orderDTO.Note,
                        OrderDate = DateTime.Now,
                        Status = orderDTO.TypePayment == 1? 1: 0,
                        UserId = orderDTO.UserId,
                        District = orderDTO.District,
                        Province = orderDTO.Province,
                        Ward = orderDTO.Ward,
                        VoucherId = orderDTO.VoucherId,
                        DeliveryFee = 20000,
                        TypePayment = orderDTO.TypePayment,
                        OrderDetails = orderDetails,
                    };
                    _unitOfWork.OrderRepository.Add(order);
                    if (orderDTO.VoucherId != null)
                    {
                        await _unitOfWork.VoucherRepository.ReduceQuantityVoucher(orderDTO.VoucherId.Value);
                    }
                    foreach (var product in products)
                    {
                        await _unitOfWork.ProductRepository.ReduceQuantityProduct(product.Id,product.Count);
                    }
                    var result = await _unitOfWork.SaveChangesAsync();
                    if (result > 0)
                    {
                        await transaction.CommitAsync();
                        return PaymentType[order.TypePayment];
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        return "";
                    }

                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return "";
                }

            }

        }

        public async Task<bool> CheckPaymentSuccess(Guid userId, string vnp_ResponseCode)
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                var order = await _unitOfWork.OrderDetailRepository.GetOrderByUserIdAsync(userId);
                if (order == null)
                {
                    return false;
                }               
                if (vnp_ResponseCode == "00") // "00" là thanh toán thành công
                {
                    order.First().Order.Status = 1;
                    await _unitOfWork.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                else
                {
                    // Hoàn lại số lượng hàng nếu thanh toán thất bại
                    order.First().Order.Status = 0;
                    foreach (var o in order)
                    {
                        o.Product.Quantity += o.Quantity;
                    }
                    await _unitOfWork.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                return false;

            }
        }

        public async Task<GetOrderByUserIdDTO?> GetNewByUserID(Guid userId)
        {
            var order = await _unitOfWork.OrderRepository.GetNewByUserID(userId);
            if(order != null)
            {
                GetOrderByUserIdDTO orderByUserIdDTO = new GetOrderByUserIdDTO()
                {
                    ID = order.Id,
                    Email = order.Email,
                    FullName = order.FullName,
                    PhoneNumber = order.PhoneNumber,
                    ProvinceName = order.Province,
                    DistrictName = order.District,
                    WardName = order.Ward,
                    Address = order.Address,
                    DeliveryFee = order.DeliveryFee,
                    OrderDate = order.OrderDate,
                    Note = order.Note,
                    Status = order.Status,
                    TypePayment = order.TypePayment,

                    Voucher = order.Voucher,
                    UserID = order.UserId.Value,
                    OrderDetails = order.OrderDetails.Select(od => new OrderDetail()
                    {
                        ProductId = od.ProductId,
                        Product = _unitOfWork.ProductRepository.GetProductById(od.ProductId),
                        OrderId = od.OrderId,
                        Price = od.Price,
                        Quantity = od.Quantity

                    }).ToList()
                };
                return orderByUserIdDTO;
            }
            return null;
        }
    }
}
