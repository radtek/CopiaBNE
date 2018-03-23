using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Text;

namespace BNE.BLL.Enumeradores
{
    public enum VagaLog
    {
        ImportarVagas = 1,
        IntegracaoVaga = 2,
        ArquivarVaga = 3,
        EmpresaAuditadaAtivaTodasVagasDaEmpresaDisparaRastreador = 4,
        MigrarVagaRapida = 5,
        PluginPublicacaoVaga = 6,
        PluginPublicacaoVaga_ValidarTermosVaga = 7,
        LimpaCampos = 8,
        ServicesAPIBusiness = 9,
        PluginRemoverVagaEmpresaBloqueada = 10

    }
}
