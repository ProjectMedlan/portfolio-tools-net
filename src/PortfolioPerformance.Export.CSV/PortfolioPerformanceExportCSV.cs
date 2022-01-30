using PortfolioPerformance.DataObjects;
using System.Text;

namespace PortfolioPerformance.Export.CSV
{
    public class PortfolioPerformanceExportCSV
    {
        [Flags]
        public enum CSVCOLUMNS
        {
            ISIN,
            WKN,
            Symbol,
            Name,
            Porfolio,
            Shares,
            LatestDate,
            LatestValue,
            LatestHigh,
            LatestLow,
            CurrencyCode
        }
        
        private static CSVCOLUMNS[] baseColumnSetup = new CSVCOLUMNS[]
        { 
            CSVCOLUMNS.Name,
            CSVCOLUMNS.ISIN, 
            CSVCOLUMNS.WKN, 
            CSVCOLUMNS.Symbol, 
            CSVCOLUMNS.Porfolio,
            CSVCOLUMNS.Shares,
            CSVCOLUMNS.LatestValue,
            CSVCOLUMNS.LatestHigh,
            CSVCOLUMNS.LatestLow,
            CSVCOLUMNS.LatestDate,
            CSVCOLUMNS.CurrencyCode
        };

        public static void Export(PortfolioPerformanceData data, string targetFile)
        {
            Export(data, targetFile, true, true, baseColumnSetup);
        }

        public static bool Export(PortfolioPerformanceData data, string targetFile, bool addHeader = true, bool removeStocksWithoutShares = true, params CSVCOLUMNS[] columns)
        {
            if (data == null) return false;
            if (data.Securities == null || data.Securities.Count == 0) return false;

            StringBuilder fileContent = new StringBuilder();

            if (addHeader)
            {
                fileContent.AppendLine(CreateHeader(columns));
            }

            // Start here: Generate CSV file
            foreach (Security security in data.Securities)
            {
                // Sold everything
                if (removeStocksWithoutShares && security.Shares == 0)
                    continue;

                fileContent.AppendLine(SecurityToCSVItem(security, removeStocksWithoutShares, columns));
            }
            File.WriteAllText(targetFile, fileContent.ToString());
            return true;
        }

        private static string CreateHeader(params CSVCOLUMNS[] columns)
        {
            List<string> lineParts = new List<string>();
            foreach (var column in columns)
            {
                switch (column)
                {
                    case CSVCOLUMNS.ISIN:
                        lineParts.Add("ISIN");
                        break;
                    case CSVCOLUMNS.WKN:
                        lineParts.Add("WKN");
                        break;
                    case CSVCOLUMNS.Symbol:
                        lineParts.Add("TickerSymbol");
                        break;
                    case CSVCOLUMNS.Name:
                        lineParts.Add("Name");
                        break;
                    case CSVCOLUMNS.Porfolio:
                        lineParts.Add("Portfolio");
                        break;
                    case CSVCOLUMNS.Shares:
                        lineParts.Add("Anteile");
                        break;
                    case CSVCOLUMNS.CurrencyCode:
                        lineParts.Add("Währung");
                        break;
                    case CSVCOLUMNS.LatestValue:
                        lineParts.Add("Letzter Wert");
                        break;
                    case CSVCOLUMNS.LatestHigh:
                        lineParts.Add("Höchster Wert");
                        break;
                    case CSVCOLUMNS.LatestLow:
                        lineParts.Add("Niedrigster Wert");
                        break;
                    case CSVCOLUMNS.LatestDate:
                        lineParts.Add("Letzter Stand");
                        break;
                    default:
                        break;
                }
            }

            return String.Join(";", lineParts);
        }

        private static string SecurityToCSVItem(Security security, bool removeStocksWithoutShares,  params CSVCOLUMNS[] columns)
        {
            // Security splited in more than one portfolio?
            if (security.ShareDetails != null && security.ShareDetails.Count > 0)
            {
                List<String> portfolioNames = security.ShareDetails.Select(x => x.Portfolio).Distinct().ToList();
                StringBuilder lines = new StringBuilder();
                foreach (var portfolio in portfolioNames)
                {

                    // Security in multiple Portfolios, but completely sold in some portfolios?
                    // Check if completely sold -> No export
                    if ((removeStocksWithoutShares) && (double)(security.ShareDetails.Where(x => x.Portfolio.Equals(portfolio)).Sum(x => x.Shares) / 1000.0 / 1000.0) == 0)
                    {
                        continue;
                    }

                    // Add linebreak
                    if (lines.Length > 0)
                    {
                        lines.AppendLine("");
                    }

                    lines.Append(SecurityToCSVItem(security, portfolio, columns));
                }
                return lines.ToString();
            }
            // All in one Portfolio or no share Details
            else
            {
                return SecurityToCSVItem(security, "", columns);
            }
        }

        private static string SecurityToCSVItem(Security security, string portfolioName, params CSVCOLUMNS[] columns)
        {
            List<string> lineParts = new List<string>();
            foreach (var column in columns)
            {
                switch (column)
                {
                    case CSVCOLUMNS.ISIN:
                        lineParts.Add(security.ISIN);
                        break;
                    case CSVCOLUMNS.WKN:
                        lineParts.Add(security.WKN);
                        break;
                    case CSVCOLUMNS.Symbol:
                        lineParts.Add(security.TickerSymbol);
                        break;
                    case CSVCOLUMNS.Name:
                        lineParts.Add(security.Name);
                        break;
                    case CSVCOLUMNS.Porfolio:
                        lineParts.Add(portfolioName);
                        break;
                    case CSVCOLUMNS.Shares:
                        if (!string.IsNullOrEmpty(portfolioName))
                        {
                            lineParts.Add((security.ShareDetails.Where(x => x.Portfolio.Equals(portfolioName)).Sum(x => x.Shares) / 1000.0 / 1000.0).ToString());
                        }
                        else
                        {
                            lineParts.Add((security.Shares / 1000.0 / 1000.0).ToString());
                        }
                        break;
                    case CSVCOLUMNS.CurrencyCode:
                        lineParts.Add(security.CurrencyCode);
                        break;
                    case CSVCOLUMNS.LatestValue:
                        lineParts.Add((security.LatestValue / 1000.0 / 1000.0 / 100.0).ToString());
                        break;
                    case CSVCOLUMNS.LatestHigh:
                        lineParts.Add((security.LatestHigh / 1000.0 / 1000.0 / 100.0).ToString());
                        break;
                    case CSVCOLUMNS.LatestLow:
                        lineParts.Add((security.LatestLow / 1000.0 / 1000.0 / 100.0).ToString());
                        break;
                    case CSVCOLUMNS.LatestDate:
                        lineParts.Add(security.LatestDate.ToString("dd.MM.yyyy"));
                        break;
                    default:
                        break;
                }
            }

            return String.Join(";", lineParts);
        }
    }
}