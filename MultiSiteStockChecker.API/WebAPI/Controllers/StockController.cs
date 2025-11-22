using Application.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockCheckService;
        public StockController(IStockService stockCheckService)
        {
            _stockCheckService = stockCheckService;
        }
        [HttpGet("checkStock")]
        public async Task<IActionResult> CheckStocks([FromQuery] string stockCode)
        {
            var results = await _stockCheckService.SearchForStock(stockCode);
            return Ok(results);
        }
    }
}
