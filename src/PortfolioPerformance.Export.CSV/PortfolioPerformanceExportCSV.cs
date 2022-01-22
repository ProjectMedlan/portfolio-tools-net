using PortfolioPerformance.DataObjects;

namespace PortfolioPerformance.Export.CSV
{
    public class PortfolioPerformanceExportCSV
    {
        private static string[] allColumns = new string[] { "" };

        public static void Export(PortfolioPerformanceData data, string targetFile)
        {
            Export(data, targetFile, allColumns);
        }

        public static void Export(PortfolioPerformanceData data, string targetFile, params string[] columns)
        {
            // Start here: Generate CSV file
        }
    }
}