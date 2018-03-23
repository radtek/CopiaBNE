using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BNE.BLL.Test
{
    [TestClass]
    public class HelperTest
    {
        [TestMethod]
        public void CalcularIdade()
        {
            Assert.AreEqual(BNE.BLL.Custom.Helper.CalcularIdade(new DateTime(1986,3,24)), 29);
            Assert.AreEqual(BNE.BLL.Custom.Helper.CalcularIdade(new DateTime(1985, 4, 24)), 29);
            Assert.AreEqual(BNE.BLL.Custom.Helper.CalcularIdade(new DateTime(1985, 4, 23)), 30);
            Assert.AreNotEqual(BNE.BLL.Custom.Helper.CalcularIdade(new DateTime(1985, 4, 24)), 30);
            Assert.AreEqual(BNE.BLL.Custom.Helper.CalcularIdade(new DateTime(1985, 4, 24)), 29);
            Assert.AreEqual(BNE.BLL.Custom.Helper.CalcularIdade(new DateTime(1992, 2, 26)), 23);
            Assert.AreEqual(BNE.BLL.Custom.Helper.CalcularIdade(DateTime.Today), 0);
        }
    }
}
