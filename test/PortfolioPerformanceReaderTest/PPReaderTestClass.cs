using PortfolioPerformanceReader;
using Xunit;

namespace PortfolioPerformanceReaderTests
{
    public class PPReaderTestClass
    {
        const string localPPFile = @"";

        [Theory]
        [InlineData(localPPFile)]
        public async void FileReadNoExceptionImplicit(string file)
        {
            // Brad Wilson describes it on github: Think of it this way: every line of code you write outside of
            // a try block has an invisible Assert.DoesNotThrow around it.
            await PortfolioPerformanceDataReader.ReadPortfolioPerformanceFile(file);
        }
        [Theory]
        [InlineData(localPPFile)]
        public async void FileReadNoExceptionExplicit(string file)
        {
            var exception = await Record.ExceptionAsync(async () => await PortfolioPerformanceDataReader.ReadPortfolioPerformanceFile(file));
            Assert.Null(exception);
        }

        [Theory]
        [InlineData(localPPFile)]
        public async void FileContainsSecurities(string file)
        {
            PortfolioPerformanceReader.DataObjects.PortfolioPerformanceData? data = await PortfolioPerformanceDataReader.ReadPortfolioPerformanceFile(file);
            Assert.NotNull(data);
            Assert.NotNull(data.Securities);
            Assert.NotEmpty(data.Securities);
        }

    }
}