﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyEcosystem.BL.BusinessModels;

namespace CompanyEcosystem.BL.Data_Transfer_Object
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public Position Position { get; set; }
    }
}
