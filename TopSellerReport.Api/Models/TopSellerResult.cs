namespace TopSellerReport.Api.Models
{
    public class TopSellerResult
    {
        public required string Month { get; set; }
        public required string Seller { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSales { get; set; }
    }
}
