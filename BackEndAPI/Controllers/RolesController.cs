using BackEndAPI.Service;
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
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IServiceAPIUser _serviceAPIUser;

        public RolesController(IServiceAPIUser serviceAPIUser)
        {
            _serviceAPIUser = serviceAPIUser;
        }
        [HttpGet("GetRoles")]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _serviceAPIUser.GetAllRole();
            return Ok(roles);
        }
    }
}
