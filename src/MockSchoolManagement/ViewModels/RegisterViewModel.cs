﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MockSchoolManagement.CustomerMiddlewares.Utils;

namespace MockSchoolManagement.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "邮箱")]
        [ValidEmailDomain(allowedDomain: "qq.com", ErrorMessage = "只能使用QQ邮箱")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { set; get; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码与确认密码不一致, 请重新输入")]
        public string ConfirmPassword { set; get; }

        [Required]
        [Display(Name = "所在城市")]
        public string City { set; get; }
    }
}
