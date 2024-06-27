using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using webapi.Data;
using webapi.Models;

namespace webapi.Services
{
    public class ApplicationUserManager : UserManager<User>
    {
        private readonly UserStore<User, IdentityRole, webapiContext, string, IdentityUserClaim<string>,
            IdentityUserRole<string>, IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>> _store;
        public ApplicationUserManager(
            IUserStore<User> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<User>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            //_store = (UserStore<User, IdentityRole, webapiContext, string, IdentityUserClaim<string>,
            //    IdentityUserRole<string>, IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>>)store;
        }

        public async Task<User?> FindByDisplayIdAsync(int displayId)
        {
            ThrowIfDisposed();
            

            var user = from u in Users
                       where u.DisplayId == displayId
                       select u;
            return user.FirstOrDefault();
        }

        public async Task<User?> FindByEmail(string email)
        {
            ThrowIfDisposed();


            var user = from u in Users
                       where u.Email == email
                       select u;
            return user.FirstOrDefault();
        }

    }
}
