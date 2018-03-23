using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.PessoaFisica.Web.Models
{
    public class Escolaridade
    {
        public int IdEscolaridade { get; set; }
        public string Descricao { get; set; }

        public List<Escolaridade> ListarEscolaridades()
        {
            return new List<Escolaridade>
            {
                new Escolaridade{IdEscolaridade = 0, Descricao="Selecione uma Escolaridade"},

                //new Escolaridade{IdEscolaridade = 1, Descricao="Alfabetizado"},
                //new Escolaridade{IdEscolaridade = 2, Descricao="1ª. a 4ª. Incompleto"},
                //new Escolaridade{IdEscolaridade = 3, Descricao="4ª. Série completa do 1º Grau"},
                //new Escolaridade{IdEscolaridade = 4, Descricao="5ª. a 8ª. Série Incompleta do 1º. Grau"},
                new Escolaridade{IdEscolaridade = 5, Descricao="1º. Grau Completo"},
                new Escolaridade{IdEscolaridade = 6, Descricao="2º. Grau Incompleto"},
                new Escolaridade{IdEscolaridade = 7, Descricao="2º. Grau Completo"},
                new Escolaridade{IdEscolaridade = 8, Descricao="Técnico/Pós-Médio Incompleto"},
                new Escolaridade{IdEscolaridade = 9, Descricao="Técnico/Pós-Médio Completo"},
                new Escolaridade{IdEscolaridade = 10, Descricao="Tecnólogo Incompleto"},
                new Escolaridade{IdEscolaridade = 12, Descricao="Tecnólogo Completo"},
                new Escolaridade{IdEscolaridade = 11, Descricao="Superior Incompleto"},
                new Escolaridade{IdEscolaridade = 13, Descricao="Superior Completo"},
                new Escolaridade{IdEscolaridade = 14, Descricao="Pós Graduação / Especialização"},
                new Escolaridade{IdEscolaridade = 15, Descricao="Mestrado"},
                new Escolaridade{IdEscolaridade = 16, Descricao="Doutorado"},
                new Escolaridade{IdEscolaridade = 17, Descricao="Pós-Doutorado"},
                //new Escolaridade{IdEscolaridade = 18, Descricao="Aperfeiçoamento"}, 
            };
        }
    }
}