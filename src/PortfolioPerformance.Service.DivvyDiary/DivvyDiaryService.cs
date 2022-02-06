using PortfolioPerformance.Service.DivvyDiary.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioPerformance.Service.DivvyDiary
{
    public class DivvyDiaryService
    {
        static HttpClient _client = new HttpClient();

        private const string BASE_URL = "https://api.divvydiary.com/symbols/";

        public DivvyDiaryService()
        {
            _client.DefaultRequestHeaders.Add("Accept", "*/*"); // IMO Nicht notwendig
        }

        public async Task<decimal> GetCurrentDividendForShare(string ISIN)
        {
            decimal rValue = 0;
            HttpResponseMessage response = await _client.GetAsync($"{BASE_URL}{ISIN}");
            string content = await response.Content.ReadAsStringAsync();

            DivvyDiaryData tmpElement = Newtonsoft.Json.JsonConvert.DeserializeObject<DivvyDiaryData>(content);
            if ((tmpElement != null) && (tmpElement.Dividends != null))
            {
                DivvyDiaryDividend dividend = tmpElement.Dividends.FirstOrDefault();
                if (dividend != null)
                {
                    rValue = dividend.Amount;
                }
            }
            return rValue;
        }

        public async Task<DivvyDiaryDividend> GetCurrentDividendForShareWithDetails(string ISIN)
        {
            DivvyDiaryDividend rValue = null;
            HttpResponseMessage response = await _client.GetAsync($"{BASE_URL}{ISIN}");
            string content = await response.Content.ReadAsStringAsync();

            DivvyDiaryData tmpElement = Newtonsoft.Json.JsonConvert.DeserializeObject<DivvyDiaryData>(content);
            if ((tmpElement != null) && (tmpElement.Dividends != null))
            {
                rValue = tmpElement.Dividends.FirstOrDefault();
            }

            return rValue;
        }
    }
}
