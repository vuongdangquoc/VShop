using System.Globalization;
using VShop.DAL.Enums;
using VShop.BLL.DTO;
using VShop.DAL.UnitOfWork;

namespace VShop.BLL.ServiceContracts
{
    public interface IProductService
    {
        public Task<List<ProductDTO>?> GetBestSellingProductsAsync(string category = "all");

        public Task<List<ProductDTO>?> GetNewArrivalsAsync( string category = "all");

        Task<List<ProductDTO>> GetAllProductsAsync(
             string? search,
             string? category,
             double? minPrice,
             double? maxPrice,            
             SortBy sortBy,
             bool isDescending,
             int page,
             int pageSize);
        }
}
