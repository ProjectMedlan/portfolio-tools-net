using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDividendsPreview
{
    internal class DividendReportObject
    {
        public string Name { get; set; }
        public decimal Shares { get; set; }
        public decimal Dividend { get; set; }
        public string CurrencyCode { get; set; }
        public DateOnly ExDate { get; set; }
        public DateOnly PayDate { get; set; }
    }
}
