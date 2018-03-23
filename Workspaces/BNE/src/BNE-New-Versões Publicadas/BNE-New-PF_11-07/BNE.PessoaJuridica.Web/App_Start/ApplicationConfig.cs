using System.Configuration;
using System.Web;

namespace BNE.PessoaJuridica.Web
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
            application[Enumeradores.ApplicationKeys.EnderecoApiPessoaJuridica.ToString()] = ConfigurationManager.AppSettings["EnderecoApiPessoaJuridica"];
        }
      
    }
}