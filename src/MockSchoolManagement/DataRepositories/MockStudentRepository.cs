using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using MockSchoolManagement.Models;
using MockSchoolManagement.Models.EnumTypes;

namespace MockSchoolManagement.DataRepositories
{
    public class MockStudentRepository : IStudentRepository
    {
        List<Student> _students = new List<Student>();

        public MockStudentRepository()
        {
            _students.Add(new Student() {
                Id = 1,
                Name = "张三",
                Major = MajorEnum.ComputerScience,
                Email = "zhangsan@hotmail.com"
            });

            _students.Add(new Student()
            {
                Id = 2,
                Name = "李四",
                Major = MajorEnum.ElectronicCommerce,
                Email = "lisi@hotmail.com"
            });

            _students.Add(new Student()
            {
                Id = 3,
                Name = "王五",
                Major = MajorEnum.Mathematics,
                Email = "wangwu@hotmail.com"
            });
        }

        public Student Add(Student student)
        {
            student.Id = _students.Max(f => f.Id) + 1;
            //student.Major = MajorEnum.
            _students.Add(student);

            return student;
        }

        public Student GetStudent(int id)
        {
            Student student = _students.Where(f => f.Id == id).FirstOrDefault();

            return student;
        }

        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }
    }
}
