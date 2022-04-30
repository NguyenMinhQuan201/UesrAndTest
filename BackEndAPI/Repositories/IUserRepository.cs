using BackEndAPI.Models.UserService;
using BackEndAPI.Repositories.Models;
using Data.Entities;
using System.Threading.Tasks;

namespace BackEndAPI.Repositories
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserByUserNameAsync(string userName);

        Task<bool> PasswordSignInAsync(PasswordSignInParams passwordSignInParams);
    }
}
