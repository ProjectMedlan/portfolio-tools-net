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

        public async static Task<PortfolioPerformanceData> ReadPortfolioPerformanceFile(string portfolioFile)
        {
            PortfolioPerformanceData data = new PortfolioPerformanceData();

            AddLogMessage?.Invoke("Lade Datei: " + portfolioFile);

            XmlParser parser = new XmlParser();
            IXmlDocument doc = await parser.ParseDocumentAsync(File.ReadAllText(portfolioFile));

            // Die bestehenden Aktien auslesen
            int itemNumber = 1;
            foreach (IElement item in doc.QuerySelectorAll("security"))
            {
                // Keine Ahnung woher das kam ....
                if (item.Children.Count() == 0)
                {
                    continue;
                }

                AddLogMessage?.Invoke($"Aktie {itemNumber}");

                Security security = new Security();
                // security.ShareListWithDate = new List<Tuple<DateTime, long>>();
                // security.ShareDetails = new List<Share>();
                security.ID = itemNumber++;
                security.Uuid = item.QuerySelector("uuid").InnerHtml;
                security.Name = item.QuerySelector("name").InnerHtml.Replace("&amp;", "&");
                if (item.QuerySelector("latest")?.GetAttribute("t") != null)
                {
                    security.LastUpdate = DateTime.Parse(item.QuerySelector("latest")?.GetAttribute("t"), new DateTimeFormat("yyyy-MM-dd").FormatProvider);
                }
                if (item.QuerySelector("latest")?.GetAttribute("v") != null)
                {
                    security.LastValue = Convert.ToInt64(item.QuerySelector("latest")?.GetAttribute("v"));
                }
                security.ISIN = item.QuerySelector("isin")?.InnerHtml;
                security.WKN = item.QuerySelector("wkn")?.InnerHtml;
                security.TickerSymbol = item.QuerySelector("tickerSymbol")?.InnerHtml;
                security.Feed = item.QuerySelector("feed")?.InnerHtml;

                AddLogMessage?.Invoke($"Aktie {itemNumber}; ISIN {security.ISIN}; WKN {security.WKN}");

                data.Securities.Add(security);
            }

            AddLogMessage?.Invoke($"Alle Aktien gelesen - Anzahl: {data.Securities.Count}");


            



            /*
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