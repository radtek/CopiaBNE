using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business.DTO
{
    public class PessoaFisicaEnvioSMSTanque
    {
        public int idDestinatario { get; set; }
        public string dddCelular { get; set; }
        public string numeroCelular { get; set; }
        public string nomePessoa { get; set; }
        public string emailPessoa { get; set; }
        public string mensagem { get; set; }
    }
}
