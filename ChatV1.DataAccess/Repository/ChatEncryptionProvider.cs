using Microsoft.EntityFrameworkCore.DataEncryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.DataAccess.Repository
{
    internal class ChatEncryptionProvider : IEncryptionProvider
    {
        byte[] key = Convert.FromBase64String("DXmR17m90v7HfAfVn4wvf1DrrcZUwSPfn/dKu1nMOvQ=");
        byte[] iv = Convert.FromBase64String("o/yx2kgEKiqNkGVxtAJZRQ==");

        public byte[] Decrypt(byte[] input)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                byte[] decryptedBytes;
                using (var msDecrypt = new System.IO.MemoryStream(input))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var msPlain = new System.IO.MemoryStream())
                        {
                            csDecrypt.CopyTo(msPlain);
                            decryptedBytes = msPlain.ToArray();
                        }
                    }
                }
                return decryptedBytes;
            }
        }


        public byte[] Encrypt(byte[] input)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                byte[] encryptedBytes;
                using (var msEncrypt = new System.IO.MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        //byte[] plainBytes = Encoding.UTF8.GetBytes(plaintext);
                        csEncrypt.Write(input, 0, input.Length);
                    }
                    encryptedBytes = msEncrypt.ToArray();
                }
                return encryptedBytes;
            }
        }
    }
}
