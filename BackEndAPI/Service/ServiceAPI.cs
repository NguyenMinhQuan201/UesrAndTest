using BackEndAPI.Models;
using Data.EF;
using Data.Entities;
using Data.MMM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BackEndAPI.Service
{
    public interface IServiceAPIUser
    {
        Task<ApiResult<string>> Authencate(LoginRequest request);

        Task<ApiResult<bool>> Register(RegisterRequest request);
        Task<ApiResult<bool>> Update(Guid Id, UserUpdateRequest request);

        Task<ApiResult<GetList<UserVm>>> GetUsersPaging();
        Task<ApiResult<UserVm>> GetById(Guid Id);
        Task<ApiResult<bool>> Delete(Guid Id);
        Task<List<RoleVm>> GetAllRole();
        Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request);
        Task<ApiResult<string>> TokenForgotPass(InputModel Input);
        Task<ApiResult<bool>> GetResetPasswordConfirm(string email, string token);
    }
    public class ServiceAPIUser : IServiceAPIUser
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly UserAndTestDbContext _context;

        public ServiceAPIUser(UserAndTestDbContext context, UserManager<AppUser> userManage, SignInManager<AppUser> signInManage, RoleManager<AppRole> roleManager, IConfiguration config)
        {
            _userManager = userManage;
            _signInManager = signInManage;
            _roleManager = roleManager;
            _config = config;
            _context = context;
        }

        public async Task<ApiResult<string>> Authencate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return new ApiErrorResult<string>("Tai khoan ko ton tai");

            var result = await _signInManager.PasswordSignInAsync(user, request.PassWord, request.RememberMe, request.RememberMe);
            if (!result.Succeeded)
            {
                return new ApiErrorResult<string>("Đăng nhập không đúng");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.Role, string.Join(";",roles)),
                new Claim(ClaimTypes.Name, request.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new ApiSuccessResult<string>(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task<ApiResult<bool>> Delete(Guid Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("User ko ton tai");
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("xoa ko thanh cong");
        }

        public async Task<ApiResult<UserVm>> GetById(Guid Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<UserVm>("User ko ton tai");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var Uservm = new UserVm()
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                Id = Id,
                LastName = user.LastName,
                UserName = user.UserName,
                Roles=roles
            };
            return new ApiSuccessResult<UserVm>(Uservm);
        }

        public async Task<ApiResult<GetList<UserVm>>> GetUsersPaging()
        {
            var data = await _userManager.Users.Select(x=> new UserVm() 
            {
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                UserName = x.UserName,
                FirstName = x.FirstName,
                Id = x.Id,
                LastName = x.LastName,
            }).ToListAsync();
            if (data==null)
            {
                return new ApiErrorResult<GetList<UserVm>>("KO");
            }
            var list = new GetList<UserVm>()
            {
                Items = data
            };
            return new ApiSuccessResult<GetList<UserVm>>(list);
        }


        public async Task<ApiResult<bool>> Register(RegisterRequest request)
        {
            var Email = _userManager.Users.FirstOrDefault(x => x.Email == request.Email);
            if (Email != null)
            {
                return new ApiErrorResult<bool>("Emai đã tồn tại");
            }
            var UserName = await _userManager.FindByNameAsync(request.UserName);
            if (UserName != null)
            {
                return new ApiErrorResult<bool>("Tài khoản đã tồn tại");
            }
            var user = new AppUser()
            {
                Dob = request.Dob,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, request.PassWord);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var end= await _userManager.ConfirmEmailAsync(user, token);
            if (end.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            
            return new ApiErrorResult<bool>("Đăng ký không thành công");
        }

        public async Task<ApiResult<bool>> Update(Guid Id, UserUpdateRequest request)
        {
            var Email = _userManager.Users.Any(x => x.Email == request.Email && x.Id != Id);
            if (Email)
            {
                return new ApiErrorResult<bool>("Emai đã tồn tại");
            }
            var user = await _userManager.FindByIdAsync(Id.ToString());

            user.Dob = request.Dob;
            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Cap Nhat không thành công");
        }
        public async Task<List<RoleVm>> GetAllRole()
        {
            var roles = await _roleManager.Roles
                .Select(x => new RoleVm()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToListAsync();

            return roles;
        }
        public async Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("Tài khoản không tồn tại");
            }
            var removedRoles = request.Roles.Where(x => x.Selected == false).Select(x => x.Name).ToList();
            foreach (var roleName in removedRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == true)
                {
                    await _userManager.RemoveFromRoleAsync(user, roleName);
                }
            }
            await _userManager.RemoveFromRolesAsync(user, removedRoles);

            var addedRoles = request.Roles.Where(x => x.Selected).Select(x => x.Name).ToList();
            foreach (var roleName in addedRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == false)
                {
                    await _userManager.AddToRoleAsync(user, roleName);
                }
            }

            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<string>> TokenForgotPass(InputModel Input)
        {
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                return new ApiErrorResult<string>("ko thanh cong");
            }

            // Phát sinh Token để reset password
            // Token sẽ được kèm vào link trong email,
            // link dẫn đến trang /Account/ResetPassword để kiểm tra và đặt lại mật khẩu
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = System.Web.HttpUtility.UrlEncode(token);
            /*token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));*/
            return new ApiSuccessResult<string>(token);
        }
        public async Task<ApiResult<bool>> GetResetPasswordConfirm(string email, string token)
        {
            if (email == null || token==null)
            {
                return new ApiErrorResult<bool>(false.ToString());
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ApiErrorResult<bool>(false.ToString());
            }
            var password = "Quan2001@";
            string code = token;
            var result = await _userManager.ResetPasswordAsync(user, code, password);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>(true);
            }
            return new ApiErrorResult<bool>(false.ToString());
        }
    }
}
