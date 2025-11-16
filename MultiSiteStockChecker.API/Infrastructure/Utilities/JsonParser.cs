using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Utilities
{
    public static class JsonParser
    {
        /// <summary>
        /// JSON içindeki string değerleri list halinde döndürür.
        /// Eğer keysOrder sağlanırsa, belirtilen sıraya göre değerleri döndürür (bulunmayan anahtar atlanır).
        /// </summary>
        public static Dictionary<string, string> ParseValues(string json, string[]? keysOrder = null)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new Dictionary<string, string>();

            try
            {
                var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new Dictionary<string, string>();

                return dict;
            }
            catch (JsonException)
            {
                // Geçersiz JSON -> boş liste döndür
                return new Dictionary<string, string>();
            }
        }
    }
}
