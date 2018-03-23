using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.PessoaFisica.Web.Models
{
    public class Deficiencia
    {
        public int IdDeficiencia { get; set; }
        public string Descricao { get; set; }

        public List<Deficiencia> ListarDeficiencias()
        {
            return new List<Deficiencia>
            {
                new Deficiencia{IdDeficiencia = 0, Descricao="Selecione uma Deficiencia"},

                
                new Deficiencia{IdDeficiencia = 1, Descricao="Física"},
                new Deficiencia{IdDeficiencia = 2, Descricao="Auditiva"},
                new Deficiencia{IdDeficiencia = 3, Descricao="Visual"},
                new Deficiencia{IdDeficiencia = 4, Descricao="Mental"},
                new Deficiencia{IdDeficiencia = 5, Descricao="Múltipla"},
                new Deficiencia{IdDeficiencia = 6, Descricao="Reabilitado"},
                new Deficiencia{IdDeficiencia = 7, Descricao="Qualquer"}
            };
        }
    }
}