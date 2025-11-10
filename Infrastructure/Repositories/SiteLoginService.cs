using Application.Interfaces;
using Core.Models;
using Infrastructure.Services;
using Microsoft.Playwright;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SiteLoginService : ISiteLoginService
    {
        private readonly PlaywrightService _playwrightService;

        public SiteLoginService(PlaywrightService playwrightService)
        {
            _playwrightService = playwrightService;
        }
        public async Task<bool> LoginAsync(Site site)
        {
            await _playwrightService.InitializeAsync();
            _playwrightService.IncrementPageCount();

            var stopwatch = Stopwatch.StartNew();
            
            var page = await _playwrightService.browser.NewPageAsync();

            try
            {
                await page.GotoAsync(site.LoginUrl, new() { WaitUntil = WaitUntilState.NetworkIdle });
                await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
                await page.WaitForSelectorAsync("input[name='usercode']", new() { Timeout = 5000 });
                await page.WaitForSelectorAsync("input[name='userpassword']", new() { Timeout = 5000 });
                await page.FillAsync("input[name='usercode']", site.UserName);
                await page.FillAsync("input[name='userpassword']", site.Password);
                await page.ClickAsync("input[value='Giriş Yap']");
                await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                // Giriş başarılı mı kontrol et
                bool isLoggedIn = !await page.IsVisibleAsync("input[name='userpassword']");
                return isLoggedIn;
            }
            catch (TargetClosedException tex)
            {
                Console.WriteLine($"[{site.SiteName}] Page closed unexpectedly: {tex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error for {site.SiteName}: {ex.Message}");
                return false;
            }
            finally
            {
                stopwatch.Stop();
                _playwrightService.RecordLogin(stopwatch.ElapsedMilliseconds);
                try
                {
                    if (!page.IsClosed) await page.CloseAsync();
                }
                catch (Exception)
                {
                }
                
                _playwrightService.DecrementPageCount();
            }

        }
    }
}
