using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watchify.Models
{
    public class MovieProductionCompanies
    {
        public int MovieId { get; set; }
        public List<ProductionCompanies> ProductionCompanies { get; set; }
        public int CompanyId { get; set; }
    }
}
