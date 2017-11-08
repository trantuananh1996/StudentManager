using exam.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using exam.Models;

namespace StudentManager.Repository.Point
{
    public class PointRepository : Repository<Models.Point.Point>, IPointRepository
    {
        public PointRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
