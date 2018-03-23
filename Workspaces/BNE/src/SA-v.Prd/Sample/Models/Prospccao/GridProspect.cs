using AdminLTE_Application;
using Sample.DTO;
using System.Collections.Generic;

namespace Sample.Models.Empresa
{
    public class GridProspect
    {
        public int pag { get; set; }
        public int rowsPag { get; set; }
        public int Qtd_Total { get; set; }
        public double TotalPag { get; set; }
        //Filtros
        public string FlEstado { get; set; }
        public string FlCidade { get; set; }
        public string FlEmpresa { get; set; }
        public string FlArea { get; set; }
        public string FlPlano { get; set; }
        public string FlDataUltimoPlano { get; set; }
        public string FlDataCadastro { get; set; }
        public int? FlSituacaoEmpresa { get; set; }
        //ordernação
        public string Ordenacao { get; set; }
        public List<CobrancaRecorrencia> Recorrencia { set; get; }

        public PagedList.IPagedList<VW_Banco_Empresas> lista { get; set; }
    }
}