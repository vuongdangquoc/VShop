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

        public async Task<bool> ChangeStatusProduct(int id, string updateBy)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);
            if (product == null) return false;
            product.Status = !product.Status;
            product.UpdatedBy = updateBy;
            _unitOfWork.ProductRepository.Update(product);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> CheckProductExist(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);
            return product!= null && product.Status;
        }

        public async Task<bool> CreateProduct(CreateProductDTO productDTO)
        {
            var product = new Product()
            {
                Name = productDTO.Name,
                CategoryId = productDTO.CategoryID,
                Quantity = productDTO.Quantity,
                Describe = productDTO.Describe,
                Image = productDTO.Image,
                ListImages = productDTO.ListImages,
                Price = productDTO.Price,
                Discount = productDTO.Discount,
                CreatedBy = productDTO.CreatedBy,
                CreatedDate = DateTime.Now,
                Status = productDTO.Status,
                PurchasePrice = productDTO.PurchasePrice,
            };
            _unitOfWork.ProductRepository.Add(product);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            _unitOfWork.ProductRepository.DeleteProduct(id);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> EditProduct(UpdateProductDTO productDTO)
        {
            var findProduct = _unitOfWork.ProductRepository.GetProductById(productDTO.Id);
            if (findProduct != null)
            {
                findProduct.Name = productDTO.Name;
                findProduct.CategoryId = productDTO.CategoryID;
                findProduct.Quantity = productDTO.Quantity;
                findProduct.Describe = productDTO.Describe;
                if(productDTO.Image != null)
                findProduct.Image = productDTO.Image;
                if(productDTO.ListImages != null)
                findProduct.ListImages = productDTO.ListImages;
                findProduct.Price = productDTO.Price;
                findProduct.Discount = productDTO.Discount;
                findProduct.UpdatedDate = DateTime.Now;
                findProduct.UpdatedBy = productDTO.UpdatedBy;
                findProduct.Status = productDTO.Status;
                findProduct.PurchasePrice = productDTO.PurchasePrice;
            }
            var x = findProduct;
                _unitOfWork.ProductRepository.Update(findProduct);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public async Task<List<ProductDTO>> GetAllActiveProductsAsync(
             string? search,
             string? category,
             double? minPrice,
             double? maxPrice,                         
             SortBy sortBy,
             bool isDescending,
             int page,
             int pageSize = 9)
        {
            var list = (await _unitOfWork.ProductRepository.GetAllProductsAsync(search,category,minPrice,maxPrice,page,pageSize,sortBy,isDescending)).Where(x => x.Status).ToList();
            var result = convertListProductToListProductViewModel(list);
            return result;
        }

        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            var list = await _unitOfWork.ProductRepository.GetQuery().ToListAsync();
            var result = convertListProductToListProductViewModel(list);
            return result;
        }

        public async Task<List<ProductDTO>> GetAllProductsInAdminAsync(string? search, int? categoryId,bool? status, SortBy sortBy, bool isDescending)
        {
            var list = (await _unitOfWork.ProductRepository.GetAllProductsInAdminAsync(search, categoryId,status, sortBy, isDescending)).ToList();
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
            if (product == null || !product.Status ) 
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
                Quantity = product.Quantity,
                SumPrice = (product.Price - (product.Discount * product.Price/100)) * items.Single(x => x.ProductId == product.Id).Count
            }).ToList();
            return result;
        }

        public async Task<UpdateProductDTO?> GetUpdateProductDTO(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return null;
            }
            var productDTO = new UpdateProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Quantity = product.Quantity,
                Image = product.Image,
                Discount = (int)product.Discount,
                Describe = product.Describe,
                Price = product.Price,
                PurchasePrice = product.PurchasePrice,
                CategoryID = product.CategoryId,
                Status = product.Status,
                ListImages = String.Join(",", JsonConvert.DeserializeObject<List<string>>(product.ListImages))
            };
            return productDTO;
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
                Price = x.Price,
                Quantity = x.Quantity,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name,
                Status = x.Status
            }).ToList();
            return result;
        }
        private string convertToVND(double s)
        {
            return s.ToString("C0", new System.Globalization.CultureInfo("vi-VN"));
        }
    }
}
