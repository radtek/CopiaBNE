using System;
using System.Collections.Generic;

namespace BNE.Integration
{
    public class Curriculo
    {
        public int Id { get; set; }
        public string Observacoes { get; set; }
        public decimal PretensaoSalarial { get; set; }

        public List<FuncaoPretendida> FuncoesPretendidas { get; set; }
        public List<ExperienciaProfissional> ExperienciasProfissionais { get; set; }
        public List<Formacao> Formacoes { get; set; }
        public List<NivelIdioma> Idiomas { get; set; }
        public List<Disponibilidade> Disponibilidades { get; set; }

        public Pessoa Pessoa { get; set; }
    }

    public class Pessoa
    {
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Email { get; set; }
        public string NumeroDDDCelular { get; set; }
        public string NumeroCelular { get; set; }
        public string NumeroDDDTelefone { get; set; }
        public string NumeroTelefone { get; set; }
        public decimal CPF { get; set; }
        public string RG { get; set; }
        public string RGOrgaoEmissor { get; set; }
        public decimal? Peso { get; set; }
        public decimal? Altura { get; set; }
        public string NumeroHabilitacao { get; set; }
        public string CategoriaHabilitacao { get; set; }

        public string EstadoCivil { get; set; }
        public string Raca { get; set; }
        public string Sexo { get; set; }
        public string Deficiencia { get; set; }

        public Endereco Endereco { get; set; }
        public List<Veiculo> Veiculos { get; set; }

    }

    public class Endereco
    {
        public string NumeroCEP { get; set; }
        public string DescricaoLogradouro { get; set; }
        public string NumeroEndereco { get; set; }
        public string DescricaoComplemento { get; set; }
        public string DescricaoBairro { get; set; }
        public string Cidade { get; set; }
    }

    public class FuncaoPretendida
    {
        public string Funcao { get; set; }
        public short? TempoExperiencia { get; set; }
    }

    public class ExperienciaProfissional
    {
        public string Funcao { get; set; }
        public string AtividadeExercida { get; set; }
        public DateTime DataAdmissao { get; set; }
        public DateTime? DataDemissao { get; set; }
        public string Empresa { get; set; }
        public string Area { get; set; }
        public decimal? UltimoSalario { get; set; }
    }

    public class Veiculo
    {
        public string Tipo { get; set; }
        public string Modelo { get; set; }
        public short Ano { get; set; }
    }

    public class Disponibilidade
    {
        public string Descricao { get; set; }
    }

    public class NivelIdioma
    {
        public string Idioma { get; set; }
        public string Nivel { get; set; }
    }

    public class Formacao
    {
        public string Escolaridade { get; set; }
        public short? AnoConclusao { get; set; }
        public short? CargaHoraria { get; set; }
        public string NomeInstituicao { get; set; }
        public string NomeCurso { get; set; }
    }

}
