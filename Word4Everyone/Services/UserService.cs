using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Word4Everyone.Model;
using Word4Everyone.Services.Interfaces;

namespace Word4Everyone.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        //private IMailService _mailService;

        public UserService(UserManager<IdentityUser> userManager, IConfiguration configuration, IMailService mailService)
        {
            _userManager = userManager;
            _configuration = configuration;
            //_mailService = mailService;
        }

        public async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model)
        {
            if (model == null)
                throw new NullReferenceException("Пустая модель регистрации");

            if (model.Password != model.PasswordConfirm)
                return new UserManagerResponse
                {
                    Message = "Подтверждение пароля не соответствует паролю",
                    IsSuccess = false
                };

            IdentityUser user = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                //var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
                //var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);
                //string url = $"{_configuration["ThisAppUrl"]}/api/Auth/ConfirmEmail?userid={user.Id}&token={validEmailToken}";
                //await _mailService.SendEmailAsync(user.Email, "Confirm your Email", "<h1>Welcome to Word4Everyone</h1>"+
                //    $"<p>Please confirm your email by <a href='{url}'>Clicking here</a></p>");

                return new UserManagerResponse
                {
                    Message = "Пользователь успешно зарегистрирован",
                    IsSuccess = true
                };
            }

            return new UserManagerResponse
            {
                Message = "Пользователь не был зарегистрирован",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task<UserManagerResponse> LoginUserAsync(LoginViewModel model)
        {
            IdentityUser user = await _userManager.FindByNameAsync(model.Email);

            if (user == null)
                return new UserManagerResponse
                {
                    Message = "Неверный Email",
                    IsSuccess = false
                };

            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
                return new UserManagerResponse
                {
                    Message = "Неверный пароль",
                    IsSuccess = false
                };

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("Email", model.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));


            var token = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["AuthSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponse
            {
                Message = tokenAsString,
                IsSuccess = true,
                ExpireDate = token.ValidTo
            };
        }

        //public async Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null)
        //        return new UserManagerResponse
        //        {
        //            Message = "User not found",
        //            IsSuccess = false
        //        };

        //    var decodedToken = WebEncoders.Base64UrlDecode(token);
        //    string normalToken = Encoding.UTF8.GetString(decodedToken);

        //    var result = await _userManager.ConfirmEmailAsync(user, normalToken);

        //    if (result.Succeeded)
        //        return new UserManagerResponse
        //        {
        //            Message = "Email confirmed successfully",
        //            IsSuccess = true
        //        };

        //    return new UserManagerResponse
        //    {
        //        Message = "Email did not confirm",
        //        IsSuccess = false,
        //        Errors = result.Errors.Select(e => e.Description)
        //    };
        //}
    }
}
