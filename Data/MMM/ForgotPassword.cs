using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.MMM
{
    public class ForgotPassword
    {
        public string NewPassWord { get; set; }
        public string ConfirmPassWord { get; set; }
        public string Token { get; set; }

    }
}
