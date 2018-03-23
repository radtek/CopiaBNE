using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace APIGateway.Model
{
    public class Requisicao
    {
        public Guid Id { get; set; }

        public Endpoint Endpoint { get; set; }

        public double TempoExecucao { get; set; }
        public HttpStatusCode ResponseStatusCode { get; set; }
        public DateTime DataRequisicao { get; set; }
        public string Request { get; set; }
        public string RequestContent { get; set; }

        public Usuario Usuario { get; set; }

        public SistemaCliente SistemaCliente { get; set; }

        public Perfil Perfil { get; set; }
        public string PerfilString
        {
            get { return Perfil.ToString(); }
            private set { Perfil = (Perfil)Enum.Parse(typeof(Perfil), value, true); }
        }
        public string Response { get; set; }
        public string ResponseContent { get; set; }
    }
}
