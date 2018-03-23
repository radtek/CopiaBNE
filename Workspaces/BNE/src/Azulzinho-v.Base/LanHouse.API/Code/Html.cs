using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanHouse.API.Code
{
    public class Html
    {
        private string urlServer = "http://lanhouse.bne.com.br/";

        public string html;
        public string HtmlHeadPadrao = "<!DOCTYPE html><html>" +
        "<head>"+
        "<meta charset=\"utf-8\" />"+
        "<title>{{\"Lan House BNE\"}}</title>"+
        "<script src=\"http://lanhouse.bne.com.br/Scripts/bootstrap.min.js\"></script>" +
        "<link href=\"http://lanhouse.bne.com.br/css/bootstrap.css\" rel=\" stylesheet\" />" +
        "<link href=\"http://lanhouse.bne.com.br/css/LanHouse.css\" rel=\"stylesheet\" />" +
        "<link href=\"http://lanhouse.bne.com.br/css/font-awesome.min.css\" rel=\"stylesheet\" />" +
        "<link href=\"http://lanhouse.bne.com.br/css/fonts/styles.css\" rel=\"stylesheet\" />";

        public string HtmlCVElegante = "<link href=\"http://lanhouse.bne.com.br/css/imprimirCVElegante.css\" rel=\"stylesheet\" /></head><body>";
        public string HtmlCVModerno = "<link href=\"http://lanhouse.bne.com.br/css/imprimirCVModerno.css\" rel=\"stylesheet\" /></head><body>";
        public string HtmlCVBNE = "<link href=\"http://lanhouse.bne.com.br/css/imprimirCVBNE.css\" rel=\"stylesheet\" /></head><body>";
    
    public string HtmlFooter="</body></html>";
    }
}