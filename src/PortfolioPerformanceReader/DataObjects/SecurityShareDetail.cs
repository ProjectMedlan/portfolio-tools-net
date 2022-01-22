using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioPerformanceReader.DataObjects
{
    public class SecurityShareDetail
    {
        /// <summary>Portfolio name</summary>
        public string Portfolio { get; set; }
        /// <summary>Date</summary>
        public DateTime Date { get; set; }
        /// <summary>Shares</summary>
        public decimal Shares { get; set; }
        /// <summary>TotalValue</summary>
        public decimal TotalValue { get; set; }
    }
}
