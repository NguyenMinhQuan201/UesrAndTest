using BackEndAPI.Models.TestEngService;
using BackEndAPI.Repositories;
using Data.EF;
using Data.Entities;
using Data.MMM;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndAPI.Service
{
    public class TestEngService : ITestEngService
    {

        private readonly UserAndTestDbContext _context;
        private readonly ITestEngRepository _testEngRepository;
        public TestEngService(ITestEngRepository testEngRepository)
        {
            _testEngRepository = testEngRepository;
        }
        public async Task<ApiResult<bool>> Create(CreateTestRequest request)
        {
            var query = from t in _context.TestEngs select new { t };
            var lst = await query.Select(x => new CreateTestRequest()
            {
                Title = x.t.Title
            }).ToListAsync();
            foreach (var a in lst)
            {
                if (a.Title == request.Title)
                {
                    return new ApiErrorResult<bool>("Title đã tồn tại");
                }
            }
            var Test = new TestEng()
            {
                Title = request.Title
            };
            await _testEngRepository.Create(Test);
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> Delete(DeleteTestRequest request)
        {
            var test = await _context.TestEngs.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
            var lst = await _context.TestEngs.ToListAsync();
            if (test == null)
            {
                return new ApiErrorResult<bool>("Ko tồn tại");
            }
            else
            {
                await _testEngRepository.Delete(test);
            }
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<TestEng>> GetById(int id)
        {
            var result = await _testEngRepository.GetById(id);
            if (result == null)
            {
                return new ApiErrorResult<TestEng>("Ko tồn tại");
            }
            return new ApiSuccessResult<TestEng>(result);
        }

        public Task<ApiResult<IEnumerable<TestEng>>> GetList()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> Update(UpdateTestRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
