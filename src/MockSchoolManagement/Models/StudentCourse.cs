using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MockSchoolManagement.Models
{
    public class StudentCourse
    {
        [Key]
        public int StudentCourseId { set; get; }

        public int CourseId { set; get; }

        public int StudentId { set; get; }

        public Course Course { set; get; }

        public Student Student { set; get; }
    }
}
