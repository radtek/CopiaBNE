using BNE.Comum.Model.Localizable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.Vaga.Model
{
    public class Beneficio : ILocalizableEntity
    {
        public int Id { get; set; }

        public String Descricao { get; set; }

        public System.Guid? PessoaJuridica { get; set; }

        public Comum.Model.Localizable.Enum.TranslationState TranslationState { get; set; }
    }
}
