using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Site
    {
        public int Id { get; set; }
        public string SiteName { get; set; } = string.Empty;
        public string LoginUrl { get; set; } = string.Empty;
        public string StockUrl { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? PasswordEncrypted { get; set; }
        [NotMapped]
        public string Password { get; set; } = string.Empty; // Prodda secret store kullan
        public string? ExtraParamJson { get; set; }
        public string ParserType { get; set; } = "Default";
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
