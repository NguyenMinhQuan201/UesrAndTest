using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Data.MMM
{
    public class UserUpdateRequest
    {
        public Guid Id { get; set; }
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
    }
}
