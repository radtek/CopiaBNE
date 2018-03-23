using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sample.Models
{
    public class VisualizacaoEmpresa
    {
        public int pag { get; set; }
        public int rowsPag { get; set; }
        public int Qtd_Total { get; set; }
        public List<Visualizacao> listaVisualizacao { get; set; }
        public double TotalPag { get; set; }
        public string RazSocial { get; set; }
        

    }

    public class Visualizacao
    {
        public string Tipo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }
        public string Nme_Pessoa { get; set; }
        public string Url { get; set; }
        public string nme_cidade { get; set; }
        public string sig_Estado { get; set; }
        public string funcao { get; set; }
    }
}