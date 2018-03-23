using APIGateway.Authentication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using APIGateway.Utils;
using log4net;

namespace APIGateway.Handlers
{
    /// <summary>
    /// Handler responsavel pela autenticacao da requisicao no BNE. 
    /// Será executado assim que a requisição chegar ao servidor e definirá a credencial para o usuário, disponibilizando-o para o Endpoint.
    /// </summary>
    public class ProxyHandler : DelegatingHandler
    {
        public ProxyHandler(HttpConfiguration httpConfiguration)
        {
            InnerHandler = new HttpControllerDispatcher(httpConfiguration); 
        }

        private const string WWWAuthenticateHeader = "WWW-Authenticate";

        private static readonly ILog _logger = LogManager.GetLogger("GatewayAPI");

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
            //rota não configurada
            if (request.GetRouteData().Route.DataTokens == null ||
                request.GetRouteData().Route.DataTokens["EndpointId"] == null)
                return request.CreateResponse(HttpStatusCode.NotFound).TaskCompletionResponse();

            string urlSuffix = request.GetRouteData().Route.DataTokens["UrlSuffix"].ToString();
            Int16 endpointId = Convert.ToInt16(request.GetRouteData().Route.DataTokens["EndpointId"]);

            //recuperando a api por prefixo
            Model.Api api = Domain.Api.CarregarPorSuffix(urlSuffix);
            Model.SistemaCliente sistema = null;
            List<Model.Header> forwardHeaders = new List<Model.Header>() { new Model.Header() { Item = "BNE_Api_Gateway", Value = "true" } };
            Model.Usuario usuario = null;

            //Buscando endpoint na api.
            Model.Endpoint endpoint = api.Endpoints.FirstOrDefault(e => e.Id == endpointId);
            if (endpoint == null)
                return request.CreateResponse(HttpStatusCode.NotFound, new { Message = "Endpoint não encontrado na API" }).TaskCompletionResponse();

            if (api.AuthenticationType != null)
            {
                #region Autenticando usuário
                try
                {
                    usuario = Authenticate(request, api, out sistema);
                    forwardHeaders.AddRange(usuario.ForwardHeaders);
                }
                catch (AuthenticationException ae)
                {
                    if (endpoint.AllowAnonymous)
                    {
                        try
                        {
                            usuario = new ChaveSistemaCliente().Authenticate(request, out sistema);
                        }
                        catch (AuthenticationException ae2)
                        {
                            return request.CreateResponse(HttpStatusCode.Forbidden, new { Message = ae2.Message }).TaskCompletionResponse();
                        }
                        catch
                        {
                            throw;
                        }
                    }
                    else
                        return request.CreateResponse(HttpStatusCode.Forbidden, new { Message = ae.Message }).TaskCompletionResponse();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    return request.CreateResponse(HttpStatusCode.InternalServerError).TaskCompletionResponse();
                }
                #endregion Autenticando usuário

                //Verificando Acesso Sistema
                sistema = api.Sistemas.FirstOrDefault(s => s.Chave == sistema.Chave);
                if (sistema == null)
                    return request.CreateResponse(HttpStatusCode.Forbidden, new { Message = "Id de sistema não informada ou sem acesso à api" }).TaskCompletionResponse();
                forwardHeaders.AddRange(sistema.Headers);
            }

            //Se o Content do request não está vazio, efetua a leitura para armazenar o request antes do dispose
            //Será gravado na requisicao
            String requestContent = null;
            if (request.Content != null)
                requestContent = request.Content.ReadAsStringAsync().Result;

            //Iniciando contagem de tempo da API
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //Recuperando o Path relativo para executar a chamada
            var path = Flurl.Url.Combine(api.Url, endpoint.ResolveDestination(HttpContext.Current.Request.Url)) + HttpContext.Current.Request.Url.Query;
            
            //Chamando a API destino
            return Utils.Methods.RunAsync(path, forwardHeaders, request)
                .ContinueWith(task =>
                {
                    var response = task.Result;

                    //calculando tempo de execução
                    stopWatch.Stop(); double ts = stopWatch.Elapsed.TotalMilliseconds;

                    //Chamando o controle para gravação da requisicao
                    Domain.Requisicao.GravaRequisicao(endpoint, sistema, usuario, ts, request, requestContent, response);

                    //if (credentials == null && response.StatusCode == HttpStatusCode.Unauthorized)
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                        Challenge(request, response);

                    return response;
                });
        }

        /// <summary>
        /// Authenticate User
        /// </summary>
        Model.Usuario Authenticate(HttpRequestMessage request, Model.Api api, out Model.SistemaCliente sistema)
        {
            IAuthentication auth = (IAuthentication)Activator.CreateInstance(Type.GetType("APIGateway.Authentication." + api.AuthenticationType.Interface));
            Model.Usuario usuario;

            try
            {
                usuario = auth.Authenticate(request, out sistema);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return usuario;
        }
        
        /// <summary>
        /// Send the Authentication Challenge request
        /// </summary>
        /// <param name="message"></param>
        /// <param name="actionContext"></param>
        void Challenge(HttpRequestMessage request, HttpResponseMessage response)
        {
            var host = request.RequestUri.DnsSafeHost;
            response.Headers.Add(WWWAuthenticateHeader, string.Format("BNE realm=\"{0}\"", host));
        }

        
    }
}