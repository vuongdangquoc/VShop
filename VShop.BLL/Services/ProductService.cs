using Microsoft.EntityFrameworkCore;
using VShop.DAL.Enums;
using VShop.DAL.Models.Db;
using VShop.BLL.DTO;
using VShop.BLL.ServiceContracts;
using VShop.DAL.UnitOfWork;

namespace VShop.BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; 
        }

        public async Task<List<ProductDTO>> GetAllProductsAsync(
             string? search,
             string? category,
             double? minPrice,
             double? maxPrice,                         
             SortBy sortBy,
             bool isDescending,
             int page,
             int pageSize = 9)
        {
            var list = (await _unitOfWork.ProductRepository.GetAllProductsAsync(search,category,minPrice,maxPrice,page,pageSize,sortBy,isDescending)).ToList();
            var result = convertListProductToListProductViewModel(list);
            return result;
        }
        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            var list = await _unitOfWork.ProductRepository.GetQuery().ToListAsync();
            var result = convertListProductToListProductViewModel(list);
            return result;
        }

        public async Task<List<ProductDTO>?> GetBestSellingProductsAsync(string category = "all")
        {
            var listProductId = await _unitOfWork.OrderDetailRepository.GetListBestSellingProductIdAsync(4,category);
            var list = await _unitOfWork.ProductRepository.GetProductsFromListProductIdAsync(listProductId);
            if (list.Count == 0)
            {
                return null;
            }
            var result = convertListProductToListProductViewModel(list);    
            return result;
        }

        public async Task<List<ProductDTO>?> GetNewArrivalsAsync(string category = "all")
        {
            var list = await _unitOfWork.ProductRepository.GetNewArrialsAsync(4,category);
            if (list.Count == 0)
            {
                return null;
            }
            var result = convertListProductToListProductViewModel(list);    
            return result;
        }

        private List<ProductDTO> convertListProductToListProductViewModel(List<Product> list)
        {
            var result = list.Select(x => new ProductDTO
            {
                Name = x.Name,
                Image = x.Image,
                Discount = (int)x.Discount,
                CurrentPrice = x.Discount != 0 ? (convertToVND(x.Price * (100 - x.Discount) / 100)) : (convertToVND(x.Price)),
                OldPrice = x.Discount != 0 ? (convertToVND(x.Price)) : null,
                Describe = x.Describe
            }).ToList();
            return result;
        }
        private string convertToVND(double s)
        {
            return s.ToString("C0", new System.Globalization.CultureInfo("vi-VN"));
        }
    }
}
