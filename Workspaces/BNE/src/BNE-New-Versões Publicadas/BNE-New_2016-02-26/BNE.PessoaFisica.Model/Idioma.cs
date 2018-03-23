using System;

namespace BNE.PessoaFisica.Model
{
    public class Idioma
    {
        public int Id { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }

        public virtual Global.Model.IdiomaGlobal IdiomaGlobal { get; set; }
        public virtual PessoaFisica PessoaFisica { get; set; }
        public virtual NivelIdioma NivelIdioma { get; set; }
    }
}