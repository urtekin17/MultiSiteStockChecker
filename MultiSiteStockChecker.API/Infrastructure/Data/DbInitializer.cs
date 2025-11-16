using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(AppDbContext context, CancellationToken cancellationToken = default)
        {
            await context.Database.MigrateAsync(cancellationToken);

            if (!await context.Sites.AnyAsync(cancellationToken))
            {
                context.Sites.Add(new Site
                {
                    SiteName = "Example Site",
                    LoginUrl = "https://example.com/login",
                    StockUrl = "https://example.com/stock",
                    UserName = "admin",
                    Password = "password", // In production, use a secure secret store
                    ParserType = "Default",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                });



                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
