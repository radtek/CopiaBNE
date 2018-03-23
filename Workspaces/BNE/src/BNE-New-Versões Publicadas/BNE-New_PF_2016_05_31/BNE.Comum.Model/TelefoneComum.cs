using System;

namespace BNE.Comum.Model
{
    public abstract class TelefoneComum
    {
        public Int64 Id { get; set; }
        public byte DDD { get; set; }
        public decimal Numero { get; set; }
        public decimal? Ramal { get; set; }
    }
}