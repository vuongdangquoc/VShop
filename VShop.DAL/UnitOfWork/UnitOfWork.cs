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
        private readonly IDistrictRepository _districtRepository;
        private readonly IFavoriteProductRepository _favouriteProductRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IPostRepository _postRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProvinceRepository _provinceRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVoucherRepository _voucherRepository;
        private readonly IWardRepository _wardRepository;
        public UnitOfWork(VShopContext db,ICategoryRepository categoryRepository, ICommentRepository commentRepository, IContactRepository contactRepository, IDistrictRepository districtRepository, IFavoriteProductRepository favoriteProductRepository, IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IPostRepository postRepository, IProductRepository productRepository, IProvinceRepository provinceRepository, IRoleRepository roleRepository, IUserRepository userRepository, IVoucherRepository voucherRepository, IWardRepository wardRepository)
        {
            _db = db;
            _categoryRepository = categoryRepository;
            _commentRepository = commentRepository;
            _contactRepository = contactRepository;
            _districtRepository = districtRepository;
            _favouriteProductRepository = favoriteProductRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _postRepository = postRepository;
            _productRepository = productRepository;
            _provinceRepository = provinceRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _voucherRepository = voucherRepository;
            _wardRepository = wardRepository;
        }
        public VShopContext Context => _db;

        public ICategoryRepository CategoryRepository => _categoryRepository;

        public ICommentRepository CommentRepository => _commentRepository;

        public IContactRepository ContactRepository => _contactRepository;

        public IDistrictRepository DistrictRepository => _districtRepository;

        public IFavoriteProductRepository FavoriteProductRepository => _favouriteProductRepository;

        public IOrderRepository OrderRepository => _orderRepository;

        public IOrderDetailRepository OrderDetailRepository => _orderDetailRepository;

        public IPostRepository PostRepository => _postRepository;

        public IProductRepository ProductRepository => _productRepository;

        public IProvinceRepository ProvinceRepository => _provinceRepository;

        public IRoleRepository RoleRepository => _roleRepository;

        public IUserRepository UserRepository => _userRepository;

        public IVoucherRepository VoucherRepository => _voucherRepository;

        public IWardRepository WardRepository => _wardRepository;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
