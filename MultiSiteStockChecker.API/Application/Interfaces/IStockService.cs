using Core.Models;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IStockService
    {
        Task<List<StockResult>> SearchForStock(string stockCode);
        Task<StockResult> CheckStockAsync(Site site, IPage page, string stockCode, Dictionary<string, string> parserDetails);
    }
}
