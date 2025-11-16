using Infrastructure.Utilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IEncryptionService
    {
        string Encrypt(string plain);
        string Decrypt(string cipher);
        string KeyBase64 { get; }
    }
    public class EncryptionService : IEncryptionService
    {
        private readonly string _keyBase64;
        public EncryptionService(IConfiguration configuration)
        {
            _keyBase64 = configuration["Encryption:KeyBase64"]
                         ?? throw new InvalidOperationException("Encryption key not configured.");
        }
        public string KeyBase64 => _keyBase64;
        public string Decrypt(string cipher) => AesEncryption.Decrypt(cipher ?? string.Empty, _keyBase64);
        public string Encrypt(string plain) => AesEncryption.Encrypt(plain ?? string.Empty, _keyBase64);
    }
}
