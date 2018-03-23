using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business.Enumeradores
{
    public enum Perfil
    {
        [TipoPerfilAttribute(TipoPerfil.Interno)]
        AdministradorSistema = 1,
        [TipoPerfilAttribute(TipoPerfil.Candidato)]
        AcessoVIP = 2,
        [TipoPerfilAttribute(TipoPerfil.Candidato)]
        AcessoNaoVIP = 3,
        [TipoPerfilAttribute(TipoPerfil.Empresa)]
        AcessoEmpresaMaster = 4,
        [TipoPerfilAttribute(TipoPerfil.Empresa)]
        AcessoEmpresaCursos = 5,
        [TipoPerfilAttribute(TipoPerfil.Empresa)]
        AcessoEmpresaRecrutador = 6,
        [TipoPerfilAttribute(TipoPerfil.Empresa)]
        AcessoEmpresa = 9,
        [TipoPerfilAttribute(TipoPerfil.Interno)]
        Publicador = 10,
        [TipoPerfilAttribute(TipoPerfil.Interno)]
        Auditor = 11,
        [TipoPerfilAttribute(TipoPerfil.Interno)]
        Revisor = 12,
        [TipoPerfilAttribute(TipoPerfil.Interno)]
        Interno = 13,
        [TipoPerfilAttribute(TipoPerfil.Interno)]
        Financeiro = 15
    }
}
