using BackEndAPI.Models.TestEngService;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndAPI.Repositories
{
    public interface ITestEngRepository
    {
        Task<TestEng> Create(TestEng request);
        Task<bool> Delete(TestEng request);
        Task<TestEng> Update(TestEng request);
        Task<TestEng> GetById(int id);
        Task<IEnumerable<TestEng>> GetListTest();
    }
}
