using Microsoft.AspNetCore.Identity;
using WebApplication.Resources;

namespace WebApplication.Helpers
{
    public class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = string.Format(LocalizedIdentityErrorMessages.DuplicateUserName, userName)
            };
        }

        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description = string.Format(LocalizedIdentityErrorMessages.InvalidUserName, userName)
            };
        }
    }
}
