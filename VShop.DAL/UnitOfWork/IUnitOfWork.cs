using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using VShop.DAL.Models.Db;
using VShop.DAL.RepositoryContracts;

namespace VShop.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        VShopContext Context { get; }
        ICategoryRepository CategoryRepository { get; }
        ICommentRepository CommentRepository { get; }
        IContactRepository ContactRepository { get; }
        IFavoriteProductRepository FavoriteProductRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }
        IPostRepository PostRepository { get; }
        IProductRepository ProductRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserRepository UserRepository { get; }
        IVoucherRepository VoucherRepository { get; }
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
