using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioPerformance.Service.DivvyDiary.DataObjects
{
    public class DivvyDiaryData
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string ISIN { get; set; }
        public string WKN { get; set; }
        public string Exchange { get; set; }
        public IEnumerable<DivvyDiaryDividend> Dividends { get; set; }
    }
}
