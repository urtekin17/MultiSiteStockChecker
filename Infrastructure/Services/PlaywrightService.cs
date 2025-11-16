using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class PlaywrightService : IAsyncDisposable
    {
        public IPlaywright playwright {  get; private set; }
        public IBrowser browser { get; private set; }

        private bool _initialized = false;

        private readonly object _lock = new object();
        private int _activePages = 0;
        private int _totalLoginCount = 0;
        private readonly List<double> _loginDurations = new();

        public async Task InitializeAsync()
        {
            if (_initialized) return;
            playwright = await Playwright.CreateAsync();
            browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true,
            });
            _initialized = true;
        }
        public void RecordLogin(double durationMs)
        {
            lock (_lock) 
            {
                _totalLoginCount++;
                _loginDurations.Add(durationMs);
                if (_loginDurations.Count>100)
                {
                    _loginDurations.RemoveAt(0);
                }
            }

        }

        public void IncrementPageCount() => Interlocked.Increment(ref _activePages);
        public void DecrementPageCount() => Interlocked.Decrement(ref _activePages);

        public int GetActivePageCount() => _activePages;
        public int GetTotalLoginCount() => _totalLoginCount;

        public double GetAverageLoginDuration()
        {
            lock (_lock)
            {
                if (_loginDurations.Count == 0) return 0;
                return _loginDurations.Average();
            }
        }
        public async ValueTask DisposeAsync()
        {
            if (browser != null) await browser.CloseAsync();
            playwright?.Dispose();
        }
    }
}
