using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using BNE.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BNE.NugetServer.Test
{
    [TestClass]
    public class UnitTest1
    {
        private static readonly ICryptography Crypto = new AES();


        [TestMethod]
        public void TestMethod1()
        {
            var d = Crypto.Decrypt("mduughtv9Xl+D2R/J15J3ma225eEFcVHd4LPgIqzWPE7M8YgxybeXsebaIRFKSOwgkwEvxLFB4UGgLgYM/8LDA==");
            var v = Crypto.Version;
            var cardList = new List<string>
            {
                "5256630225795198", "180", "235", "12", "20", "32"

            };

            var cardListEncrypted = new List<string>();

            foreach (var cardNumber in cardList)
            {
                cardListEncrypted.Add(Crypto.Encrypt(cardNumber));
            }

            var cardListSize = new List<string>();

            foreach (var cardEncrypted in cardListEncrypted)
            {
                cardListSize.Add(cardEncrypted.Length.ToString());
            }
            //var cardNumber = "5256630225795198";

            //var encrypted1 = aes.Encrypt(cardNumber);
            
            //var dencrypted1 = aes.Decrypt(encrypted1);


        }
    }
}
