using VShop.DAL.Models.Db;

namespace VShop.DAL.RepositoryContracts
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        public Task<Order?> GetNewByUserID(Guid userId);
    }
}
