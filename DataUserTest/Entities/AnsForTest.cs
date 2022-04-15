using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataUserTest.Entities
{
    public class AnsForTest
    {
        public int Id { get; set; }
        public int IdTest { get; set; }
        public string Ans { get; set; }
        public bool status { get; set; }
        public TestEng TestEng { get; set; }
    }
}
