﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostavkaTovarov.models
{
    internal class Users
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string role { get; set; }
    }
}
