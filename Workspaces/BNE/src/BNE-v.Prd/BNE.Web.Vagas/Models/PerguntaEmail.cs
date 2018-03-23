using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.Web.Vagas.Models
{
    [Serializable]
    public class PerguntaEmail
    {
        public int Identificador {get;set;}
        public string Email {get;set;}
        public int IdentificadorVaga { get; set; }
        public int IdPerguntaHistorico { get; set; }
        public string EmailAntigo { get; set; }


    }
}