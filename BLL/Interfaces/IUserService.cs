using DTO;
using System;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserService
    {
        Task<string> RegisterUserAsync(UserRegisterModel model);
        Task<string> LoginAsync(LoginCredentials credentials);
        Task LogoutAsync();
        Task<UserDto> GetUserProfileAsync(Guid userId);
        Task EditUserProfileAsync(Guid userId, UserEditModel model);
    }
}
