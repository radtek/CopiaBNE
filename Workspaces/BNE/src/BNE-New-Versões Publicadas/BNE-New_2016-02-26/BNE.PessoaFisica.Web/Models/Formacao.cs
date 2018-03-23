using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.PessoaFisica.Web.Models
{
    public class Formacao
    {
        public int Id { get; set; }
        public short? AnoConclusao { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public short? CargaHoraria { get; set; }
        public bool Ativo { get; set; }
        public string NomeInstituicao { get; set; }
        public string NomeCurso { get; set; }
        public string NomeCidade { get; set; }


        public int? IdCidade { get; set; }
        public int? IdInstituicaoEnsino { get; set; }
        public int? IdCurso { get; set; }
        public int? IdEscolaridade { get; set; }
        public int? Idvaga { get; set; }


        public string CPF { get; set; }
        public DateTime? DataNascimento { get; set; }
        public int? IdPesquisa { get; set; }
        public string UrlPesquisa { get; set; }
        public string UrlVoltarLogado { get; set; }

        //public virtual Cidade Cidade { get; set; }
    }
}