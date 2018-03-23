using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.PessoaFisica.Domain.Model
{
    public class DadosEmpresa
    {
        public string NomeEmpresa { get; set; }
        public int? QuantidadeFuncionarios { get; set; }
        public string NumeroTelefone { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public DateTime? DataCadastro { get; set; }
        public int? QuantidadeCurriculosVisualizados { get; set; }
        public int? QuantidadeVagasDivulgadas { get; set; }
        public bool CurriculoVIP { get; set; }
        public decimal NumeroCNPJ { get; set; }
        public decimal ValorPlanoVIP { get; set; }
        public bool VagaConfidencial { get; set; }
        public string MensagemEmpresaConfidencial { get; set; }
        public bool VagaSine { get; set; }
        public string DesAreaBne { get; set; }
    }
}
