using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class StockResult
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; } = string.Empty;

        // Sorguladığın stok numarası (Kullanıcıdan gelecek)
        public string StockCode { get; set; } = string.Empty;

        // Sonuçlar
        public bool IsAvailable { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }

        // Siteden dönen orijinal text / json
        public string RawResponse { get; set; } = string.Empty;

        // ParserType ile eşleşmesi için
        public string ParserType { get; set; } = string.Empty;
        public string  Error { get; set; }
    }
}
