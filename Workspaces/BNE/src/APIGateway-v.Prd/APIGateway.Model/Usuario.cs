using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APIGateway.Model
{
    public class Usuario
    {
        private List<Model.Header> _headers = new List<Model.Header>();

        public int Id { get; set; }
        public List<Model.Header> ForwardHeaders
        {
            get { return this._headers; }
            set { _headers = value;}
        }
        public Perfil PerfilDeAcesso { get; set; }

        public Usuario() { }
        public Usuario(Model.Perfil perfil)
        {
            this.PerfilDeAcesso = perfil;
        }
    }
}
