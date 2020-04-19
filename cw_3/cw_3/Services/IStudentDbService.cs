using cw_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw_3.Services
{
    interface IStudentDbService
    {
        String AddStudent(NewStudent student);
        void PromoteStudents(string studies, int semester);
    }
}
