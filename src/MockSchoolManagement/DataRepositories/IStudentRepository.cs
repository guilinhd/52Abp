using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MockSchoolManagement.Models;

namespace MockSchoolManagement.DataRepositories
{
    public interface IStudentRepository
    {
        Student GetStudent(int id);

        IEnumerable<Student> GetStudents();

        Student Insert(Student student);

        Student Update(Student student);

        Student Delete(int id);
    }
}
