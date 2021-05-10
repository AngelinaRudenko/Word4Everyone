using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using Word4Everyone.Model;
using Word4Everyone.Services.Interfaces;

namespace Word4Everyone.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UserService(UserManager<IdentityUser> userManager,
            IConfiguration configuration, IMailService mailService,
            IStringLocalizer<SharedResource> sharedLocalizer,
            IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mailService = mailService;
            _sharedLocalizer = sharedLocalizer;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model)
        {
            // TODO
            // if (model == null)
            //    throw new NullReferenceException(_sharedLocalizer["EmptyRegisterModel"].Value);

            IdentityUser user = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                string confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                byte[] encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
                string validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);
                string url = $"{_configuration["ThisAppUrl"]}/api/Auth/ConfirmEmail?userid={user.Id}&token={validEmailToken}";

                string path = Path.Combine(_hostEnvironment.WebRootPath, "MailTemplates", "ConfirmEmail.html");
                string text = File.ReadAllText(path);
                text = text.Replace("{link}", url);

                await _mailService.SendEmailAsync(user.Email, 
                    _sharedLocalizer["EmailConfirmationName"].Value, text);

                return new UserManagerResponse
                {
                    IsSuccess = true,
                    Message = _sharedLocalizer["RegistrationSuccess"].Value
                };
            }

            return new UserManagerResponse
            {
                IsSuccess = false,
                Message = _sharedLocalizer["RegistrationFail"].Value,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task<UserManagerResponse> LoginUserAsync(LoginViewModel model)
        {
            //TODO
            // if (model == null)
            //    throw new NullReferenceException(_sharedLocalizer["EmptyLoginModel"].Value);

            IdentityUser user = await _userManager.FindByNameAsync(model.Email);

            if (user == null)
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = _sharedLocalizer["WrongEmail"].Value
                };

            bool result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = _sharedLocalizer["WrongPassword"].Value
                };

            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("Email", model.Email)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["AuthSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponse
            {
                IsSuccess = true,
                Message = tokenAsString,
                ExpireDate = token.ValidTo
            };
        }

        public async Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token)
        {
            IdentityUser user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = _sharedLocalizer["UserNotFound"].Value
                };

            byte[] decodedToken = WebEncoders.Base64UrlDecode(token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);
            IdentityResult result = await _userManager.ConfirmEmailAsync(user, normalToken);

            if (result.Succeeded)
                return new UserManagerResponse
                {
                    IsSuccess = true,
                    Message = _sharedLocalizer["EmailConfirmed"].Value
                };

            return new UserManagerResponse
            {
                IsSuccess = false,
                Message = _sharedLocalizer["EmailNotConfirmed"].Value,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task<UserManagerResponse> ForgetPasswordAsync(string email)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = _sharedLocalizer["UserNotFound"].Value
                };

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            byte[] encodedToken = Encoding.UTF8.GetBytes(token);
            string validToken = WebEncoders.Base64UrlEncode(encodedToken);

            string url = $"{_configuration["ThisAppUrl"]}/api/Auth/ResetPassword?email={email}&token={validToken}";

            string path = Path.Combine(_hostEnvironment.WebRootPath, "MailTemplates", "ConfirmEmail.html");
            string text = File.ReadAllText(path);
            text = text.Replace("{link}", url);

            await _mailService.SendEmailAsync(user.Email,
                _sharedLocalizer["ResetPasswordEmailName"].Value, text);

            return new UserManagerResponse
            {
                IsSuccess = true,
                Message = _sharedLocalizer["ResetUrlSent"].Value
            };
        }

        public async Task<UserManagerResponse> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            // TODO
            // if (model == null)
            //    throw new NullReferenceException(_sharedLocalizer["EmptyResetModel"].Value);

            IdentityUser user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = _sharedLocalizer["UserNotFound"].Value
                };

            byte[] decodedToken = WebEncoders.Base64UrlDecode(model.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, normalToken, model.Password);

            if (result.Succeeded)
                return new UserManagerResponse
                {
                    IsSuccess = true,
                    Message = _sharedLocalizer["PasswordChanged"].Value
                };

            return new UserManagerResponse
            {
                IsSuccess = false,
                Message = _sharedLocalizer["PasswordNotChanged"].Value,
                Errors = result.Errors.Select(x => x.Description)
            };
        }
    }
}
