using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace Word4Everyone.Model
{
    public class AppIdentityErrorDescriber : IdentityErrorDescriber
    {
        private readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public AppIdentityErrorDescriber(IStringLocalizer<SharedResource> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = _stringLocalizer["DuplicateUserName"].Value
            };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = _stringLocalizer["DuplicateEmail"].Value
            };
        }
    }
}
