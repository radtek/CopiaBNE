using AdminLTE_Application;

namespace Sample.Models
{
    public class GridReservaTecnica
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
        public PagedList.IPagedList<VW_BANCO_EMPRESAS_RESERVA_TECNICA> lista { get; set; }
    }
}