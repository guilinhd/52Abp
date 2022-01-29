using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MockSchoolManagement.Models;

namespace MockSchoolManagement.DataRepositories
{
    interface IStudentRepository
    {
        Student GetStudent(int id);
    }
}
