using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watchify.Models
{
    public class Movie
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Release_Date { get; set; }  // or use CustomDate
        public string Overview { get; set; }
        public double Rating { get; set; }
        public double Popularity { get; set; }
        public int Runtime { get; set; }
        public long Budget { get; set; }
        public long Revenue { get; set; }
        public string Poster_Path { get; set; }
    }

}
