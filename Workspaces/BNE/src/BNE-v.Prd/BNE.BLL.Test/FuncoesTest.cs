using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.BLL.Test
{
    [TestClass]
    public class FuncoesTest
    {
        [TestMethod]
        public void TesteBuscaFuncoes()
        {
            BLL.Custom.Vaga.IntegracaoVaga objIntegracao = new Custom.Vaga.IntegracaoVaga();
            var funcao = objIntegracao.TratarFuncaoVaga("Vendedores");
        }

        [TestMethod]
        public void TesteBuffer()
        {
            BLL.Custom.Solr.Buffer.BufferAtualizacaoCurriculo.Update(new Curriculo(12));
            BLL.Custom.Solr.Buffer.BufferAtualizacaoCurriculo.Update(new Curriculo(13));
            BLL.Custom.Solr.Buffer.BufferAtualizacaoSMSCurriculo.Update(new Curriculo(15));
            BLL.Custom.Solr.Buffer.BufferAtualizacaoCurriculo.Update(new Curriculo(12));
        }

    }
}
