using MockSchoolManagement.Models.EnumTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MockSchoolManagement.ViewModels
{
    public class StudentCreateViewModel
    {
        public int Id { get; set; }

        [Display(Name = "姓名")]
        [Required(ErrorMessage = "请输入姓名")]
        public string Name { get; set; }

        [Display(Name = "主修科目")]
        [Required(ErrorMessage = "请选择一门科目")]
        public MajorEnum? Major { get; set; }

        [Display(Name = "电子邮件")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "邮箱的格式不正确")]
        public string Email { get; set; }

        [Display(Name = "头像图片")]
        public IFormFile Photos { set; get; }
    }
}
