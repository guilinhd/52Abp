using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MockSchoolManagement.ViewModels
{
    public class UserUpdateViewModel 
    {

        public UserUpdateViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();

        }
        public string Id { set; get; }

        [Required(ErrorMessage = "请输入用户名")]
        [Display(Name = "用户名")]
        public string Name { set; get; }

        [Required(ErrorMessage = "请输入邮箱地址")]
        [EmailAddress]
        [Display(Name = "邮箱")]
        public string Email { get; set; }

        public string City { set; get; }

        public List<string> Claims { set; get; } 

        public List<string> Roles { set; get; } 
    }
}
