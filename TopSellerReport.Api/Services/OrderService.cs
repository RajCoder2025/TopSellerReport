using CsvHelper;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using TopSellerReport.Api.Models;

namespace TopSellerReport.Api.Services
{
    public class OrderService
    {
        private readonly string _csvPath;

        // Constructor accepts csvPath for flexibility
        public OrderService(string csvPath = null)
        {
            _csvPath = csvPath ?? Path.Combine(Directory.GetCurrentDirectory(), "orders.csv");
        }

        public List<string> GetBranches()
        {
            using var reader = new StreamReader(_csvPath, Encoding.UTF8);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<OrderRecord>().ToList();

            return records
                .Select(r => r.Branch)
                .Distinct()
                .OrderBy(b => b)
                .ToList();
        }

        public List<TopSellerResult> GetTopSellersByMonth(string branch)
        {
            using var reader = new StreamReader(_csvPath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<OrderRecord>()
                .Where(r => r.Branch.Equals(branch, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var groupedByMonth = records
                .GroupBy(r => new { Year = r.OrderDate.Year, Month = r.OrderDate.Month })
                .OrderBy(g => g.Key.Month);

            var result = new List<TopSellerResult>();

            foreach (var group in groupedByMonth)
            {
                var topSeller = group
                    .GroupBy(r => r.Seller)
                    .Select(g => new
                    {
                        Seller = g.Key,
                        TotalOrders = g.Count(),
                        TotalSales = g.Sum(x => x.Price)
                    })
                    .OrderByDescending(x => x.TotalSales)
                    .First();

                var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(group.Key.Month);

                result.Add(new TopSellerResult
                {
                    Month = monthName,
                    Seller = topSeller.Seller,
                    TotalOrders = topSeller.TotalOrders,
                    TotalSales = topSeller.TotalSales
                });
            }

            return result;
        }
    }
}
