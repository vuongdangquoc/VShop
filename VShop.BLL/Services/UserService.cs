using Microsoft.AspNetCore.Identity;
using VShop.BLL.DTO;
using VShop.BLL.ServiceContracts;
using VShop.DAL.Models.Db;
using VShop.DAL.RepositoryContracts;
using VShop.DAL.UnitOfWork;

namespace VShop.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }
        public async Task<bool> Login(LoginDTO loginDTO)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(loginDTO.Email);
            if(user!= null)
            {
                var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDTO.Password);
                return result == PasswordVerificationResult.Success;
            }
            return false;
        }

        public async Task<bool> Register(RegisterDTO registerDTO)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = registerDTO.FullName,
                UserName = registerDTO.Email,
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.Phone
            };
            var passwordHash = _passwordHasher.HashPassword(user,registerDTO.Password);
            user.PasswordHash = passwordHash;

            _unitOfWork.UserRepository.Add(user);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }
    }
}
