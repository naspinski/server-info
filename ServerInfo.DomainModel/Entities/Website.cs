﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerInfo.DomainModel.Entities
{
    public class Website
    {
        public string Name { get; set; }
        public int? Tcp { get; set; }
        public int? Ssl { get; set; }
    }
}
