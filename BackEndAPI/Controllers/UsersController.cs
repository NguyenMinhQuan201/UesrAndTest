using BackEndAPI.Models;
using BackEndAPI.Service;
using Data.MMM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UsersController : ControllerBase
    {
        private readonly IServiceAPIUser _serviceAPIUser;
        public UsersController(IServiceAPIUser serviceAPIUser)
        {
            _serviceAPIUser = serviceAPIUser;
        }
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultToken = await _serviceAPIUser.Authencate(request);

            if (string.IsNullOrEmpty(resultToken.ResultObj))
            {
                return BadRequest(resultToken);
            }

            return Ok(resultToken);
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _serviceAPIUser.Register(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        //https://localhost/api/user/id=
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _serviceAPIUser.Update(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        //https://localhost/api/user/paging?pageIndex=1&pageSize=10&keyword=
        [HttpGet("buoi")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _serviceAPIUser.GetUsersPaging();
            return Ok(products);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var products = await _serviceAPIUser.GetById(Id);
            return Ok(products);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var products = await _serviceAPIUser.Delete(Id);
            return Ok(products);
        }
        [HttpPut("{id}/roles")]
        public async Task<IActionResult> RoleAssign(Guid id, [FromBody] RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _serviceAPIUser.RoleAssign(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("GetTokenForgotPass")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTokenForgotPass(InputModel Input)
        {
            var kq = await _serviceAPIUser.TokenForgotPass(Input);
            return Ok(kq);
        }
        [AllowAnonymous]
        [HttpGet("ResetPasswordConfirm")]
        public async Task<IActionResult> ResetPasswordConfirm(string email,string token,string newpassword)
        {
            var kq = await _serviceAPIUser.GetResetPasswordConfirm(email, token,newpassword);
            return Ok(kq);
        }
    }
}
