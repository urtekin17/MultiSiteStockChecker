using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    using Core.Models;
    public interface ISiteRepository
    {
        Task<IEnumerable<Site>> GetAllSitesAsync(CancellationToken cancellationToken = default);
        Task<Site?> GetSiteByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddSiteAsync(Site site, CancellationToken cancellationToken = default);
        Task UpdateSiteAsync(Site site, CancellationToken cancellationToken = default);
        Task DeleteSiteAsync(int id, CancellationToken cancellationToken = default);
    }
}
