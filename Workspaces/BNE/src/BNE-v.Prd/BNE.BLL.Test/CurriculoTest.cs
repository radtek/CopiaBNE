using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BNE.BLL.Test
{
    [TestClass]
    public class CurriculoTest
    {

        [TestMethod]
        public void IntegracaoSine()
        {
            var curriculo = BNE.BLL.Curriculo.LoadObject(1338011);
            curriculo.CompletarCurriculoIntegracao(new Sine.Integracao.Model.Curriculo());
        }
    }
}
