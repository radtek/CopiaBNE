using System;

namespace BNE.Global.Model
{
    public class FuncaoSinonimo
    {
        public int Id { get; set; }
        public int? IdSinonimoSubstituto { get; set; }
        public string NomeSinonimo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string CodigoCBO { get; set; }
        public string DescricaoPesquisa { get; set; }        
        public string DescricaoJob { get; set; }
        public string Atribuicoes { get; set; }
        public string Responsabilidades { get; set; }
        public string Beneficio { get; set; }
        public bool FlgInativo { get; set; }
        public bool FlgAuditada { get; set; }

        public virtual TipoFuncaoGlobal TipoFuncaoGlobal { get; set; }
        public virtual EscolaridadeGlobal EscolaricadeGlobal { get; set; }
        public virtual Funcao FuncaoGlobal { get; set; }
    }
}