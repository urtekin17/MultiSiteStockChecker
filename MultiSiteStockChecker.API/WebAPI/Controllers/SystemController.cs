using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemController: ControllerBase
    {
        private readonly PlaywrightService _playwrightService;
        private readonly AppDbContext _dbContext;

        public SystemController(PlaywrightService playwrightService, AppDbContext dbContext)
        {
            _playwrightService = playwrightService;
            _dbContext = dbContext;
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetStatus()
        {
            bool playwrightReady = _playwrightService != null;
            bool browserReady = _playwrightService?.browser != null;

            bool dbReady = false;

            try
            {
                dbReady = await _dbContext.Database.CanConnectAsync();
            }
            catch
            {
                dbReady = false;
            }

            double? avgLoginDuration = _playwrightService?.GetAverageLoginDuration();
            double? avgLoginTimeMs = avgLoginDuration.HasValue
                ? Math.Round(avgLoginDuration.Value, 2)
                : (double?)null;

            var status = new
            {
                TimeStamp = DateTime.Now,
                PlaywrightReady = playwrightReady,
                BrowserReady = browserReady,
                DatabaseReady = dbReady,
                ActivePageCount = _playwrightService?.GetActivePageCount(),
                TotalLogins = _playwrightService?.GetTotalLoginCount(),
                AvgLoginTimeMs = avgLoginTimeMs
            };

            return Ok(status);
        }
    }
}
