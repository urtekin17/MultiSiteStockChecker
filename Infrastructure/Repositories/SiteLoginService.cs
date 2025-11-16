using Application.Interfaces;
using Core.Models;
using Infrastructure.Services;
using Infrastructure.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using System.Diagnostics;
using System.Text.Json;

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
            var selectors = JsonParser.ParseValues(site.ExtraParamJson);
            var stopwatch = Stopwatch.StartNew();
            var page = await _playwrightService.browser.NewPageAsync();
            try
            {
                await page.GotoAsync(site.LoginUrl, new() { WaitUntil = WaitUntilState.NetworkIdle });
                // Alanları bekle
                await page.WaitForSelectorAsync(selectors["userSelector"], new() { Timeout = 10000 });
                await page.WaitForSelectorAsync(selectors["passwordSelector"], new() { Timeout = 10000 });
                //Selector kontrolu
                var user = page.QuerySelectorAsync(selectors["userSelector"]);
                var pass = page.QuerySelectorAsync(selectors["passwordSelector"]);
                var sbmt = page.QuerySelectorAsync(selectors["submitSelector"]);
                // Doldur
                await page.FillAsync(selectors["userSelector"], site.UserName);
                await page.FillAsync(selectors["passwordSelector"], site.Password);
                // Butona bas
                await page.ClickAsync(selectors["submitSelector"]);
                // Yüklenme bekle
                await page.WaitForTimeoutAsync(2000);
                // Sayfa kapandıysa → login başarılıdır
                if (page.IsClosed)
                    return true;
                // fallback — password alanı kaybolmuş mu?
                return !await page.IsVisibleAsync(selectors["passwordSelector"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{site.SiteName}] Login error: {ex.Message}");
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
                catch { }

                _playwrightService.DecrementPageCount();
            }
        }

    }
}
