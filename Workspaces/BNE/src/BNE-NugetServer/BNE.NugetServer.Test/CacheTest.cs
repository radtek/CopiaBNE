using System;
using System.Diagnostics;
using System.Threading;
using BNE.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BNE.NugetServer.Test
{
    [TestClass]
    public class CacheTest
    {
        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        [TestMethod]
        public void TestCache()
        {
            double SlidingExpiration = 0.5;
            //int SlidingExpiration = 10;

            while (true)
            {
                //var obj = Cache.GetItem("Data", () => new Obj(), SlidingExpiration);
                var obj = Cache.GetItem("Data", () => new Obj(), SlidingExpiration);
                Debug.WriteLine(obj.data);
                Thread.Sleep(1000);
            }
        }

        public class Obj
        {
            public DateTime data { get; set; }

            public Obj()
            {
                data = DateTime.Now;
            }
        }
    }
}
