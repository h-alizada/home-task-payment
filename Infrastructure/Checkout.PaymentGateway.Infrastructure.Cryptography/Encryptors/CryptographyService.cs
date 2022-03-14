using Checkout.PaymentGateway.Infrastructure.Core.Encryptors;
using Checkout.PaymentGateway.Infrastructure.Cryptography.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Checkout.PaymentGateway.Infrastructure.Cryptography.Encryptors
{
	public class CryptographyService : ICryptographyService
    {
        private readonly ICryptographyConfiguration _configuration;
        public CryptographyService(ICryptographyConfiguration configuration)
        {
            _configuration = configuration;

            if (string.IsNullOrEmpty(_configuration.Key))
            {
                throw new ArgumentNullException("key cannot be empty");
            }

            if (string.IsNullOrEmpty(_configuration.Vector))
            {
                throw new ArgumentNullException("vector cannot be empty");
            }
        }

        public string Encrypt(string plainText)
        {
            // Check arguments.
            if (string.IsNullOrEmpty(plainText))
            {
                throw new ArgumentNullException("plainText cannot be empty");
            }

            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(_configuration.Key);
                aesAlg.IV = Encoding.UTF8.GetBytes(_configuration.Vector);

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string cipherText)
        {
            // Check arguments.
            if (string.IsNullOrEmpty(cipherText))
            {
                throw new ArgumentNullException("cipherText cannot be empty");
            }

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(_configuration.Key);
                aesAlg.IV = Encoding.UTF8.GetBytes(_configuration.Vector);

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}
