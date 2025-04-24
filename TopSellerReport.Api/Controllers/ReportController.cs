using Microsoft.AspNetCore.Mvc;
//using TopSellerReport.Api.Services;
using TopSellerReport.Api.Services;

namespace TopSellerReport.Api.Controllers
{
    [ApiController]
    [Route("api/report")]
    public class ReportController : ControllerBase
    {
        private readonly OrderService _orderService;

        public ReportController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("pong");
        }

        [HttpGet("branches")]
        public IActionResult GetBranches()
        {
            var branches = _orderService.GetBranches();
            return Ok(branches);
        }

        [HttpGet("top-sellers")]
        public IActionResult GetTopSellers([FromQuery] string branch)
        {
            if (string.IsNullOrWhiteSpace(branch))
                return BadRequest("Branch is required");

            var result = _orderService.GetTopSellersByMonth(branch);
            return Ok(result);
        }

    }
}
