﻿using exam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exam.Repository
{
    public class ReligionRepository : Repository<Religion>
    {
        public ReligionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
