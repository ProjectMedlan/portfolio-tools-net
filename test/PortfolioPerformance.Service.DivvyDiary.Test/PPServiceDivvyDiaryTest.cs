using Xunit;

namespace PortfolioPerformance.Service.DivvyDiary.Test
{
    public class PPServiceDivvyDiaryTest
    {
        const string MICROSOFT_ISIN = "US5949181045";


        [Theory]
        [InlineData(MICROSOFT_ISIN)]
        public async void  GetDividendAmount(string isin)
        {
            DivvyDiaryService service = new DivvyDiaryService();
            decimal amount = await service.GetCurrentDividendForShare(isin);
            Assert.NotEqual(0, amount);
        }


        [Theory]
        [InlineData(MICROSOFT_ISIN)]
        public async void GetDividenWithDetails(string isin)
        {
            DivvyDiaryService service = new DivvyDiaryService();
            DivvyDiary.DataObjects.DivvyDiaryDividend data = await service.GetCurrentDividendForShareWithDetails(isin);
            Assert.NotNull(data);
        }
    }
}