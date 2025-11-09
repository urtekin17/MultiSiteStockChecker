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
        private readonly ISiteLoginService _loginService;

        public SiteController(ISiteRepository repo, ISiteLoginService loginService)
        {
            _repo = repo;
            _loginService = loginService;
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

        [HttpGet("login/{id:int}")]
        public async Task<IActionResult> TestLogin(int id)
        {
            var site = await _repo.GetSiteByIdAsync(id);
            if (site == null)
            {
                return NotFound();
            }
            bool success = await _loginService.LoginAsync(site);
            return Ok( new { site.SiteName, LoginSuccess = success } );
        }
    }
}
