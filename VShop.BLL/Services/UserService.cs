using Microsoft.AspNetCore.Identity;
using VShop.BLL.DTO;
using VShop.BLL.Helper;
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

        public async Task<bool> CheckEmailExistAsync(string email)
        {
           return await _unitOfWork.UserRepository.CheckEmailExistAsync(email);
        }

        public async Task<bool> CheckPhoneExistAsync(string phone)
        {
           return await _unitOfWork.UserRepository.CheckPhoneExistAsync(phone);
        }

        public async Task<UserDTO?> Login(LoginDTO loginDTO)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(loginDTO.Email);
            if(user!= null)
            {
                var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDTO.Password);
                if (result == PasswordVerificationResult.Success)
                {
                    UserDTO userDTO = ConvertUserToUserDTO(user);
                    return userDTO;
                }
            }
            return null;
        }

        public async Task<bool> Register(RegisterDTO registerDTO)
        {
            var role = await _unitOfWork.RoleRepository.GetRoleByNameAsync("Khách hàng");
            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = registerDTO.FullName,
                UserName = registerDTO.Email,
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.Phone,
                Status = 1,
                Image = "UploadFiles/Avatars/defaultAvatar.jpg",
                RoleId = role.Id
            };
            var passwordHash = _passwordHasher.HashPassword(user,registerDTO.Password);
            user.PasswordHash = passwordHash;

            _unitOfWork.UserRepository.Add(user);
            var result = await _unitOfWork.SaveChangesAsync();
            return result > 0;
        }

        public static UserDTO ConvertUserToUserDTO(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                UserName = user.UserName,
                Image = user.Image,
                Role = user.Role.Name,
                Status = UserHelper.ConvertToStatusString((int)user.Status),
            };
        }
    }
}
