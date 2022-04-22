using Data.Entities;
using Data.MMM;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndAPI.Service
{
    public interface IServiceRole
    {
        Task<ApiResult<bool>> Register(RoleRequest request);
        Task<ApiResult<bool>> Remove(RemoveRoleRequest request);
        Task<List<RoleVm>> GetAllRole();
    }
    public class ServiceRole : IServiceRole
    {
        private readonly RoleManager<AppRole> _roleManager;
        public ServiceRole(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
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
        public async Task<ApiResult<bool>> Register(RoleRequest request)
        {
            var name = _roleManager.FindByNameAsync(request.Name);
            if (name.Result != null)
            {
                return new ApiErrorResult<bool>("Role đã tồn tại");
            }
            var user = new AppRole()
            {
                Description= request.Name,
                Name=request.Name,
                NormalizedName=request.Name
            };
            var end = await _roleManager.CreateAsync(user);
            if (end.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }

            return new ApiErrorResult<bool>("Tạo không thành công");
        }

        public async Task<ApiResult<bool>> Remove(RemoveRoleRequest request)
        {
            var val = await _roleManager.FindByIdAsync(request.Id.ToString());
            if(val == null)
            {
                return new ApiErrorResult<bool>("Xoas không thành công");
            }
            var end = await _roleManager.DeleteAsync(val);
            if (end.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }

            return new ApiErrorResult<bool>("Xoas không thành công");
        }
    }
}
