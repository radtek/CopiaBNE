using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.PessoaFisica.Web.Models
{
    public class Parametros
    {
        public string UrlAutocompleteFuncao { get; set; }
        public string UrlAutocompleteCidade { get; set; }
        public string LimiteAutocompleteFuncao { get; set; }
        public string LimiteAutocompleteCidade { get; set; }

        public Parametros()
        {
            UrlAutocompleteFuncao = HttpContext.Current.Application[Enumeradores.ApplicationKeys.UrlAutocompleteFuncao.ToString()].ToString();
            UrlAutocompleteCidade = HttpContext.Current.Application[Enumeradores.ApplicationKeys.UrlAutocompleteCidade.ToString()].ToString();
            LimiteAutocompleteFuncao = HttpContext.Current.Application[Enumeradores.ApplicationKeys.LimiteAutocompleteFuncao.ToString()].ToString();
            LimiteAutocompleteCidade = HttpContext.Current.Application[Enumeradores.ApplicationKeys.LimiteAutocompleteCidade.ToString()].ToString();
        }
    }
}