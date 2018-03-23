using System;
using System.Collections.Generic;

namespace BNE.BLL.DTO
{
    public class Curriculo
    {
        public int IdCurriculo { get; set; }
        public int IdPessoaFisica { get; set; }
        public int IdUsuarioFilialPerfil { get; set; }
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
        public bool Inativo { get; set; }
        public bool Bloqueado { get; set; }
        public bool VIP { get; set; }
        public bool AceitaEstagio { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataAtualizacaoCurriculo { get; set; }
        public List<FuncaoPretendida> FuncoesPretendidas { get; set; }
        public List<ExperienciaProfissional> Experiencias { get; set; }
        public List<Idioma> Idiomas { get; set; }
        public List<Formacao> Formacoes { get; set; }
        public List<Formacao> Cursos { get; set; }
        public List<Veiculo> Veiculos { get; set; }
        public List<DisponibilidadeMorarEm> DisponibilidadesMorarEm { get; set; }
        public List<DisponibilidadeTrabalho> DisponibilidadesTrabalho { get; set; }
        public bool? Flg_WhatsApp { get; set; }
        //[Obsolete("Obtado por não utilização/disponibilização.")]
        //public List<TipoVinculo> TipoVinculos { get; set; }
    }

    public class FuncaoPretendida
    {
        public string NomeFuncaoPretendida { get; set; }
        public Int16? QuantidadeExperiencia { get; set; }

        #region MesesExperiencia
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Int16 MesesExperiencia
        {
            get
            {
                if (this.QuantidadeExperiencia.HasValue)
                {
                    if (!(this.QuantidadeExperiencia.Value % 12).Equals(0))
                        return Convert.ToInt16(this.QuantidadeExperiencia.Value - (AnosExperiencia * 12));
                }
                return 0;
            }
        }
        #endregion

        #region AnosExperiencia
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Int16 AnosExperiencia
        {
            get
            {
                if (this.QuantidadeExperiencia.HasValue)
                    return Convert.ToInt16(this.QuantidadeExperiencia.Value / 12);
                return 0;
            }
        }
        #endregion

    }

    public class ExperienciaProfissional
    {
        public string Funcao { get; set; }
        public string RazaoSocial { get; set; }
        public string DescricaoAtividade { get; set; }
        public string AreaBNE { get; set; }
        public DateTime DataAdmissao { get; set; }
        public DateTime? DataDemissao { get; set; }
        public decimal? VlrSalario { get; set; }
    }

    public class Idioma
    {
        public string DescricaoIdioma { get; set; }
        public string NivelIdioma { get; set; }
    }

    public class Formacao
    {
        public string DescricaoFormacao { get; set; }
        public string DescricaoCurso { get; set; }
        public string NomeFonte { get; set; }
        public string SiglaFonte { get; set; }
        public string SituacaoFormacao { get; set; }
        public short? AnoConclusao { get; set; }
        public string Periodo { get; set; }
    }

    public class Veiculo
    {
        public string Ano { get; set; }
        public string Modelo { get; set; }
        public string Tipo { get; set; }
    }

    public class DisponibilidadeMorarEm
    {
        public string Descricao { get; set; }
    }

    public class DisponibilidadeTrabalho
    {
        public string Descricao { get; set; }
    }

    public class TipoVinculo
    {
        public string Descricao { get; set; }
    }

}
