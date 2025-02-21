using Microsoft.EntityFrameworkCore;
using VShop.DAL.Enums;
using VShop.DAL.Models.Db;
using VShop.BLL.DTO;
using VShop.BLL.ServiceContracts;
using VShop.DAL.UnitOfWork;
using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace VShop.BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; 
        }

        public async Task<bool> CheckProductExist(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);
            return product != null;
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
            var listProductId = await _unitOfWork.OrderDetailRepository.GetListBestSellingProductIdAsync(6,category);
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
            var list = await _unitOfWork.ProductRepository.GetNewArrialsAsync(6,category);
            if (list.Count == 0)
            {
                return null;
            }
            var result = convertListProductToListProductViewModel(list);    
            return result;
        }

        public async Task<ProductDTO?> GetProductById(int id)
        {
           var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);
            if (product == null) 
            {
                return null;
            }
            var productDTO = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Quantity = product.Quantity,
                Image = product.Image,
                Discount = (int)product.Discount,
                CurrentPrice = product.Discount != 0 ? (convertToVND(product.Price * (100 - product.Discount) / 100)) : (convertToVND(product.Price)),
                OldPrice = product.Discount != 0 ? (convertToVND(product.Price)) : null,
                Describe = product.Describe,
                CategoryName = product.Category.Name,
                ListImages = JsonConvert.DeserializeObject<List<string>>(product.ListImages)
            };
            return productDTO;
        }

        public async Task<List<ProductCartDTO>> GetProductsInCart(List<CartCookieDTO> items)
        {
            var listProduct = await _unitOfWork.ProductRepository.GetProductsFromListProductIdAsync(items.Select(x => x.ProductId).ToList());
            var result = new List<ProductCartDTO>();
            result = listProduct.Select(product => new ProductCartDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price - (product.Discount * product.Price/100),
                Image = product.Image,
                Count = items.Single(x => x.ProductId == product.Id).Count,
                SumPrice = product.Price - (product.Discount * product.Price/100) * items.Single(x => x.ProductId == product.Id).Count
            }).ToList();
            return result;
        }

        private List<ProductDTO> convertListProductToListProductViewModel(List<Product> list)
        {
            var result = list.Select(x => new ProductDTO
            {
                Id = x.Id,
                Name = x.Name,
                Image = x.Image,
                Discount = (int)x.Discount,
                CurrentPrice = x.Discount != 0 ? (convertToVND(x.Price * (100 - x.Discount) / 100)) : (convertToVND(x.Price)),
                OldPrice = x.Discount != 0 ? (convertToVND(x.Price)) : null,
                Describe = x.Describe,
                CategoryName = x.Category.Name
            }).ToList();
            return result;
        }
        private string convertToVND(double s)
        {
            return s.ToString("C0", new System.Globalization.CultureInfo("vi-VN"));
        }
    }
}
