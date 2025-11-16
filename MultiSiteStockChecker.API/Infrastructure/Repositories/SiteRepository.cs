using Application.Interfaces;
using Core.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SiteRepository : ISiteRepository
    {
        private readonly AppDbContext _context;
        public SiteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddSiteAsync(Site site, CancellationToken cancellationToken = default)
        {
            await _context.Sites.AddAsync(site, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteSiteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Sites.FindAsync(new object?[] {id}, cancellationToken);
            if (entity != null)
            {
                _context.Sites.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<IEnumerable<Site>> GetAllSitesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Sites.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<Site?> GetSiteByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Sites.FindAsync(new object?[] {id}, cancellationToken);
        }

        public async Task UpdateSiteAsync(Site site, CancellationToken cancellationToken = default)
        {
            _context.Sites.Update(site);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
