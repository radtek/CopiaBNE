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

    public class JaEnvieiDTO
    {
        public String NomeEmpresa { get; set; }
        public int TotalRegistros { get; set; }
        public string Funcao { get; set; }
        public string DataCandidatura { get; set; }
        public string HoraCandidatura { get; set; }
        public bool Oportunidade { get; set; }
        public bool Inativo { get; set; }
        public bool PCD { get; set; }
        public List<EtapaCandidaturaDTO> Etapas { get; set; }
    }

   
    public class EtapaCandidaturaDTO
    {
       public EtapaCandidaturaDTO(string etapa, string data, string hora)
        {
            this.Etapa = etapa;
            this.Data = data;
            this.Hora = hora;
        }
        public string Etapa { get; set; }
        public string Data { get; set; }
        public string Hora { get; set; }
    }

    public class QuemMeViuDTO
    {
        public int IdFilial { get; set; }
        public string  RazSocial { get; set; }
        public bool vip { get; set; }
        public string DataQuemMeViu { get; set; }
        public string HoraQuemMeViu { get; set; }
        public string Cidade { get; set; }
        public string SigEstado { get; set; }
        public int QtdFuncionario { get; set; }
        public string DataCadastro { get; set; }
        public string Telefone { get; set; }
        public int TotalVagas { get; set; }
        public int TotalVisualizacoes { get; set; }
        public int TotalRegistros { get; set; }
        public string AreaEmpresa { get; set; }
        public string UrlImg { get; set; }
        public string Bairro { get; set; }
        public string LinkVagas { get; set; }
    }
}