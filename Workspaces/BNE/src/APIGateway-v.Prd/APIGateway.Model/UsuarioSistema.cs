using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APIGateway.Model
{
    public class UsuarioSistemaCliente : Usuario
    {
        public SistemaCliente SistemaCliente { get; set; }
        public Perfil Perfil { get; set; }
        public List<Header> Headers { get; set; }

        public string PerfilString
        {
            get { return Perfil.ToString(); }
            private set { Perfil = (Perfil)Enum.Parse(typeof(Perfil), value, true); }
        }

    }
}
