using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using VShop.DAL.Models.Db;
using VShop.DAL.RepositoryContracts;

namespace VShop.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VShopContext _db;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IFavoriteProductRepository _favouriteProductRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IPostRepository _postRepository;
        private readonly IProductRepository _productRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVoucherRepository _voucherRepository;
        public UnitOfWork(VShopContext db,ICategoryRepository categoryRepository, ICommentRepository commentRepository, IContactRepository contactRepository, IFavoriteProductRepository favoriteProductRepository, IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IPostRepository postRepository, IProductRepository productRepository, IRoleRepository roleRepository, IUserRepository userRepository, IVoucherRepository voucherRepository)
        {
            _db = db;
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
            _contactRepository = contactRepository;
            _favouriteProductRepository = favoriteProductRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _postRepository = postRepository;
            _productRepository = productRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _voucherRepository = voucherRepository;
        }
        public VShopContext Context => _db;

        public ICategoryRepository CategoryRepository => _categoryRepository;

        public ICommentRepository CommentRepository => _commentRepository;

        public IContactRepository ContactRepository => _contactRepository;
        public IFavoriteProductRepository FavoriteProductRepository => _favouriteProductRepository;

        public IOrderRepository OrderRepository => _orderRepository;

        public IOrderDetailRepository OrderDetailRepository => _orderDetailRepository;

        public IPostRepository PostRepository => _postRepository;

        public IProductRepository ProductRepository => _productRepository;
        public IRoleRepository RoleRepository => _roleRepository;

        public IUserRepository UserRepository => _userRepository;

        public IVoucherRepository VoucherRepository => _voucherRepository;

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _db.Database.BeginTransactionAsync();
        }
        public async Task CommitAsync()
        {
            await _db.Database.CommitTransactionAsync();
        }
        public async Task RollbackAsync()
        {
            await _db.Database.RollbackTransactionAsync();
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
