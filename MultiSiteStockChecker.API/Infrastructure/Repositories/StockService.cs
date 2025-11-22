using Application.Interfaces;
using Core.Models;
using Infrastructure.Services;
using Infrastructure.Utilities;
using Microsoft.Playwright;

namespace Infrastructure.Repositories
{
    public class StockService : IStockService
    {
        private readonly ISiteRepository _siteRepo;
        private readonly ISiteLoginService _loginService;
        private readonly PlaywrightService _pw;

        public StockService(
            ISiteRepository siteRepo,
            ISiteLoginService loginService,
            PlaywrightService pw)
        {
            _siteRepo = siteRepo;
            _loginService = loginService;
            _pw = pw;
        }
        
        public async Task<List<StockResult>> SearchForStock(string stockCode)
        {
            var sites = await _siteRepo.GetAllSitesAsync();
            var tasks = sites.Select(site => CheckSingleSiteAsync(site, stockCode));
            var results = await Task.WhenAll(tasks);
            return results.Where(r => r != null).ToList()!;
        }

        private async Task<StockResult?> CheckSingleSiteAsync(Site site, string stockNumber)
        {
            var page = await _pw.browser.NewPageAsync();

            var loggedIn = await _loginService.LoginAsync(site, page);
            if (!loggedIn) return new StockResult { SiteName = site.SiteName, Error = "Login Failed" };

            await page.GotoAsync(site.StockUrl);

            var html = await page.ContentAsync();
            var selectors = JsonParser.ParseValues(site.ExtraParamJson);
            Dictionary<string, string> parserDetails = new Dictionary<string, string>
            {
                { "StockUrl", site.StockUrl },
                { "StockInputSelector", selectors["inputSelector"] },
                { "SubmitButtonSelector", selectors["searchBtnSelector"] },
            };

            return await CheckStockAsync(site, page, stockNumber, parserDetails);
        }

        public async Task<StockResult> CheckStockAsync(Site site, IPage page, string stockCode, Dictionary<string,string> parserDetails)
        {
            await page.GotoAsync(site.StockUrl);

            await page.FillAsync("input[name='stokKodu']", stockCode);
            await page.ClickAsync("button[type='submit']");
            await page.WaitForSelectorAsync(".stokSonuc");
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await page.ContentAsync();
            string qtyText = await page.InnerTextAsync(".stokAdet");
            int qty = int.Parse(qtyText);

            return new StockResult
            {
                SiteId = site.Id,
                SiteName = site.SiteName,
                StockCode = stockCode,
                IsAvailable = qty > 0,
                Quantity = qty,
                Price = null,
                RawResponse = content,
                ParserType = site.ParserType,
                Error = ""
            };
        }
    }
}
