using VShop.DAL.Models.Db;

namespace VShop.DAL.RepositoryContracts
{
    public interface IOrderDetailRepository : IGenericRepository<OrderDetail>
    {
        public Task<List<int>> GetListBestSellingProductIdAsync(int count, string category = "all");
        public Task<List<OrderDetail>?> GetOrderByUserIdAsync(Guid userId);
    }
}
