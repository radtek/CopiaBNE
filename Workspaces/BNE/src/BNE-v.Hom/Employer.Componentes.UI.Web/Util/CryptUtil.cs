using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Employer.Componentes.UI.Web.Util
{
    public class CryptUtil
    {
        private static Byte[] ConvertStringToByArray(string s)
        {
            return (new UnicodeEncoding()).GetBytes(s);
        }

        public static string MD5(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }

            Byte[] toHash = ConvertStringToByArray(s);
            byte[] hashValue = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(toHash);
            return BitConverter.ToString(hashValue);
        }

        public static string Base64Encode(string key)
        {
            if (string.IsNullOrEmpty(key))

                return string.Empty;

            byte[] buffer = Encoding.UTF8.GetBytes(key);
            return Convert.ToBase64String(buffer);
        }

        public static string Base64Decode(string key)
        {
            if (string.IsNullOrEmpty(key))

                return "";

            byte[] buffer = Convert.FromBase64String(key);

            return Encoding.UTF8.GetString(buffer);
        }

        // Arbitrary key and iv vector.
        // You will want to generate (and protect) your own when using encryption.
        private const string actionKey = "EA81AA1D5FC1EC53E84F30AA746139EEBAFF8A9B76638895";
        private const string actionIv = "87AF7EA221F3FFF5";

        private TripleDESCryptoServiceProvider des3;

        public CryptUtil()
        {
            des3 = new TripleDESCryptoServiceProvider();
            des3.Mode = CipherMode.CBC;
        }

        public string GenerateKey()
        {
            des3.GenerateKey();
            return BytesToHex(des3.Key);
        }

        public string GenerateIV()
        {
            des3.GenerateIV();
            return BytesToHex(des3.IV);
        }

        private byte[] HexToBytes(string hex)
        {
            byte[] bytes = new byte[hex.Length / 2];

            for (int i = 0; i < hex.Length / 2; i++)
            {
                string code = hex.Substring(i * 2, 2);
                bytes[i] = byte.Parse(code, System.Globalization.NumberStyles.HexNumber);
            }

            return bytes;
        }

        private string BytesToHex(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
                hex.AppendFormat("{0:X2}", bytes[i]);

            return hex.ToString();
        }

        public string Encrypt(string data, string key, string iv)
        {
            byte[] bdata = Encoding.UTF8.GetBytes(data);
            byte[] bkey = HexToBytes(key);
            byte[] biv = HexToBytes(iv);

            using (MemoryStream stream = new MemoryStream())
            using (CryptoStream encStream = new CryptoStream(stream,
             des3.CreateEncryptor(bkey, biv), CryptoStreamMode.Write))
            {
                encStream.Write(bdata, 0, bdata.Length);
                encStream.FlushFinalBlock();

                return BytesToHex(stream.ToArray());
            }
        }

        public string Decrypt(string data, string key, string iv)
        {
            byte[] bdata = HexToBytes(data);
            byte[] bkey = HexToBytes(key);
            byte[] biv = HexToBytes(iv);

            using (MemoryStream stream = new MemoryStream())
            using (CryptoStream encStream = new CryptoStream(stream,
                des3.CreateDecryptor(bkey, biv), CryptoStreamMode.Write))
            {
                encStream.Write(bdata, 0, bdata.Length);
                encStream.FlushFinalBlock();

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        public string ActionEncrypt(string data)
        {
            return Encrypt(data, actionKey, actionIv);
        }

        public string ActionDecrypt(string data)
        {
            return Decrypt(data, actionKey, actionIv);
        }
    }
}
