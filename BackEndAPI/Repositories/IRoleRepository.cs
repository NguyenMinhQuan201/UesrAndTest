using BackEndAPI.Models.UserService;
using BackEndAPI.Repositories.Models;
using Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEndAPI.Repositories
{
    public interface IRoleRepository
    {
        Task<IEnumerable<string>> GetRolesByUserAsync(AppUser user);
    }
}
