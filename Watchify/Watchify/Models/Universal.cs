using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watchify.Models
{
    public class Universal
    {
        public Movie? Movie { get; set; }
        public MovieGenres? Genres { get; set; }
        public MovieProductionCompanies? Production { get; set; }
        public List<Video_Path>? Video_Path { get; set; }
        public List<Cast>? Cast { get; set; }
        public List<Crew>? Crew { get; set; }
    }
}
