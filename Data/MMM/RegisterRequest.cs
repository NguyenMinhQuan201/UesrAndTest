using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Data.MMM
{
    public class RegisterRequest
    {
        [Display(Name = "Ten")]
        public string FirstName { get; set; }
        [Display(Name = "Ho")]
        public string LastName { get; set; }
        [Display(Name = "Ngay sinh")]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }
        [Display(Name = "SDT")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Tai Khoan")]
        public string UserName { get; set; }
        [Display(Name = "Mat Khau")]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }

        [Display(Name = "Xac Nhan Mat Khau")]
        [DataType(DataType.Password)]
        public string ConFirmPassWord { get; set; }
    }
}
