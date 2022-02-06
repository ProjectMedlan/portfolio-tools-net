using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace PortfolioPerformance.Service.DivvyDiary.DataObjects
{
    public class DivvyDiaryDividend
    {
        // [JsonConverter(typeof(DateOnlyJsonConverter))]
        [JsonConverter(typeof(DateOnlyFormatConverter), "yyyy-MM-dd")]
        public DateTime ExDate { get; set; }

        // [JsonConverter(typeof(DateOnlyJsonConverter))]
        [JsonConverter(typeof(DateOnlyFormatConverter), "yyyy-MM-dd")]
        public DateTime PayDate { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
