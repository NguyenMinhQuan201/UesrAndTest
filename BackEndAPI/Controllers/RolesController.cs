using BackEndAPI.Service;
using Data.MMM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IServiceAPIUser _serviceAPIUser;
        private readonly IServiceRole _ServiceRole;

        public RolesController(IServiceAPIUser serviceAPIUser, IServiceRole ServiceRole)
        {
            _serviceAPIUser = serviceAPIUser;
            _ServiceRole = ServiceRole;
        }
        [HttpGet("GetRoles")]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _ServiceRole.GetAllRole();
            return Ok(roles);
        }
        [HttpPost]
        public async Task<IActionResult> CreaterRole(RoleRequest request)
        {
            var result = await _ServiceRole.Register(request);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> Remove(RemoveRoleRequest request)
        {
            var result = await _ServiceRole.Remove(request);
            return Ok();
        }
    }
}
