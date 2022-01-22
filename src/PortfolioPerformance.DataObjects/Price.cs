using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioPerformance.DataObjects
{
    public class Price
    {
        /// <summary>Value</summary>
        public long Value { get; set; }
        /// <summary>Date</summary>
        public DateOnly Date { get; set; }
    }
}
