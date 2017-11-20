using exam.Models;
using exam.Repository;

namespace StudentManager.Repository
{
    public class SemesterRepository : Repository<SchoolYear>
    {
        public SemesterRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
