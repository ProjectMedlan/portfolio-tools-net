using AngleSharp.Dom;
using AngleSharp.Xml.Dom;
using AngleSharp.Xml.Parser;
using PortfolioPerformance.DataObjects;
using System.Runtime.Serialization;

namespace PortfolioPerformanceReader
{
    public class PortfolioPerformanceDataReader
    {
        public static event Action<string> AddLogMessage;

        private static readonly IFormatProvider DATE_FORMAT = new DateTimeFormat("yyyy-MM-dd").FormatProvider;
        private static readonly IFormatProvider TIMESTAMP_FORMAT = new DateTimeFormat("g").FormatProvider;

        private const string TRANSACTION_TYPE_DELIVERY_OUTBOUND = "DELIVERY_OUTBOUND";


        public async static Task<PortfolioPerformanceData> ReadPortfolioPerformanceFile(string portfolioFile)
        {
            return await ReadPortfolioPerformanceFile(portfolioFile, new PortfolioPerformanceDataReaderOptions());
        }

        public async static Task<PortfolioPerformanceData> ReadPortfolioPerformanceFile(string portfolioFile, PortfolioPerformanceDataReaderOptions options)
        {
            PortfolioPerformanceData data = new PortfolioPerformanceData();

            AddLogMessage?.Invoke("Lade Datei: " + portfolioFile);

            XmlParser parser = new XmlParser();
            IXmlDocument doc = await parser.ParseDocumentAsync(File.ReadAllText(portfolioFile));

            // Read all the data
            IElement rootElement = doc.QuerySelector("client");

            Int32.TryParse(rootElement.QuerySelector("version").InnerHtml, out int fileVersion);
            data.Version = fileVersion;
            data.BaseCurrency = rootElement.QuerySelector("baseCurrency").InnerHtml;

            // Read all the securities and their data
            int itemNumber = 1;
            foreach (IElement item in doc.QuerySelectorAll("security"))
            {
                // Honestly I don't know anymore why there are some without children
                if (item.Children.Count() == 0)
                {
                    continue;
                }

                AddLogMessage?.Invoke($"Aktie {itemNumber}");

                Security security = new Security();
                security.ID = itemNumber++;
                security.Uuid = item.QuerySelector("uuid").InnerHtml;
                security.Name = item.QuerySelector("name").InnerHtml.Replace("&amp;", "&");
                security.CurrencyCode = item.QuerySelector("currencyCode").InnerHtml;
                security.Note = item.QuerySelector("note")?.InnerHtml;
                IElement latestElement = item.QuerySelector("latest");
                if (latestElement != null)
                {
                    if (latestElement.GetAttribute("t") != null)
                    {
                        security.LatestDate = DateOnly.Parse(latestElement.GetAttribute("t"), DATE_FORMAT);
                    }
                    if (latestElement.GetAttribute("v") != null)
                    {
                        security.LatestValue = Convert.ToInt64(latestElement.GetAttribute("v"));
                    }
                    security.LatestHigh = Convert.ToInt64(latestElement.QuerySelector("high")?.InnerHtml);
                    security.LatestLow = Convert.ToInt64(latestElement.QuerySelector("low")?.InnerHtml);
                    security.LatestVolume = Convert.ToInt64(latestElement.QuerySelector("volume")?.InnerHtml);
                }

                security.ISIN = item.QuerySelector("isin")?.InnerHtml;
                security.WKN = item.QuerySelector("wkn")?.InnerHtml;
                security.TickerSymbol = item.QuerySelector("tickerSymbol")?.InnerHtml;
                security.Feed = item.QuerySelector("feed")?.InnerHtml;
                security.UpdatedAt = DateTime.Parse(item.QuerySelector("updatedAt").InnerHtml, TIMESTAMP_FORMAT);
                security.IsRetired = Convert.ToBoolean(item.QuerySelector("isRetired")?.InnerHtml);

                if (options.WithSecurityPriceHistory)
                {
                    // Only init, when option 'with Prices' selected
                    security.Prices = new List<Price>();
                    foreach (IElement priceItem in item.QuerySelector("prices").QuerySelectorAll("price"))
                    {
                        Price price = new Price();
                        if (priceItem.GetAttribute("t") != null)
                        {
                            price.Date = DateOnly.Parse(priceItem.GetAttribute("t"), DATE_FORMAT);
                        }
                        if (priceItem.GetAttribute("v") != null)
                        {
                            price.Value = Convert.ToInt64(priceItem.GetAttribute("v"));
                        }
                    }
                }

                if (options.WithAttributes)
                {
                    // Only init, when option 'with Attributes' selected
                    security.Attributes = new SecurityAttributes();

                    foreach (IElement entryItem in item.QuerySelector("attributes").QuerySelector("map").QuerySelectorAll("entry"))
                    {
                        // Entry hast two childs: Descriptions & Value
                        if (entryItem.Children.Count() == 2)
                        {
                            switch (entryItem.Children[0].InnerHtml?.ToLower())
                            {
                                case "vendor":
                                    security.Attributes.Vendor = entryItem.Children[1].InnerHtml;
                                    break;
                                case "aum":
                                    security.Attributes.AssetsUnderManagement = Convert.ToInt64(entryItem.Children[1].InnerHtml);
                                    break;
                                case "logo":
                                    // Starts with: data:image/png;base64 ?
                                    string logoidentifier = "data:image/png;base64,";
                                    string logo = entryItem.Children[1].InnerHtml;
                                    if (logo.StartsWith(logoidentifier))
                                    {
                                        security.Attributes.Base64Logo = logo.Substring(logoidentifier.Length);
                                    }
                                    break;
                                case "ter":
                                    security.Attributes.TotalExpenseRatio = Convert.ToDecimal(entryItem.Children[1].InnerHtml);
                                    break;
                                case "managementfee":
                                    security.Attributes.ManagementFee = Convert.ToDecimal(entryItem.Children[1].InnerHtml);
                                    break;
                                case "acquisitionfee":
                                    security.Attributes.AcquisitionFee = Convert.ToDecimal(entryItem.Children[1].InnerHtml);
                                    break;
                            }
                        }
                    }
                }

                AddLogMessage?.Invoke($"Aktie {itemNumber}; ISIN {security.ISIN}; WKN {security.WKN}");

                data.Securities.Add(security);
            }

            AddLogMessage?.Invoke($"Alle Aktien gelesen - Anzahl: {data.Securities.Count}");

            // TODO: Read watchlists? Tag: <watchlists>
            /*
            foreach (IElement item in doc.QuerySelectorAll(""))
            {
            }
            */

            // TODO: Read Accounts?
            // Account Transactions: Type Dividends -> Payout

            // Read: Security transactions
            foreach (IElement transactionItem in doc.QuerySelectorAll("portfolio-transaction"))
            {
                string id_string = transactionItem.QuerySelector("security").GetAttribute("reference");
                int itemID = 1;
                if (id_string.Contains("["))
                {
                    int start = id_string.IndexOf("[") + 1;
                    int end = id_string.IndexOf("]");
                    itemID = Convert.ToInt32(id_string.Substring(start, end - start));
                }

                AddLogMessage?.Invoke($"Lese Kauf/Verkauf: {itemID}");

                // Sometimes there's this crossEntry .... remove it.
                IElement crossEntry = transactionItem.QuerySelector("crossEntry");
                if (crossEntry != null)
                {
                    transactionItem.RemoveChild(crossEntry);
                }

                long shares = Convert.ToInt64(transactionItem.QuerySelector("shares").InnerHtml);
                AddLogMessage?.Invoke($"Lese Kauf/Verkauf: {itemID} - Shares {shares}");
                long amount = Convert.ToInt64(transactionItem.QuerySelector("amount").InnerHtml);
                AddLogMessage?.Invoke($"Lese Kauf/Verkauf: {itemID} - Shares {amount}");

                string dateReference = transactionItem.QuerySelector("date").InnerHtml;
                if (string.IsNullOrEmpty(dateReference))
                {
                    dateReference = transactionItem.QuerySelector("date").Attributes[0].Value;
                }

                DateTime? date = null;
                IElement parent = transactionItem;

                // We find the portfolio name above the transaction.
                string portfolioName = "";
                if ((parent.ParentElement != null) && (parent.ParentElement.ParentElement != null))
                {
                    portfolioName = parent.ParentElement.ParentElement.QuerySelector("name").InnerHtml;
                }

                // 2 Review: My currentfile does not contain '../'
                // for each '../' set parent as parent.ParentElement
                if (dateReference.Contains("../"))
                {
                    while (dateReference.Contains("../"))
                    {
                        dateReference = dateReference.Substring(3);
                        if (dateReference.Contains("../"))
                        {
                            parent = parent.ParentElement;
                        }
                    }
                    dateReference = parent.QuerySelector("date").InnerHtml;
                    date = Convert.ToDateTime(dateReference);
                }
                else
                {
                    date = Convert.ToDateTime(dateReference);
                }

                // Sold? -> shares * (-1)
                if (transactionItem.QuerySelector("type").InnerHtml == TRANSACTION_TYPE_DELIVERY_OUTBOUND)
                {
                    AddLogMessage?.Invoke($"Lese Kauf/Verkauf: {itemID} - Es ist ein Verkauf!");
                    shares *= (-1);
                    amount *= (-1);
                }

                // Find Security in List
                Security security = data.Securities.Find(x => x.ID == itemID);
                if (security != null)
                {
                    AddLogMessage?.Invoke($"Lese Kauf/Verkauf: {itemID} - Aktie gefunden: {security.Name} - Shares werden angepasst");

                    shares = shares / 100;

                    security.Shares += shares;

                    if (security.ShareDetails == null)
                        security.ShareDetails = new List<SecurityShareDetail>();

                    security.ShareDetails.Add(new SecurityShareDetail { Date = (date.GetValueOrDefault(DateTime.MinValue)), Shares = shares, Portfolio = portfolioName, TotalValue = amount });
                }
                else
                {
                    // Throw error
                }

            }

            // TODO: Read plans? Tag: <plans>
            // TODO: Read taxonomies? Tag: <taxonomies>
            // TODO: Read dashboards? Tag: <dashboards>
            // TODO: Read properties? Tag: <properties>
            // TODO: Read settings? Tag: <settings>
            /*
            foreach (IElement item in doc.QuerySelectorAll(""))
            {
            }
            */

            return data;
        }
    }
}