using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioPerformanceReader.DataObjects
{
    public class PortfolioPerformanceData
    {
        /// <summary>Fileversion</summary>
        public int Version { get; set; }
        /// <summary>Base currency</summary>
        public string BaseCurrency { get; set; }

        /// <summary>List of securities</summary>
        public List<Security> Securities { get; set; } = new List<Security>();
    }
}
