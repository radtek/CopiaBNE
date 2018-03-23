using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Core.ExtensionsMethods
{
    public static class StringExtension
    {
        public static string GetHash(this string s)
        {
            SHA1 hasher = SHA1.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();

            byte[] array = encoding.GetBytes(s);
            array = hasher.ComputeHash(array);

            StringBuilder strHexa = new StringBuilder();

            foreach (byte item in array)
            {
                strHexa.Append(item.ToString("x2"));
            }

            return strHexa.ToString();
        }

        #region Criptografia
        /// <summary>
        /// Criptografa uma string de acordo com a chave inserida no banco
        /// </summary>
        /// <param name="txtCriptografa"></param>
        /// <returns></returns>
        public static string Criptografa(this string txtCriptografa, string secretKey)
        {
            byte[] utfData = UTF8Encoding.UTF8.GetBytes(txtCriptografa);

            byte[] saltBytes = Encoding.UTF8.GetBytes(secretKey);

            string encryptedString = string.Empty;

            using (AesManaged aes = new AesManaged())
            {

                Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(secretKey, saltBytes);

                aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;

                aes.KeySize = aes.LegalKeySizes[0].MaxSize;

                aes.Key = rfc.GetBytes(aes.KeySize / 8);

                aes.IV = rfc.GetBytes(aes.BlockSize / 8);

                using (ICryptoTransform encryptTransform = aes.CreateEncryptor())
                {

                    using (MemoryStream encryptedStream = new MemoryStream())
                    {

                        using (CryptoStream encryptor = new CryptoStream(encryptedStream, encryptTransform, CryptoStreamMode.Write))
                        {

                            encryptor.Write(utfData, 0, utfData.Length);

                            encryptor.Flush();

                            encryptor.Close();

                            byte[] encryptBytes = encryptedStream.ToArray();

                            encryptedString = Convert.ToBase64String(encryptBytes);

                        }

                    }

                }

            }

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(encryptedString);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        /// <summary>
        /// Descriptografa uma string de acordo com a chave inserida no banco
        /// </summary>
        /// <param name="txtDesCriptografa"></param>
        /// <returns></returns>
        public static string Descriptografa(this string txtDesCriptografa, string secretKey)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(txtDesCriptografa);
            txtDesCriptografa = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

            // string se = WebUtility.HtmlDecode();
            byte[] encryptedBytes = Convert.FromBase64String(txtDesCriptografa.Replace(" ", "+"));
            byte[] saltBytes = System.Text.Encoding.UTF8.GetBytes(secretKey);
            string decryptedString = string.Empty;

            using (var aes = new AesManaged())
            {

                Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(secretKey, saltBytes);

                aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;

                aes.KeySize = aes.LegalKeySizes[0].MaxSize;

                aes.Key = rfc.GetBytes(aes.KeySize / 8);

                aes.IV = rfc.GetBytes(aes.BlockSize / 8);

                using (ICryptoTransform decryptTransform = aes.CreateDecryptor())
                {

                    using (MemoryStream decryptedStream = new MemoryStream())
                    {

                        CryptoStream decryptor = new CryptoStream(decryptedStream, decryptTransform, CryptoStreamMode.Write);

                        decryptor.Write(encryptedBytes, 0, encryptedBytes.Length);

                        decryptor.Flush();

                        decryptor.Close();

                        byte[] decryptBytes = decryptedStream.ToArray();

                        decryptedString = UTF8Encoding.UTF8.GetString(decryptBytes, 0, decryptBytes.Length);

                    }

                }

            }
            return decryptedString;
        }

        #endregion
    }
}
