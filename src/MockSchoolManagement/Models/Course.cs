using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace MockSchoolManagement.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CourseId { set; get; }

        public string Title { set; get; }

        public int Credits { set; get; }

        public ICollection<StudentCourse> StudentCourses { set; get; }
    }
}
