using System.Collections.Generic;
using BNE.BLL;

namespace BNE.Web.Vagas.Models
{
    public class Parametros
    {
        public string UrlAutocompleteFuncao { get; set; }
        public string UrlAutocompleteCidade { get; set; }
        public string LimiteAutocompleteFuncao { get; set; }
        public string LimiteAutocompleteCidade { get; set; }

        public Parametros()
        {
            var parametros = new List<BLL.Enumeradores.Parametro>
            {
                BLL.Enumeradores.Parametro.NumeroResultadosAutoCompleteFuncao,
                BLL.Enumeradores.Parametro.NumeroResultadosAutoCompleteCidade,
                BLL.Enumeradores.Parametro.UrlAutoCompleteCidade,
                BLL.Enumeradores.Parametro.UrlAutoCompleteFuncao
            };

            Dictionary<BLL.Enumeradores.Parametro, string> valoresParametros = Parametro.ListarParametros(parametros);

            UrlAutocompleteFuncao = valoresParametros[BLL.Enumeradores.Parametro.UrlAutoCompleteFuncao];
            UrlAutocompleteCidade = valoresParametros[BLL.Enumeradores.Parametro.UrlAutoCompleteCidade];
            UrlAutocompleteCidade = valoresParametros[BLL.Enumeradores.Parametro.UrlAutoCompleteCidade];


            LimiteAutocompleteFuncao = valoresParametros[BLL.Enumeradores.Parametro.NumeroResultadosAutoCompleteFuncao];
            LimiteAutocompleteCidade = valoresParametros[BLL.Enumeradores.Parametro.NumeroResultadosAutoCompleteCidade];
        }
    }
}