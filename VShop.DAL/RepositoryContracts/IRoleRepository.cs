using VShop.DAL.Models.Db;

namespace VShop.DAL.RepositoryContracts
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        public Task<Role?> GetRoleByNameAsync(string roleName);
    }
}
