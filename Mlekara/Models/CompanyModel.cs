﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mlekara.Models
{
    public class CompanyModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool FirstStartup { get; set; }

        public CompanyModel()
        {
        }

        public CompanyModel(int id, string name, bool firstStartup)
        {
            Id = id;
            Name = name;
            FirstStartup = firstStartup;
        }
    }
}
