﻿using exam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace exam.Repository
{
    public interface IRoleRepository:IBaseRepository<Role>
    {
        Task<Role> FindRoleById(int id);
    }
}