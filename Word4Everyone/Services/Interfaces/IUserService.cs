using System.Threading.Tasks;
using Word4Everyone.Model;

namespace Word4Everyone.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);
        Task<UserManagerResponse> LoginUserAsync(LoginViewModel model);
        //Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token);
    }
}
