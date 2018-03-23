using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.BLL.DTO
{
    public class AllInFilial
    {
        public AllInFilial()
        {
            Pesquisas = new Dictionary<UsuarioFilialPerfil, PesquisaCurriculo>();
            Interacoes = new Dictionary<UsuarioFilialPerfil, Usuario>();
            UsuariosFilial = new Dictionary<UsuarioFilialPerfil, UsuarioFilial>();
            Perfis = new UsuarioFilialPerfil[0];
        }
        public BLL.Filial Filial { get; set; }

        public BLL.Vaga UltimaVagaDados { get; set; }
        public BLL.TipoVinculo[] UltimaVagaVinculos { get; set; }

        public UsuarioFilialPerfil[] Perfis { get; set; }

        public Dictionary<UsuarioFilialPerfil, PesquisaCurriculo> Pesquisas { get; set; }
        public Dictionary<UsuarioFilialPerfil, Usuario> Interacoes { get; set; }
        public Dictionary<UsuarioFilialPerfil, UsuarioFilial> UsuariosFilial { get; set; }

        public bool? AceitaEstag { get; set; }
        public bool? PublicaVaga { get; set; }

        public bool Plano { get; set; }
        public DateTime PlanoInicio { get; set; }
        public DateTime PlanoFim { get; set; }

        public DateTime? UltimaPesquisa
        {
            get
            {
                if (!Pesquisas.Any(a => a.Value.DataCadastro.HasValue))
                    return null;

                return Pesquisas.Max(a => a.Value.DataCadastro.GetValueOrDefault());
            }
        }

        public DateTime? UltimaInteracao
        {
            get
            {
                if (!Interacoes.Any(a => a.Value.DataUltimaAtividade.HasValue))
                    return null;

                var max = Interacoes.Select(a => a.Value).Max(a => a.DataUltimaAtividade.GetValueOrDefault());
                return max;
            }
        }

    }
}
