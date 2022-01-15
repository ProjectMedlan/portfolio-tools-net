using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioPerformanceReader.DataObjects
{
    public class PortfolioPerformanceDataReaderOptions
    {
        /// <summary>Read price history</summary>
        public bool WithSecurityPriceHistory { get; set; } = true;
        /// <summary>Read buy/sell data</summary>
        public bool WithBuySellData { get; set; } = true;
        /// <summary>Read atributes</summary>
        public bool WithAttributes { get; set; } = true;
    }
}
