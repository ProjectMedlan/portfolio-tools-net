using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioPerformance.DataObjects
{
    public class SecurityAttributes
    {
        /// <summary>Vendor</summary>
        public string Vendor { get; set; }
        /// <summary>Assets under management (aum)</summary>
        public long AssetsUnderManagement { get; set; }
        /// <summary>Logo as a base64 string</summary>
        public string Base64Logo { get; set; }
        /// <summary>TER</summary>
        public decimal TotalExpenseRatio { get; set; }
        /// <summary>Acquisition fee</summary>
        public decimal AcquisitionFee { get; set; }
        /// <summary>Management fee)</summary>
        public decimal ManagementFee { get; set; }
    }
}
