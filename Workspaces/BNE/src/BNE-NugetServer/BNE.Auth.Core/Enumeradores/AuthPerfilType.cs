using System;

namespace BNE.Auth.Core.Enumeradores
{
    [Flags]
    public enum AuthPerfilType
    {
        NenhumOuDesconhecido = 0,
        Candidato = 1,
        VIP = 2,
        NaoVIP = 4,
        CandidatoVIP = Candidato | VIP,
        CandidatoNaoVIP = Candidato | NaoVIP,
        STC = 8,
        NaoSTC = 16,
        CandidatoSTC = Candidato | STC,
        CandidatoNaoSTC = Candidato | NaoSTC,
        UsuarioEmpresa = 32,
        UsuarioAdministrador = 64,
    }
}
