﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watchify.Models
{
    public class Crew
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Job { get; set; }
        public int MovieId { get; set; }
    }
}