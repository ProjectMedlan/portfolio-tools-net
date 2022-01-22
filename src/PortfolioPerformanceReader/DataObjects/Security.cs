using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioPerformanceReader.DataObjects
{
    public class Security
    {
        /// <summary> Uuid </summary>
        public string Uuid { get; set; }
        /// <summary> Security Position in file </summary>
        public int ID { get; set; }
        /// <summary> Security Name </summary>
        public string Name { get; set; }
        /// <summary> Lastest refresh date </summary>
        public DateOnly LatestDate { get; set; }
        /// <summary> Lastest current value </summary>
        public long LatestValue { get; set; }
        /// <summary> Lastest high value </summary>
        public long LatestHigh { get; set; }
        /// <summary> Lastest low value </summary>
        public long LatestLow { get; set; }
        /// <summary> Lastest current volume </summary>
        public long LatestVolume { get; set; }
        /// <summary> Amout of shares </summary>
        public long Shares { get; set; }
        /// <summary> ISIN </summary>
        public string ISIN { get; set; }
        /// <summary> WKN </summary>
        public string WKN { get; set; }
        /// <summary> TickerSymbol </summary>
        public string TickerSymbol { get; set; }
        /// <summary> Currency </summary>
        public string Feed { get; set; }
        /// <summary> Currency code </summary>
        public string CurrencyCode { get; set; }
        /// <summary> Note </summary>
        public string Note { get; set; }
        /// <summary> Is retired </summary>
        public bool IsRetired { get; set; }
        /// <summary> Updated At </summary>
        public DateTime UpdatedAt { get; set; }
        /// <summary> Price history </summary>
        public List<Price> Prices { get; set; }
        /// <summary> Selected Attributes </summary>
        public SecurityAttributes Attributes { get; set; }
        /// <summary>List of transaction Details</summary>
        public List<SecurityShareDetail> ShareDetails { get; set; }

        // TOOD: More to read? Events?

    }
}
