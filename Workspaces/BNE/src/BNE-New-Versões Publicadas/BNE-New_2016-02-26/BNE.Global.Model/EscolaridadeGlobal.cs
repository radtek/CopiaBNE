using System;

namespace BNE.Global.Model
{
    public class EscolaridadeGlobal
    {
        public short Id { get; set; }
        public string Descricao { get; set; }
        public string DescricaoBNE { get; set; }
        public string Abreviacao { get; set; }

        public virtual GrauEscolaridadeGlobal GrauEscolaridade { get; set; }
    }
}