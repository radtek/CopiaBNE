using System;

namespace BNE.Mensagem.Model
{
    public class Usuario
    {

        public long Id { get; set; }
        public Guid Guid { get; set; }

        public virtual Sistema Sistema { get; set; }

    }
}
