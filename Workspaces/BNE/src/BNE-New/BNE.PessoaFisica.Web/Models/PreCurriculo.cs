using System;
using System.Collections.Generic;

namespace BNE.PessoaFisica.Web.Models
{
    public class PreCurriculo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sexo { get; set; }
        public string Email { get; set; }
        public string ConfirmarEmail { get; set; }
        public string Celular { get; set; }
        public string DDDCelular { get; set; }
        public string NumeroCelular { get; set; }
        public string Cidade { get; set; }
        public string Funcao { get; set; }

        public decimal? PretensaoSalarial { get; set; }
        public int? TempoExperienciaAnos { get; set; }
        public int? TempoExperienciaMeses { get; set; }
        public int IdEscolaridade { get; set; }
        public int? IdDeficiencia { get; set; }

        public string CPF { get; set; }
        public DateTime? DataNascimento { get; set; }
        public bool FlgWhatsApp { get; set; }


        public Vaga Vaga { get; set; }
        public int? IdVaga { get; set; }

        public bool Candidatar { set; get; } //para quando for salvar não fazer a candidatura automatica


        public List<Escolaridade> ListarEscolaridades()
        {
            var lista = new List<Escolaridade>(new Escolaridade().ListarEscolaridades());
            return lista;
        }

        public List<Deficiencia> ListarDeficiencia()
        {
            var lista = new List<Deficiencia>(new Deficiencia().ListarDeficiencias());
            return lista;
        }
    }
}