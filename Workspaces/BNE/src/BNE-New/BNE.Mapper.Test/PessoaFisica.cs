using System;
using System.Collections.Generic;
using BNE.Mapper.Models.Indicacao;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BNE.Mapper.Test
{
    [TestClass]
    public class PessoaFisica
    {
        [TestMethod]
        public void TestIndicarAmigo()
        {
            var pessoaFisicaMapper = new BNE.Mapper.ToOld.PessoaFisica();

            var r = pessoaFisicaMapper.IndicarAmigos(new Indicacao
            {
                CPF = "4914896923",
                IdCurriculo = null,
                IdVaga = 125352,
                listaAmigos = new List<AmigoIndicado> {new AmigoIndicado
                {
                    Email="grstelmak@gmail.com",
                    Nome = "Gieyson"
                }}
            });

            Assert.AreEqual(true, r);
        }
    }
}
