using System;

namespace BNE.Web.LanHouse.BLL.Entity
{
    public sealed class ExperienciaProfissional
    {
        public string NomeEmpresa { get; set; }
        public int AreaBNE { get; set; }
        public DateTime DataAdmissao { get; set; }
        public DateTime? DataDemissao { get; set; }
        public string Atribuicoes { get; set; }
        public string DescricaoFuncao { get; set; }
        public Decimal? UltimoSalario { get; set; }
    }
}