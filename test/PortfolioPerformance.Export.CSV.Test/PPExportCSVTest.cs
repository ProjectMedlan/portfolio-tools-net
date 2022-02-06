using PortfolioPerformanceReader;
using System.Diagnostics;
using System.IO;
using Xunit;

namespace PortfolioPerformance.Export.CSV.Test
{
    public class PPExportCSVTest
    {
        const string localPPFile = @"";

        [Theory]
        [InlineData(localPPFile)]
        public async void FileExistsWithContent(string file)
        {
            string tagetFile = Path.GetTempFileName();

            var data = await PortfolioPerformanceDataReader.ReadPortfolioPerformanceFile(file);
            PortfolioPerformanceExportCSV.Export(data, tagetFile);
            Assert.True(File.Exists(tagetFile));

            FileInfo fi = new FileInfo(tagetFile);
            Assert.NotEqual(0, fi.Length);

            File.Delete(tagetFile);
        }

        [Theory]
        [InlineData(localPPFile)]
        public async void FileExistsWithContent_DebugNoCleanup(string file)
        {
            string tagetFile = Path.GetTempFileName();
            tagetFile = tagetFile + ".csv";

            var data = await PortfolioPerformanceDataReader.ReadPortfolioPerformanceFile(file);
            data.Securities.Sort((x, y) => x.Name.CompareTo(y.Name));
            PortfolioPerformanceExportCSV.Export(data, tagetFile);
            

            Assert.True(File.Exists(tagetFile));

            FileInfo fi = new FileInfo(tagetFile);
            Assert.NotEqual(0, fi.Length);
            
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("explorer", Path.GetDirectoryName(tagetFile)));
        }
    }
}