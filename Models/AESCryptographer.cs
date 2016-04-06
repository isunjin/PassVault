using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace PassVault.Models
{
    /// <summary> encrypt/decrypt by AES</summary>
    class AESCryptographer 
    {
        /// <summary/>
        private byte[] CryptoTransform(string password, byte[] data, byte[] salt, bool encrypt)
        {
            using (AesManaged aes = new AesManaged())
            {
                Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(password, salt);

                aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
                aes.KeySize = aes.LegalKeySizes[0].MaxSize;
                aes.Key = rfc.GetBytes(aes.KeySize / 8);
                aes.IV = rfc.GetBytes(aes.BlockSize / 8);

                ICryptoTransform encryptTransform = encrypt ? aes.CreateEncryptor() : aes.CreateDecryptor();
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptTransform, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(data, 0, data.Length);
                    }
                    return memoryStream.ToArray();
                }
            }
        }

        /// <summary/>
        public byte[] Encrypt(string password, byte[] data, byte[] salt)
        {
            return this.CryptoTransform(password, data, salt, true);
        }

        /// <summary/>
        public byte[] Decrypt(string password, byte[] data, byte[] salt)
        {
            return this.CryptoTransform(password, data, salt, false);
        }
    }
}
