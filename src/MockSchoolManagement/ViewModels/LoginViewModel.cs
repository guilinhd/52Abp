using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;

namespace MockSchoolManagement.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "邮箱地址")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "邮箱的格式不正确")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { set; get; }

        public string ReturnUrl { set; get; }

        public IList<AuthenticationScheme> ExternalLogins { set; get; }
    }
}
