using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.Bridge
{
    public class BNESessaoLoginResultValueInfo
    {
        public BNE.BLL.Curriculo Curriculo { get; set; }
        public bool ExisteCurriculoNaOrigem { get; set; }
        public bool UsaSTC { get; set; }
        public int QuantidadeEmpresas { get; set; }

    }
}
