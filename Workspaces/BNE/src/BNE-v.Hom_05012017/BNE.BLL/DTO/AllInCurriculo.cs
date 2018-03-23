using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.BLL.DTO
{
    public class AllInCurriculo
    {
        public AllInCurriculo()
        {
            FuncoesPretendidas = new List<FuncaoPretendida>();
            UltimaExperiencia = new ExperienciaProfissional();
            Idiomas = new List<Idioma>();
            Formacoes = new List<Formacao>();
            DisponibilidadesTrabalho = new List<DisponibilidadeTrabalho>();
        }
        public int IdCurriculo { get; set; }
        public int IdPessoaFisica { get; set; }
        public decimal NumeroCPF { get; set; }
        public decimal? ValorPretensaoSalarial { get; set; }
        public decimal? ValorUltimoSalario { get; set; }
        public string NomeFuncaoPretendida { get; set; }
        public string NomeCompleto { get; set; }
        public string PrimeiroNome { get { return PessoaFisica.RetornarPrimeiroNome(NomeCompleto); } }
        public int Idade { get; set; }
        public string Email { get; set; }
        public string NumeroCEP { get; set; }
        public string Logradouro { get; set; }
        public string NumeroEndereco { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string NomeCidade { get; set; }
        public string NomeEstado { get; set; }
        public string SiglaEstado { get; set; }
        public string NumeroDDDCelular { get; set; }
        public string NumeroCelular { get; set; }
        public string NomeOperadoraCelular { get; set; }
        public string URLImagemOperadoraCelular { get; set; }
        public string NumeroDDDTelefone { get; set; }
        public string NumeroTelefone { get; set; }
        public string UltimaFormacaoCompleta { get; set; }
        public string UltimaFormacaoAbreviada { get; set; }
        public string Sexo { get; set; }
        public string EstadoCivil { get; set; }
        public string CategoriaHabilitacao { get; set; }
        public string Raca { get; set; }
        public decimal? Altura { get; set; }
        public decimal? Peso { get; set; }
        public string Deficiencia { get; set; }
        public string Observacao { get; set; }
        public string OutrosConhecimentos { get; set; }
        public bool? DisponibilidadeViajar { get; set; }
        public bool? TemFilhos { get; set; }
        public string NumeroDDDCelularRecado { get; set; }
        public string NumeroCelularRecado { get; set; }
        public string CelularRecadoContato { get; set; }
        public string NumeroDDDTelefoneRecado { get; set; }
        public string NumeroTelefoneRecado { get; set; }
        public string TelefoneRecadoContato { get; set; }
        public bool VIP { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime? DataAtualizacaoCurriculo { get; set; }
        public List<FuncaoPretendida> FuncoesPretendidas { get; set; }
        public ExperienciaProfissional UltimaExperiencia { get; set; }
        public List<Idioma> Idiomas { get; set; }
        public List<Formacao> Formacoes { get; set; }
        public List<DisponibilidadeTrabalho> DisponibilidadesTrabalho { get; set; }
        public int? QuemMeViuQuantidade { get; set; }

        public DateTime? DataModificacaoCurriculo { get; set; }

        public DateTime DataCadastroCurriculo { get; set; }

        public string TipoCurriculo { get; set; }

        public string SituacaoCurriculo { get; set; }

        public DateTime? UltimoPlanoVipFim { get; set; }

        public DateTime? UltimoPlanoVipInicio { get; set; }
    }
}
