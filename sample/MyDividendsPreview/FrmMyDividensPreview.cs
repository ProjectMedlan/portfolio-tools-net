using PortfolioPerformance.DataObjects;
using PortfolioPerformance.Reader;
using PortfolioPerformance.Service.DivvyDiary;
using PortfolioPerformance.Service.DivvyDiary.DataObjects;

namespace MyDividendsPreview
{
    public partial class FrmMyDividensPreview : Form
    {
        public FrmMyDividensPreview()
        {
            InitializeComponent();
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            // Read File
            string file = txtFilename.Text;
            if (!File.Exists(file))
            {
                updateStatus("Datei existiert nicht");
                return;
            }
            PortfolioPerformanceData portfolioData = await readPortfolioPerformanceData(file);

            // Read Dividens
            Dictionary<String, DivvyDiaryDividend> dividends = await readDividens(portfolioData);

            // Prepare results
            List<DividendReportObject> reportData = prepareUserDividens(portfolioData, dividends);

            // Show data
            showReport(reportData);

            updateStatus("Fertig!");
        }

        private void updateStatus(string message)
        {
            if (lblStatus.InvokeRequired)
            {
                this.Invoke(new Action<string>(updateStatus), message);
                return;
            }

            lblStatus.Text = $"Status: {message}";
            Application.DoEvents();
        }

        private async Task<PortfolioPerformanceData> readPortfolioPerformanceData(string file)
        {
            updateStatus("Lese Datei ...");
            return await PortfolioPerformanceDataReader.ReadPortfolioPerformanceFile(file);
        }

        private async Task<Dictionary<String, DivvyDiaryDividend>> readDividens(PortfolioPerformanceData portfolioData)
        {
            Dictionary<String, DivvyDiaryDividend> resultList = new Dictionary<String, DivvyDiaryDividend>();
            DivvyDiaryService divvyService = new DivvyDiaryService();
            foreach (var item in portfolioData.Securities.Select(x => x.ISIN).Distinct())
            {
                updateStatus($"Lese Dividende für {item}");

                DivvyDiaryDividend result = await divvyService.GetCurrentDividendForShareWithDetails(item);
                if (result != null)
                {
                    resultList.Add(item, result);
                }
            }

            return resultList;
        }

        private List<DividendReportObject> prepareUserDividens(PortfolioPerformanceData portfolioData, Dictionary<String, DivvyDiaryDividend> dividends)
        {
            updateStatus($"Daten aufbereiten ...");

            // Loop through securitys. Check if there's a current dividen. Calc amount for ex date
            List<DividendReportObject> reportResult = new List<DividendReportObject>();

            foreach (Security security in portfolioData.Securities)
            {
                if (string.IsNullOrEmpty(security.ISIN)) continue; // No ISIN, no dividend -> continue
                if (!dividends.ContainsKey(security.ISIN)) continue; // No dividend -> continue
                
                // Select dividen data
                DivvyDiaryDividend dividenData = dividends[security.ISIN];

                if (dividenData.PayDate < DateOnly.FromDateTime(DateTime.Now.Date)) continue; // No future dividend -> continue

                // calc amount for ex date (ignore portfolio)
                decimal totalShares = security.ShareDetails.FindAll(x => DateOnly.FromDateTime(x.Date.Date) <= dividenData.ExDate).Sum(x => x.Shares);

                DividendReportObject result = new DividendReportObject();
                result.Shares = totalShares / 1000 / 1000;
                result.CurrencyCode = dividenData.Currency;
                result.ExDate = dividenData.ExDate;
                result.PayDate = dividenData.PayDate;
                result.Dividend = dividenData.Amount;
                result.Name = security.Name;
                reportResult.Add(result);
            }
            return reportResult;
        }

        private void showReport(List<DividendReportObject> reportData)
        {
            updateStatus($"Daten anzeigen ...");
            lvwDividends.Items.Clear();
            
            reportData.Sort((x,y) => x.PayDate.CompareTo(y.PayDate));
            foreach (var entry in reportData)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = entry.Name;
                lvi.SubItems.Add(entry.Shares.ToString());
                lvi.SubItems.Add(entry.Dividend.ToString());
                lvi.SubItems.Add((entry.Shares * entry.Dividend).ToString());
                lvi.SubItems.Add(entry.CurrencyCode);
                lvi.SubItems.Add(entry.ExDate.ToString("dd.MM.yyyy"));
                lvi.SubItems.Add(entry.PayDate.ToString("dd.MM.yyyy"));
                lvwDividends.Items.Add(lvi);
            }
        }



    
    }
}