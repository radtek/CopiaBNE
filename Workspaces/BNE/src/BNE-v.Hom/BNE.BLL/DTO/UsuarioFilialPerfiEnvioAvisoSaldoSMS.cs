using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.BLL.DTO
{
    public class UsuarioFilialPerfiEnvioAvisoSaldoSMS
    {
        public int idPerfil { get; set; }
        public int idUsuarioFilialPerfil { get; set; }
        public string dddCelular { get; set; }
        public string numeroCelular { get; set; }
        public string nomePessoa { get; set; }
        public string emailPessoa { get; set; }
        public string mensagem { get; set; }
    }
}
