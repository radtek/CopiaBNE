using BNE.StorageManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using Microsoft.Data.Edm.Library.Expressions;

namespace BNE.NugetServer.Test
{
    [TestClass]
    public class StorageTest
    {
        Dictionary<string, string> arquivosTeste = new Dictionary<string, string>()
        {
            {"009.269.009-39.png", @"..\..\..\BNE.NugetServer.Test\TestFiles\009.269.009-39.png"},
            {"009.269.809-39.pdf", @"..\..\..\BNE.NugetServer.Test\TestFiles\009.269.809-39.pdf"},
            {"042.518.759-40.pdf", @"..\..\..\BNE.NugetServer.Test\TestFiles\042.518.759-40.pdf"},
            {"042.518.759-41.pdf", @"..\..\..\BNE.NugetServer.Test\TestFiles\042.518.759-41.pdf"},
            {"123.456.789-09.pdf", @"..\..\..\BNE.NugetServer.Test\TestFiles\123.456.789-09.pdf"},
            {"123.456.789-10.pdf", @"..\..\..\BNE.NugetServer.Test\TestFiles\123.456.789-10.pdf"}
        };

        [TestMethod]
        public void TestOSToAzure()
        {
            IFileManager os = StorageManager.StorageManager.GetFileManager("SoftlayerPrivado");
            IFileManager az = StorageManager.StorageManager.GetFileManager("AzurePrivado");

            var bytes2 = os.GetBytes("curriculos/#Curriculo Nathália Moraes-164.623.027-20.rtf");
            az.Save("curriculos/#Curriculo Nathália Moraes-164.623.027-20.rtf", bytes2);

            var total = os.Count();
            var done = 0;
            foreach (var item in os.List())
            {
                var bytes = os.GetBytes(item);
                az.Save(item, bytes);
                done++;
            }
        }

        [TestMethod]
        public void List()
        {
            IFileManager fm = StorageManager.StorageManager.GetFileManager("test");

            //foreach (var arquivo in arquivosTeste)
            //{
            //    fm.Save(arquivo.Key, File.ReadAllBytes(arquivo.Value));
            //}

            var l = fm.List();
        }

        [TestMethod]
        public void Save()
        {
            IFileManager fm = StorageManager.StorageManager.GetFileManager("privateTemp");
            IFileManager fmPub = StorageManager.StorageManager.GetFileManager("publicTemp");

            foreach (var arquivo in arquivosTeste)
            {
                fm.Save(arquivo.Key, File.ReadAllBytes(arquivo.Value));

                fmPub.Save(arquivo.Key, File.ReadAllBytes(arquivo.Value));
            }
        }

        [TestMethod]
        public void Move()
        {
            IFileManager fm = StorageManager.StorageManager.GetFileManager("privateTemp");

            foreach (var arquivo in arquivosTeste)
            {
                fm.Move(arquivo.Key, arquivo.Key.Replace(".png", "-moved.png").Replace(".pdf", "-moved.pdf"));
            }
        }

        [TestMethod]
        public void Exists()
        {
            IFileManager fm = StorageManager.StorageManager.GetFileManager("publicTemp");

            foreach (var arquivo in arquivosTeste)
            {
                if (!fm.Exists(arquivo.Key))
                    throw new Exception(arquivo.Key + ": Objecto deveria existir");

                if (fm.Exists(new Random().Next().ToString() + ".png"))
                    throw new Exception("Objecto não deveria existir");
            }
        }

        [TestMethod]
        public void ObjectPathEmpty_ShouldNotExist()
        {
            IFileManager fm = StorageManager.StorageManager.GetFileManager("SoftlayerPrivado");
            Assert.AreEqual(false, fm.ExistsAsync(string.Empty).Result);

            //foreach (var arquivo in arquivosTeste)
            //{
            //    if (!fm.Exists(arquivo.Key))
            //        throw new Exception(arquivo.Key + ": Objecto deveria existir");

            //    if (fm.Exists(new Random().Next().ToString() + ".png"))
            //        throw new Exception("Objecto não deveria existir");
            //}
        }

        [TestMethod]
        public void Delete()
        {
            IFileManager fm = StorageManager.StorageManager.GetFileManager("privateTemp");
            IFileManager pubfm = StorageManager.StorageManager.GetFileManager("publicTemp");

            foreach (var arquivo in arquivosTeste)
            {
                fm.Delete(arquivo.Key.Replace(".png", "-moved.png").Replace(".pdf", "-moved.pdf"));
                pubfm.Delete(arquivo.Key);
            }
        }

        [TestMethod]
        public void GetBytes()
        {
            IFileManager fm = StorageManager.StorageManager.GetFileManager("publicTemp");

            foreach (var arquivo in arquivosTeste)
                fm.GetBytes(arquivo.Key);
        }

        [TestMethod]
        public void TestConfiguration()
        {
            IFileManager fm = StorageManager.StorageManager.GetFileManager("publicTemp");
        }
    }
}
