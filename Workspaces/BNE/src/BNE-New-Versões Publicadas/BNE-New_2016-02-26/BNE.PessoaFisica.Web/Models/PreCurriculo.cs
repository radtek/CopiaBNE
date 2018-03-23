using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BNE.PessoaFisica.Web.Models
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
        public int? TempoExperienciaAnos { get; set; }
        public int? TempoExperienciaMeses { get; set; }
        public decimal? PretensaoSalarial { get; set; }
        public int? IdVaga { get; set; }
        public int? IdCurriculo { get; set; }
        public DateTime? DataCadastro { get; set; }
        public string NumeroCandidaturasGratis { get; set; }

        public string CPF { get; set; }
        public DateTime? DataNascimento { get; set; }
        public int? IdPesquisa { get; set; }
        public string UrlPesquisa { get; set; }
        public string UrlVoltarLogado { get; set; }

        public virtual ExperienciaProfissional ExperienciaProfissional { get; set; }
        public virtual Formacao Formacao { get; set; }

        public string VagaOportunidade { get; set; }

        //vaga
        public Vaga vagaTela { get; set; }


        public PreCurriculo()
        {
        }


        public PreCurriculo(Models.Vaga vaga)
        {
            vagaTela = vaga;
        }

        public List<Escolaridade> ListarEscolaridades()
        {
            var lista = new List<Escolaridade>(new Escolaridade().ListarEscolaridades());
            return lista;
        }

        public List<RamoAtividade> ListarRamosAtividades()
        {
            var lista = new List<RamoAtividade>(new RamoAtividade().Listar());
            return lista;
        }
    }
}