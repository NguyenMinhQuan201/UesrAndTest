using BackEndAPI.Repositories.Models;
using Data.EF;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserAndTestDbContext _context;
        private readonly SignInManager<AppUser> _signInManage;

        public UserRepository(UserAndTestDbContext context, SignInManager<AppUser> signInManage)
        {
            _context = context;
            _signInManage = signInManage;
        }

        public async Task<AppUser> GetUserByUserNameAsync(string userName)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserName == userName);

            return await Task.FromResult(user);
        }

        public async Task<bool> PasswordSignInAsync(PasswordSignInParams passwordSignInParams)
        {
            var result = await _signInManage.PasswordSignInAsync(passwordSignInParams.User, passwordSignInParams.Password, passwordSignInParams.RememberMe, passwordSignInParams.RememberMe);

            return result.Succeeded;
        }
    }
}
