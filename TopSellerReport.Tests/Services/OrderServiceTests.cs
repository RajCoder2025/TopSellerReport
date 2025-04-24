using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Globalization;
using TopSellerReport.Api.Services;

namespace TopSellerReport.Tests.Services
{
    [TestClass]
    public class OrderServiceTests
    {
        private OrderService _orderService;
        //private readonly string _csvPath = csvPath ?? throw new ArgumentNullException(nameof(csvPath));

        [TestInitialize]
        public void Setup()
        {
            // Use a mock file path or a test-specific path here
           var path = Path.Combine(AppContext.BaseDirectory, "mock_data.csv");
            _orderService = new OrderService(path);
        }

        [TestMethod]
        public void TestGetBranches()
        {
            // Mocking data or using a real small test file
            var branches = _orderService.GetBranches();
            Assert.IsNotNull(branches);
            Assert.IsTrue(branches.Count > 0);
        }

        [TestMethod]
        public void TestGetTopSellersByMonth()
        {
            var sellers = _orderService.GetTopSellersByMonth("Branch 1");
            Assert.IsNotNull(sellers);
            Assert.IsTrue(sellers.Count > 0);
        }

        [TestMethod]
        public void TestGetBranches_ReturnsDistinct()
        {
            var branches = _orderService.GetBranches();
            var distinctBranches = branches.Distinct().ToList();

            Assert.AreEqual(distinctBranches.Count, branches.Count);
        }

        [TestMethod]
        public void TestTopSellers_AreSortedByMonth()
        {
            var result = _orderService.GetTopSellersByMonth("Branch A");

            for (int i = 1; i < result.Count; i++)
            {
                var currentMonth = DateTime.ParseExact(result[i].Month, "MMMM", CultureInfo.CurrentCulture).Month;
                var prevMonth = DateTime.ParseExact(result[i - 1].Month, "MMMM", CultureInfo.CurrentCulture).Month;
                Assert.IsTrue(currentMonth >= prevMonth);
            }
        }
    }
}
