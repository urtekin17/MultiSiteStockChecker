using Application.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SiteController : ControllerBase
    {
        private readonly ISiteRepository _repo;

        public SiteController(ISiteRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSites()
        {
            var sites = await _repo.GetAllSitesAsync();
            return Ok(sites);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSiteById(int id)
        {
            var site = await _repo.GetSiteByIdAsync(id);
            if (site == null)
            {
                return NotFound();
            }
            return Ok(site);
        }

        [HttpPost]
        public async Task<IActionResult> AddSite([FromBody] Site site)
        {
            await _repo.AddSiteAsync(site);
            return CreatedAtAction(nameof(GetSiteById), new { id = site.Id }, site);
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateSite([FromBody] Site site)
        {
            await _repo.UpdateSiteAsync(site);
            return Ok(site);
        }
    }
}
