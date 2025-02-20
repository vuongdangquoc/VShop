using VShop.DAL.Models.Db;

namespace VShop.DAL.RepositoryContracts
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public Task<User?> GetUserByEmailAsync(string email);
        public Task<bool> CheckEmailExistAsync(string email);
        public Task<bool> CheckPhoneExistAsync(string phone);
    }
}
