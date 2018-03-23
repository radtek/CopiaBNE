using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace BNE.PessoaFisica.Web
{
    public class ApplicationConfig
    {
        public static void Configure(HttpApplicationState application)
        {
            application[Enumeradores.ApplicationKeys.EnderecoBNE.ToString()] = ConfigurationManager.AppSettings["EnderecoBNE"];
            application[Enumeradores.ApplicationKeys.UrlAutocompleteFuncao.ToString()] = ConfigurationManager.AppSettings["UrlAutocompleteFuncao"];
            application[Enumeradores.ApplicationKeys.UrlAutocompleteCidade.ToString()] = ConfigurationManager.AppSettings["UrlAutocompleteCidade"];
            application[Enumeradores.ApplicationKeys.LimiteAutocompleteFuncao.ToString()] = ConfigurationManager.AppSettings["LimiteAutocompleteFuncao"];
            application[Enumeradores.ApplicationKeys.LimiteAutocompleteCidade.ToString()] = ConfigurationManager.AppSettings["LimiteAutocompleteCidade"];
            application[Enumeradores.ApplicationKeys.EnderecoApiPessoaFisica.ToString()] = ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"];
        }
    }
}