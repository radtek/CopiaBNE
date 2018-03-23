using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BNE.BLL.Test
{
    [TestClass]
    public class VagaTest
    {

        [TestMethod]
        public void CandidaturaAutomatica_TemCandidatosNoPerfil_DeveMandarEmailComOsCandidatos()
        {
            var vaga = Vaga.LoadObject(1465976);
            vaga.FinalizarPublicacaoNovaVaga();
        }

        [TestMethod]
        public void CandidaturaAutomatica_NaoTemCandidatosNoPerfil_DeveMandarEmailSemOsCandidatos()
        {
            var vaga = Vaga.LoadObject(1465972);
            vaga.FinalizarPublicacaoNovaVaga();
        }

        [TestMethod]
        public void CandidaturaAutomatica_NaoTemCandidatosNoPerfil_DeveMandarEmailSemOsCandidatos2()
        {
            var vaga = Vaga.LoadObject(1465971);
            vaga.FinalizarPublicacaoNovaVaga();
        }

        [TestMethod]
        public void CandidaturaAutomatica_VagaRapida_NaoTemCandidatosNoPerfil_DeveMandarEmailSemOsCandidatos()
        {
            var vaga = Vaga.LoadObject(1465885);
            vaga.FinalizarPublicacaoNovaVaga();
        }



        [TestMethod]
        public void CandidaturaAutomatica_VagaRapida_NaoTemCandidatosNoPerfil_DeveMandarEmailSemOsCandidatos_TesteSony()
        {
            var vagas = new List<int> { 1465994, 1465993, 1465995 };

            foreach (var vaga in vagas)
            {
                var o = Vaga.LoadObject(vaga);
                o.FinalizarPublicacaoNovaVaga();
            }
        }


    }
}
