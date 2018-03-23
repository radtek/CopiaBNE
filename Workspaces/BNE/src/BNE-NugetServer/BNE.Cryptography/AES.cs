using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace BNE.Cryptography
{
    public class AES : ICryptography
    {

        private static readonly ICryptography Instance = new AES();
        private static Key _key;
        private const int SaltSize = 32;

        #region Version
        public string Version
        {
            get
            {
                return _key.Version;
            }
        }
        #endregion

        #region GetInstance
        public static AES GetInstance()
        {
            return (AES)Instance;
        }
        #endregion

        #region Constructor
        public AES()
        {
            var path = ConfigurationManager.AppSettings["KeyPath"];
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("KeyPath");

            _key = new Key { Value = File.ReadAllText(path), Version = "0.1" };
        }
        #endregion

        #region Encrypt
        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");
            if (_key == null)
                throw new ArgumentNullException("key");
            if (string.IsNullOrEmpty(_key.Value))
                throw new ArgumentNullException("key");

            // Derive a new Salt and IV from the Key
            using (var keyDerivationFunction = new Rfc2898DeriveBytes(_key.Value, SaltSize))
            {
                var saltBytes = keyDerivationFunction.Salt;
                var keyBytes = keyDerivationFunction.GetBytes(32);
                var ivBytes = keyDerivationFunction.GetBytes(16);

                using (var aesManaged = new AesManaged())
                using (var encryptor = aesManaged.CreateEncryptor(keyBytes, ivBytes))
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    using (var streamWriter = new StreamWriter(cryptoStream))
                    {
                        // Send the data through the StreamWriter, through the CryptoStream, to the underlying MemoryStream
                        streamWriter.Write(plainText);
                    }

                    // Return the encrypted bytes from the memory stream, in Base64 form so we can send it right to a database (if we want).
                    var cipherTextBytes = memoryStream.ToArray();

                    Array.Resize(ref saltBytes, saltBytes.Length + cipherTextBytes.Length);
                    Array.Copy(cipherTextBytes, 0, saltBytes, SaltSize, cipherTextBytes.Length);

                    return Convert.ToBase64String(saltBytes);
                }
            }
        }
        #endregion

        #region Decrypt
        public string Decrypt(string ciphertext)
        {
            if (string.IsNullOrEmpty(ciphertext))
                throw new ArgumentNullException("cipherText");
            if (_key == null)
                throw new ArgumentNullException("key");
            if (string.IsNullOrEmpty(_key.Value))
                throw new ArgumentNullException("key");

            // Extract the salt from our ciphertext
            var allTheBytes = Convert.FromBase64String(ciphertext);
            var saltBytes = allTheBytes.Take(SaltSize).ToArray();
            var ciphertextBytes = allTheBytes.Skip(SaltSize).Take(allTheBytes.Length - SaltSize).ToArray();

            using (var keyDerivationFunction = new Rfc2898DeriveBytes(_key.Value, saltBytes))
            {
                // Derive the previous IV from the Key and Salt
                var keyBytes = keyDerivationFunction.GetBytes(32);
                var ivBytes = keyDerivationFunction.GetBytes(16);

                // Create a decrytor to perform the stream transform.
                // Create the streams used for decryption.
                // The default Cipher Mode is CBC and the Padding is PKCS7 which are both good
                using (var aesManaged = new AesManaged())
                using (var decryptor = aesManaged.CreateDecryptor(keyBytes, ivBytes))
                using (var memoryStream = new MemoryStream(ciphertextBytes))
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                using (var streamReader = new StreamReader(cryptoStream))
                {
                    // Return the decrypted bytes from the decrypting stream.
                    return streamReader.ReadToEnd();
                }
            }
        }
        #endregion

    }
}
