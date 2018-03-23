using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.PessoaFisica.Domain.Command
{
    public class CurriculoParametro
    {
        public int idCurriculo {get;set;}
        public DateTime  DataCadastro {get;set;}
        public bool Ativo {get;set;}
        public string Valor {get;set;}
        public int idParametro { get; set; }

    }
}
