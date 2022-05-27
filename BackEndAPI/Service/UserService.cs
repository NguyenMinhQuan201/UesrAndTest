using BackEndAPI.Models.UserService;
using BackEndAPI.Repositories;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
namespace BackEndAPI.Service
{
    public class UserService : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IConfiguration _config;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository,IConfiguration config)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _config = config;
        }

        public async Task<LoginResult> Login(LoginParams loginParams)
        {
            var user = await _userRepository.GetUserByUserNameAsync(loginParams.UserName);
            if (user == null)
            {
                return new LoginResult
                {
                    Succeeded = false,
                    Message = "User không tồn tại"
                };
            };

            var succeeded = await _userRepository.PasswordSignInAsync(new Repositories.Models.PasswordSignInParams
            {
                User = user,
                Password = loginParams.Password,
                RememberMe = loginParams.RememberMe
            });

            if (!succeeded)
            {
                return new LoginResult
                {
                    Succeeded = false,
                    Message = "Đăng nhập không thành công"
                };
            }

            var roles = await _roleRepository.GetRolesByUserAsync(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.Role, string.Join(";",roles)),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"], _config["Tokens:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds);

            return new LoginResult
            {
                Succeeded = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }
    }
}
