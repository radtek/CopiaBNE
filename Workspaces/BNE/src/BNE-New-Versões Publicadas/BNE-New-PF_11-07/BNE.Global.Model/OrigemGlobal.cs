using System;

namespace BNE.Global.Model
{
    public class OrigemGlobal
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
        public string URL { get; set; }


        public virtual TipoOrigemGlobal TipoOrigem { get; set; }
    }
}