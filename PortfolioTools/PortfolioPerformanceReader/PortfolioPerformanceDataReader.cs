using PortfolioPerformanceReader.DataObjects;

namespace PortfolioPerformanceReader
{
    public class PortfolioPerformanceDataReader
    {
        public async static Task<PortfolioPerformanceData> ReadPortfolioPerformanceFile(string portfolioFile)
        {
            PortfolioPerformanceData data = new PortfolioPerformanceData();

            // AngleSharp.Xml
            // XmlParser parser = new XmlParser();
            // IXmlDocument doc = await parser.ParseDocumentAsync(File.ReadAllText(portfoliofile));

            // TODO: Fill data with data :)

            return data;
        }
    }
}