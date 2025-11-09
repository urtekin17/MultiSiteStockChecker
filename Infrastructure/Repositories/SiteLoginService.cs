using Application.Interfaces;
using Core.Models;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SiteLoginService : ISiteLoginService
    {
        public async Task<bool> LoginAsync(Site site)
        {
            using var playwright = Playwright.CreateAsync();
            await using var browser  = await playwright.Result.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });
            var page = await browser.NewPageAsync();

            try
            {
                await page.GotoAsync(site.LoginUrl);
                await page.FillAsync("input[name='username']", site.UserName);
                await page.FillAsync("input[name='password']", site.Password);
                await page.ClickAsync("button[type='submit']");
                await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                // Giriş başarılı mı kontrol et
                bool isLoggedIn = !await page.IsVisibleAsync("input[name='password']");
                return isLoggedIn;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error for {site.SiteName}: {ex.Message}");
                throw;
            }


        }
    }
}
