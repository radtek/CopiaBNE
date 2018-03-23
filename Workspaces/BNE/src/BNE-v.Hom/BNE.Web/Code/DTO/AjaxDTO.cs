using System;
using System.Collections.Generic;

namespace BNE.Web.Code.DTO
{
    public class AjaxDTO
    {
        public List<CandidaturasI> Candidaturas { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalRegistros { get; set; }
    }


    public class CandidaturasI
    {
        public string codVaga { get; set; }
        public string Funcao { get; set; }
        public string Cidade { get; set; }
        public string DataCadastro { get; set; }
        public string linkVaga { get; set; }
        public string Status { get; set; }

    }

    public class EmpresasQueVisualizaram
    {
        public List<EmpresaVisualizaram> Visualizacoes { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalRegistros { get; set; }
    }

    public class EmpresaVisualizaram
    {
        public string Nome_Empresa { get; set; }
        public string Data_Visualizacao { get; set; }
    }

    public class EnvioCvEmpresaInfo
    {
        public List<EnvioCvEmpresa> EnvioEmpresa { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalRegistros { get; set; }
    }

    public class EnvioCvEmpresa
    {
        public string Nome_Empresa { get; set; }
        public string Data_Envio { get; set; }
    }

    public class AlertaVagasDTO
    {
        public List<String> Cidades { get; set; }
        public List<String> Funcoes { get; set; }
        public List<String> Dias { get; set; }
    }
}