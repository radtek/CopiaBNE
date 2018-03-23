using System;

namespace BNE.ValidaCelular.Model
{
    internal class CodigoConfirmacaoCelular
    {

        public int Id { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime? DataUtilizacao { get; set; }

        public string CodigoConfirmacao { get; set; }

        public string NumeroDDDCelular { get; set; }

        public string NumeroCelular { get; set; }

    }
}
