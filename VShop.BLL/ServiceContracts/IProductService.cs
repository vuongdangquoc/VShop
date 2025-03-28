﻿using System.Globalization;
using VShop.DAL.Enums;
using VShop.BLL.DTO;
using VShop.DAL.UnitOfWork;

namespace VShop.BLL.ServiceContracts
{
    public interface IProductService
    {
        public Task<List<ProductDTO>?> GetBestSellingProductsAsync(string category = "all");

        public Task<List<ProductDTO>?> GetNewArrivalsAsync(string category = "all");

        Task<List<ProductDTO>> GetAllActiveProductsAsync(
             string? search,
             string? category,
             double? minPrice,
             double? maxPrice,
             SortBy sortBy,
             bool isDescending,
             int page,
             int pageSize);

        Task<List<ProductDTO>> GetAllProductsInAdminAsync(
             string? search,
             int? categoryId,
             bool? status,
             SortBy sortBy,
             bool isDescending);

        public Task<bool> CheckProductExist(int id);

        public Task<ProductDTO?> GetProductById(int id);
        public Task<UpdateProductDTO?> GetUpdateProductDTO(int id);
        public Task<List<ProductCartDTO>> GetProductsInCart(List<CartCookieDTO> items);

        public Task<bool> CreateProduct(CreateProductDTO createProductDTO);
        public Task<bool> EditProduct(UpdateProductDTO updateProductDTO);
        public Task<bool> DeleteProduct(int id);
        public Task<bool> ChangeStatusProduct(int id, string updateBy);

    }
}
