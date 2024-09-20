using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watchify.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Overview { get; set; }
        public double Rating { get; set; }
        public double Popularity { get; set; }
        public int Runtime { get; set; }
        public long Budget { get; set; }
        public long Revenue { get; set; }
        public string PosterPath { get; set; }
        public List<Genre> Genres { get; set; }
        public List<ProductionCompany> ProductionCompanies { get; set; }
        public List<Video> Videos { get; set; }
        public List<Crew> Crew { get; set; }
        public List<Cast> Cast { get; set; }
    }
}
