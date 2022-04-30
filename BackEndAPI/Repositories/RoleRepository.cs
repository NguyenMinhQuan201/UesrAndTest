using Data.EF;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEndAPI.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly UserManager<AppUser> _userManager;

        public RoleRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<string>> GetRolesByUserAsync(AppUser user)
        {
            var result = await _userManager.GetRolesAsync(user);

            return result;
        }
    }
}
