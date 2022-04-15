using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.MMM
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public bool RememberMe { get; set; }
    }
}
