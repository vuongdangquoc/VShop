using VShop.Models.Db;
using VShop.RepositoryContracts;

namespace VShop.UnitOfWork
{
    public interface IUnitOfWork
    {
        VShopContext Context { get; }
        ICategoryRepository CategoryRepository { get; }
        ICommentRepository CommentRepository { get; }
        IContactRepository ContactRepository { get; }
        IDistrictRepository InvoiceRepository { get; }
        IFavoriteProductRepository FavoriteProductRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }
        IPostRepository PostRepository { get; }
        IProductRepository ProductRepository { get; }
        IProvinceRepository ProvinceRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserRepository UserRepository { get; }
        IVoucherRepository VoucherRepository { get; }
        IWardRepository WardRepository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
