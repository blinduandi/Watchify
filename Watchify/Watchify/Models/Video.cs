using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watchify.Models
{
    public class Video
    {
        public string ID { get; set; }
        public int MovieId { get; set; }
        public string Name { get; set; }
        public string VType { get; set; }
        public string Url { get; set; }
    }
}
