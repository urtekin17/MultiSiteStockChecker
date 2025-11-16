using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utilities
{
    public static class AesEncryption
    {
        public static string Encrypt(string plainText,string keyBase64)
        {
            if (string.IsNullOrEmpty(plainText) || string.IsNullOrEmpty(keyBase64))
                return string.Empty;
            var key = Convert.FromBase64String(keyBase64);
            using var aes = Aes.Create();
            aes.Key = key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.GenerateIV();
            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();

            ms.Write(aes.IV, 0, aes.IV.Length);
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs,Encoding.UTF8))
            {
                sw.Write(plainText);
            }
            var cipherWithIv = ms.ToArray();
            return Convert.ToBase64String(cipherWithIv);
        }

        public static string Decrypt(string cipherTextBase64,string keyBase64)
        {
            if (string.IsNullOrEmpty(cipherTextBase64) || string.IsNullOrEmpty(keyBase64))
                return string.Empty;
            var fullCipher = Convert.FromBase64String(cipherTextBase64);
            var key = Convert.FromBase64String(keyBase64);
            using var aes = Aes.Create();
            aes.Key = key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            var iv = new byte[aes.BlockSize / 8];
            Array.Copy(fullCipher, 0, iv, 0, iv.Length);
            var cipher = new byte[fullCipher.Length - iv.Length];
            Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);
            
            using var decryptor = aes.CreateDecryptor(aes.Key, iv);
            using var ms = new MemoryStream(cipher);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs, Encoding.UTF8);
            return sr.ReadToEnd();
        }

        public static string GenerateKeyBase64()
        {
            using var rng = RandomNumberGenerator.Create();
            var key = new byte[32]; // 256 bits
            rng.GetBytes(key);
            return Convert.ToBase64String(key);
        }
    }
}
