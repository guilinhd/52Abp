using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MockSchoolManagement.Models.EnumTypes;


namespace MockSchoolManagement.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Display(Name = "姓名")]
        [Required(ErrorMessage ="请输入姓名")]
        public string Name { get; set; }

        [Display(Name = "主修科目")]
        [Required(ErrorMessage = "请选择一门科目")]
        public MajorEnum? Major { get; set; }

        [Display(Name = "电子邮件")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "邮箱的格式不正确")]
        public string Email { get; set; }

        public string PhotoPath { set; get; }

        [NotMapped]
        public string EncryptedId { get; set; }

        [Display(Name = "入学时间")]
        public DateTime EnrollmentDate { set; get; }

        public ICollection<StudentCourse> StudentCourses { set; get; }
    }
}
