using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioPerformanceReader.DataObjects
{
    public class Security
    {
        public string Uuid { get; set; } = "";
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public DateTime LastUpdate { get; set; }
        public long LastValue { get; set; }
        public long Shares { get; set; }
        public string ISIN { get; set; } = "";
        public string WKN { get; set; } = "";
        public string TickerSymbol { get; set; } = "";
        public string Feed { get; set; } = "";

        
    }
}
