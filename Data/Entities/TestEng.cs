using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class TestEng
    {
        public int Id { get; set; }
        public string Title { get; set; }
        
        public List<AnsForTest> AnsForTests { get; set; }
    }
}
