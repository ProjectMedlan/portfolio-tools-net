using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioPerformanceReader.DataObjects
{
    public class PortfolioPerformanceData
    {
        public List<Security> Securities { get; set; } = new List<Security>();
    }
}
