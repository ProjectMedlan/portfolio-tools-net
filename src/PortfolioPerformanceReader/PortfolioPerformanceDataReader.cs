using AngleSharp.Dom;
using AngleSharp.Xml.Dom;
using AngleSharp.Xml.Parser;
using PortfolioPerformanceReader.DataObjects;
using System.Runtime.Serialization;

namespace PortfolioPerformanceReader
{
    public class PortfolioPerformanceDataReader
    {
        public static event Action<string> AddLogMessage;

        private static readonly IFormatProvider DATE_FORMAT = new DateTimeFormat("yyyy-MM-dd").FormatProvider;
        private static readonly IFormatProvider TIMESTAMP_FORMAT = new DateTimeFormat("g").FormatProvider;

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


            /*

                // security.ShareListWithDate = new List<Tuple<DateTime, long>>();
                // security.ShareDetails = new List<Share>();

            List<StockItem> allStockItems = new List<StockItem>();

            // Die Käufe zuordnen
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

                // Bei mir nicht drin - bei Sven schon
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

                // Da holen wir uns schnell das Depot (Transactionen, darüber sind die Portfolios mit Namen
                string portfolioName = "";
                if ((parent.ParentElement != null) && (parent.ParentElement.ParentElement != null))
                {
                    portfolioName = parent.ParentElement.ParentElement.QuerySelector("name").InnerHtml;
                }

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

                // Verkauf? Dann gehen die Shares in den negativen Bereich
                if (transactionItem.QuerySelector("type").InnerHtml == "DELIVERY_OUTBOUND")
                {
                    AddLogMessage?.Invoke($"Lese Kauf/Verkauf: {itemID} - Es ist ein Verkauf!");
                    shares *= (-1);
                    amount *= (-1);
                }

                // Aktien finden
                StockItem s = allStockItems.Find(x => x.ID == itemID);
                if (s != null)
                {
                    AddLogMessage?.Invoke($"Lese Kauf/Verkauf: {itemID} - Aktie gefunden: {s.Name} - Shares werden angepasst");

                    shares = shares / 100;

                    s.Shares += shares;
                    
                    // Das kann dann eigentlich weg
                    s.ShareListWithDate.Add(new Tuple<DateTime, long>(date.GetValueOrDefault(DateTime.MinValue), shares));
                    
                    // Und durch das hier ersetzt werden
                    s.ShareDetails.Add(new Share { Date = (date.GetValueOrDefault(DateTime.MinValue)), Shares = shares, Portfolio = portfolioName, Amount = amount });
                }
                else
                {
                    // Throw error
                }
            }

            return allStockItems;
        }
            */

            return data;
        }
    }
}