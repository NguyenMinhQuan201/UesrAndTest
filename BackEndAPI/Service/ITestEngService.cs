using BackEndAPI.Models.TestEngService;
using Data.Entities;
using Data.MMM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndAPI.Service
{
    public interface ITestEngService
    {
        Task<ApiResult<bool>> Create(CreateTestRequest request);
        Task<ApiResult<bool>> Delete(DeleteTestRequest request);
        Task<ApiResult<bool>> Update(UpdateTestRequest request);
        Task<ApiResult<TestEng>> GetById(int id);
        Task<ApiResult<IEnumerable<TestEng>>> GetList();

    }
}
