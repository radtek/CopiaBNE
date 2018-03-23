using System;
using System.Collections.Generic;

namespace BNE.PessoaFisica.Web.Models
{
    public class Curriculo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string ConfirmarEmail { get; set; }
        public string DDDCelular { get; set; }
        public string NumeroCelular { get; set; }
        public string Celular { get; set; }
        public int? IdFuncao { get; set; }
        public string Funcao { get; set; }
        public string DescricaoFuncao { get; set; }
        public int? IdCidade { get; set; }
        public string Cidade { get; set; }
        public int? TempoExperienciaAnos { get; set; }
        public int? TempoExperienciaMeses { get; set; }
        public int IdEscolaridade { get; set; }
        public int? IdDeficiencia { get; set; }
        public decimal? PretensaoSalarial { get; set; }
        public int? IdVaga { get; set; }
        public int? IdCurriculo { get; set; }
        public DateTime? DataCadastro { get; set; }
        public int NumeroCandidaturasGratis { get; set; }

        public string CPF { get; set; }
        public DateTime? DataNascimento { get; set; }
        public int? IdPesquisa { get; set; }
        public string UrlPesquisa { get; set; }
        public string UrlVoltarLogado { get; set; }

        public virtual ExperienciaProfissional ExperienciaProfissional { get; set; }
        public virtual Formacao Formacao { get; set; }
        public virtual AmigoIndicado IndicadoUm { get; set; }
        public virtual AmigoIndicado IndicadoDois { get; set; }
        public virtual AmigoIndicado IndicadoTres { get; set; }

        public string VagaOportunidade { get; set; }
        public bool Candidatar { set; get; } //para quando for salvar não fazer a candidatura automatica
        public bool Candidatou { set; get; } //para quando for salvar não fazer a candidatura automatica
        public string Sexo { get; set; }

        public bool EstaEmBH { get; set; }
        public bool EmpresaBloqueada { get; set; }
        public bool CurriculoVIP { get; set; }
        public bool FlgWhatsApp { get; set; }
        //vaga
        public Vaga Vaga { get; set; }

        public Curriculo() { }

        public Curriculo(Vaga vaga)
        {
            Vaga = vaga;
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
        public List<Deficiencia> ListarDeficiencia()
        {
            var lista = new List<Deficiencia>(new Deficiencia().ListarDeficiencias());
            return lista;
        }
    }
}