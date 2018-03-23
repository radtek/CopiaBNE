using System;
using System.Collections.Generic;

namespace BNE.ExceptionLog.Model
{
    public class Error : MessageBase
    {

        public string CustomMessage { get; set; }
        public TraceLog TraceLog { get; set; }

        public Error()
        {
            this.TipoMensagem = new TipoMensagem(Tipo.Erro);
        }

        public class Ocorrencia
        {
            public DateTime DataCadastro { get; set; }
            public string Payload { get; set; }
            public string Usuario { get; set; }
        }
    }
}
