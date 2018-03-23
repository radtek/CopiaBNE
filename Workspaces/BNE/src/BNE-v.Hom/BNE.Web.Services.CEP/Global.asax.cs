using System.Configuration;
using System.Web;
using System.Web.Http;
using BNE.Web.Services.Solr.Models;
using Microsoft.Practices.ServiceLocation;
using SolrNet;
using SolrNet.Impl;
using SolrNet.Utils;

namespace BNE.Web.Services.Solr
{
    public class WebApiApplication : HttpApplication
    {
        private static readonly string solrURLBairro = ConfigurationManager.AppSettings["SolrServerBairro"];
        private static readonly string solrURLCidade = ConfigurationManager.AppSettings["SolrServerCidade"];
        private static readonly string solrURLFuncao = ConfigurationManager.AppSettings["SolrServerFuncao"];

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var connectionBairro = new SolrConnection(solrURLBairro);
            var connectionCidade = new SolrConnection(solrURLCidade);
            var connectionFuncao = new SolrConnection(solrURLFuncao);

            Startup.Init<Bairro>(connectionBairro);
            Startup.Init<Cidade>(connectionCidade);
            Startup.Init<Funcao>(connectionFuncao);

            #region Configuração de Custom Handlers
            /*
             * 
             * O handler padrão é o select, então não é preciso customizar
            var container = ServiceLocator.Current as Container;
            if (container != null)
            {
                container.Remove<ISolrQueryExecuter<Cidade>>();
                var instanceCidade = new SolrQueryExecuter<Cidade>(container.GetInstance<ISolrAbstractResponseParser<Cidade>>(), connectionCidade, container.GetInstance<ISolrQuerySerializer>(), container.GetInstance<ISolrFacetQuerySerializer>(), container.GetInstance<ISolrMoreLikeThisHandlerQueryResultsParser<Cidade>>())
                {
                    Handler = "/select"
                };
                container.Register<ISolrQueryExecuter<Cidade>>(typeof(ISolrQueryExecuter<Cidade>).FullName, c => instanceCidade);

                container.Remove<ISolrQueryExecuter<Funcao>>();
                var instanceFuncao = new SolrQueryExecuter<Funcao>(container.GetInstance<ISolrAbstractResponseParser<Funcao>>(), connectionFuncao, container.GetInstance<ISolrQuerySerializer>(), container.GetInstance<ISolrFacetQuerySerializer>(), container.GetInstance<ISolrMoreLikeThisHandlerQueryResultsParser<Funcao>>())
                {
                    Handler = "/select"
                };
                container.Register<ISolrQueryExecuter<Funcao>>(typeof(ISolrQueryExecuter<Funcao>).FullName, c => instanceFuncao);
            }
            */
            #endregion

            Bootstrapper.Run();
        }

    }
}
