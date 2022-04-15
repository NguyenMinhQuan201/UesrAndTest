using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataUserTest.Entities
{
    public class TestEng
    {
        public int Id { get; set; }
        public int Title { get; set; }
        
        public List<AnsForTest> AnsForTests { get; set; }
    }
}
