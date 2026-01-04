// See https://aka.ms/new-console-template for more information
using PortfolioPerformance.DataObjects;
using System.Diagnostics;

PortfolioPerformanceData data = await PortfolioPerformance.Reader.PortfolioPerformanceDataReader.ReadPortfolioPerformanceFile(@"D:\Daten\Dokumente\Banken & Finanzen\Portfolio Performance\medlan_alles.xml");
data.Securities.Sort((x, y) => x.Name.CompareTo(y.Name));

foreach (var item in data.Securities)
{
    Debug.WriteLine($"{item.ISIN};{item.Shares / 1000.0 / 1000};{item.Name}");
}
