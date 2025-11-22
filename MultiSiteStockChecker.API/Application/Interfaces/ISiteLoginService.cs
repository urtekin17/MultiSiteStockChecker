using Core.Models;
using Microsoft.Playwright;

namespace Application.Interfaces
{
    public interface ISiteLoginService
    {
        Task<bool> LoginAsync(Site site);
        Task<bool> LoginAsync(Site site, IPage page);
    }
}
