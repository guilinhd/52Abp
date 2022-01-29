using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MockSchoolManagement.Models;

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
                Email = "zhangsan@hotmail.com"
            });

            _students.Add(new Student()
            {
                Id = 2,
                Name = "李四",
                Email = "lisi@hotmail.com"
            });
        }

        public Student GetStudent(int id)
        {
            Student student = _students.Where(f => f.Id == 1).FirstOrDefault();

            return student;
        }
    }
}
