using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.MMM
{
    public class classa
    {
        public string stringA { get; set; }
    }
    public class classb
    {
        public string stringA { get; set; }
        public string SString()
        {
            return stringA + "haha";
        }
    }
}
