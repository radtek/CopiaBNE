using System;

namespace BNE.Global.Model
{
    public class CargoGlobal
    {
        public short Id { get; set; }
        public short Prioridade { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool FlgInativo { get; set; }

        public virtual RamoAtividadeGlobal RamoAtividadeGlobal { get; set; }
        public virtual CategoriaCargoGlobal CategoriaCargoGlobal { get; set; }
    }
}