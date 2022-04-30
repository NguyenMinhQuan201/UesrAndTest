using Data.Entities;

namespace BackEndAPI.Repositories.Models
{
    public class PasswordSignInParams
    {
        public AppUser User { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
