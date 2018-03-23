using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.BLL.DTO
{
   public class DTOBoletoPagarMe
    {
        public string UrlBoleto { get; set; }
        public string CodigoDeBarra { get; set; }
        public string NossoNumero { get; set; }
    }
}
