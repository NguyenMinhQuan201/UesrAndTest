using BackEndAPI.Models.TestEngService;
using BackEndAPI.Service;
using Data.EF;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndAPI.Repositories
{
    public class TestEngRepository : ITestEngRepository
    {
        private readonly UserAndTestDbContext _context;
        public TestEngRepository(UserAndTestDbContext context)
        {
            _context = context;
        }
        public async Task<TestEng> Create(TestEng request)
        {
            _context.TestEngs.Add(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<bool> Delete(TestEng request)
        {
            _context.TestEngs.Remove(request);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TestEng> GetById(int id)
        {
            var find = await _context.TestEngs.FindAsync(id);
            return find;
        }

        public async Task<IEnumerable<TestEng>> GetListTest()
        {
            return await _context.TestEngs.ToListAsync();
        }

        public async Task<TestEng> Update(TestEng request)
        {
            _context.TestEngs.Update(request);
            await _context.SaveChangesAsync();
            return request;
        }
    }
}
