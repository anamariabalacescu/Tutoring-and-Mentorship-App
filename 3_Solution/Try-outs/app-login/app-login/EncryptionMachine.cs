using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Input;
using System.Security.Cryptography;

namespace app_login
{
    internal class EncryptionMachine
    {
        public static string Encrypt(string text)
        {
            string key = "AppBD2023!";

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key.PadRight(16)); // Padding key if its length is less than 16 bytes
                aesAlg.Mode = CipherMode.CBC;

                // Generate a random IV
                byte[] iv = new byte[aesAlg.BlockSize / 8];
                using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(iv);
                }

                // Encrypt using AES-CBC mode
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, iv);
                byte[] encryptedBytes = null;
                using (var msEncrypt = new System.IO.MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }
                    }
                    encryptedBytes = msEncrypt.ToArray();
                }

                // Combine IV and encrypted data
                byte[] combinedBytes = new byte[iv.Length + encryptedBytes.Length];
                Buffer.BlockCopy(iv, 0, combinedBytes, 0, iv.Length);
                Buffer.BlockCopy(encryptedBytes, 0, combinedBytes, iv.Length, encryptedBytes.Length);

                // Convert to base64 for string representation
                string cipherText = Convert.ToBase64String(combinedBytes);
                return cipherText;
            }
        }

        public static string Decrypt(string cipherText)
        {
            string key = "AppBD2023!";

            byte[] combinedBytes = Convert.FromBase64String(cipherText);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key.PadRight(16));
                aesAlg.Mode = CipherMode.CBC;

                // Extract IV from combined bytes
                byte[] iv = new byte[aesAlg.BlockSize / 8];
                Buffer.BlockCopy(combinedBytes, 0, iv, 0, iv.Length);

                // Extract encrypted data from combined bytes
                byte[] encryptedBytes = new byte[combinedBytes.Length - iv.Length];
                Buffer.BlockCopy(combinedBytes, iv.Length, encryptedBytes, 0, encryptedBytes.Length);

                // Decrypt using AES-CBC mode
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, iv);
                string decryptedText = null;
                using (var msDecrypt = new System.IO.MemoryStream(encryptedBytes))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                        {
                            decryptedText = srDecrypt.ReadToEnd();
                        }
                    }
                }

                return decryptedText;
            }
        }
        public static string Base64Encode(string plainText)
        {
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64Encoded)
        {
            byte[] base64EncodedBytes = Convert.FromBase64String(base64Encoded);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

    }
}
