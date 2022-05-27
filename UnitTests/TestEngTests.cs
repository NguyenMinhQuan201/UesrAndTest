using Abp.Domain.Uow;
using BackEndAPI.Repositories;
using BackEndAPI.Service;
using Data.Entities;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitTests.CLASSTEST;
using Xunit;

namespace UnitTests
{
    public class TestEngTests
    {
        private readonly TestEngService _testEngService;
        private readonly Mock<ITestEngRepository> _iestEngRepoMock = new Mock<ITestEngRepository>();
        public TestEngTests()
        {
            _testEngService = new TestEngService(_iestEngRepoMock.Object);
        }
        [Fact]
        public async Task Get_TestEng_Work()
        {
            // Chuan bi
            var TestEngId = 1;
            var Tile = " dau buoi";
            var testEngDto = new TestEng()
            {
                Id = TestEngId,
                Title = Tile,
            };
            _iestEngRepoMock.Setup(x => x.GetById(TestEngId)).ReturnsAsync(testEngDto);
            // thuc hien 
            var testEng = await _iestEngRepoMock.Object.GetById(1);
            //ket qua
            Assert.Equal(TestEngId, testEng.Id);
        }
    }
}
