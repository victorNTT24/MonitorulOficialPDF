﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorulOficialPDF
{
    public class Link
    {
        public string URL { get; set; }
        public string Name { get; set; }

        public Link()
        { 
        }

        public Link(string url, string name)
        {
            URL = url;
            Name = name;
        }
    }
}
