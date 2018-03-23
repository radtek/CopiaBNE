using System;

namespace BNE.PessoaFisica.Model
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
        public short? TempoExperiencia { get; set; }
        public decimal? PretensaoSalarial { get; set; }
        public int? IdVaga { get; set; }
        public int? IdCurriculo { get; set; }
        public DateTime DataCadastro { get; set; }
        public virtual Global.Model.Sexo Sexo { get; set; }
    }
}