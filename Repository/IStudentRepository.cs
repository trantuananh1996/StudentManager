using exam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exam.Repository
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        Task<List<Student>> FindStudentByClass(int classId);
        Task<List<Student>> FindStudentByName(string name);
    }
}
