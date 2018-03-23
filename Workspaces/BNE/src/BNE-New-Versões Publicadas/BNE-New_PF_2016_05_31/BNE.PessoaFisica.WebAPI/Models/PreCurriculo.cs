using System;
using System.Collections.Generic;

namespace BNE.PessoaFisica.WebAPI.Models
{
    public class PreCurriculo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string DDDCelular { get; set; }
        public string Celular { get; set; }
        public int? IdFuncao { get; set; }
        public string DescricaoFuncao { get; set; }
        public int? IdCidade { get; set; }
        public short? TempoExperienciaAnos { get; set; }
        public short? TempoExperienciaMeses { get; set; }
        public decimal? PretensaoSalarial { get; set; }
        public int? IdVaga { get; set; }
        public int? IdCurriculo { get; set; }
        public DateTime? DataCadastro { get; set; }
        public int? IdPesquisa { get; set; }
        public string UrlPesquisa { get; set; }
        public virtual AmigoIndicado IndicadoUm { get; set; }
        public virtual AmigoIndicado IndicadoDois { get; set; }
        public virtual AmigoIndicado IndicadoTres { get; set; }

        public string CPF { get; set; }
        public DateTime? DataNascimento { get; set; }
        public bool Candidatar { set; get; }
        public Vaga vagaTela { get; set; }
        public string Sexo { get; set; }
    }

    public class InformacoesCurriculo
    {
        public bool EhVip { get; set; }
        public int idCurriculo { get; set; }
        public bool EmpresaBloqueada { get; set; }
        public bool JaEnvioCvParaVaga { get; set; }
    }
}