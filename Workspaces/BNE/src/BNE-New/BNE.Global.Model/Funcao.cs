using System;

namespace BNE.Global.Model
{
    public class Funcao
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public short Prioridade { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool FlgInativo { get; set; }

        public virtual Global.Model.CargoGlobal CargoGlobal { get; set; }
    }
}
