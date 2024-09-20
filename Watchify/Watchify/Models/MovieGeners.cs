using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watchify.Models
{
    public class MovieGenres
    {
        public int MovieId { get; set; }
        public List<Genres> Genre { get; set; }
        public int GenreId { get; set; }
    }
}
