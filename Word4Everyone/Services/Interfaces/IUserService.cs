using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Word4Everyone.Shared;

namespace Word4Everyone.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);
        Task<UserManagerResponse> LoginUserAsync(LoginViewModel model);
        Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token);
    }
}
