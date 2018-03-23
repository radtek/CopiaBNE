using System;

namespace BNE.Mensagem.Model
{
    public abstract class Mensagem
    {

        public long Id { get; set; }
        public string Parametros { get; set; }
        public DateTime? DataEnvio { get; set; }
        public DateTime DataCadastro { get; set; }

        public virtual Usuario UsuarioRemetente { get; set; }
        public virtual Usuario UsuarioDestinatario { get; set; }
        public virtual Status Status { get; set; }

    }
}
