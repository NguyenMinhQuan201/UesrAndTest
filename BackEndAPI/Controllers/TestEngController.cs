using BackEndAPI.Models.TestEngService;
using BackEndAPI.Service;
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
    public class TestEngController : ControllerBase
    {
        private readonly ITestEngService _testEngService;
        public TestEngController(ITestEngService testEngService)
        {
            _testEngService = testEngService;
        }
        [HttpPost("CreateTest")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateTestRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _testEngService.Create(request);
            if (result.IsSuccessed)
            {
                return Ok();
            }
            return BadRequest(result);
        }
        [HttpDelete("DeleteTest")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete([FromBody] DeleteTestRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _testEngService.Delete(request);
            if (result.IsSuccessed)
            {
                return Ok();
            }
            return BadRequest(result);
        }
        [HttpGet("GetTestList")]
        [AllowAnonymous]
        public async Task<IActionResult> GetList()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _testEngService.GetList();
            if (result.IsSuccessed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
