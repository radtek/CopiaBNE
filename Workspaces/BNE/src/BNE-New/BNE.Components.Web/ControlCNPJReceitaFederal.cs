using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace BNE.Components.Web
{
    public static class ControlCNPJReceitaFederal
    {
        private const string URLGerarCaptcha = "http://www.receita.fazenda.gov.br/pessoajuridica/cnpj/cnpjreva/captcha/gerarCaptcha.asp";
        private const string URLSolicitacao = "http://www.receita.fazenda.gov.br/pessoajuridica/Cnpj/cnpjreva/cnpjreva_solicitacao2.asp";
        private const string URLValida = "http://www.receita.fazenda.gov.br/pessoajuridica/Cnpj/cnpjreva/valida.asp";

        private static FormularioReceita DadosFormularioReceita(this HtmlHelper htmlHelper)
        {
            if (htmlHelper.GetViewDataValue("FormularioReceita") == null)
                htmlHelper.SetViewDataValue("FormularioReceita", new FormularioReceita());

            return htmlHelper.GetViewDataValue("FormularioReceita") as FormularioReceita;
        }

        public static object GetViewDataValue(this HtmlHelper htmlHelper, string key)
        {
            object objValue;
            if (htmlHelper.ViewData.TryGetValue(key, out objValue))
                return objValue;

            return null;
        }

        public static void SetViewDataValue(this HtmlHelper htmlHelper, string key, object value)
        {
            htmlHelper.ViewData[key] = value;
        }

        public static MvcHtmlString CNPJReceitaFederal<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, InputType inputType = InputType.Text, object htmlAttributes = null, string placeHolder = null, bool isReadOnly = false, bool disabled = false, bool autoFocus = false, string informationText = "Digite os caracteres acima:", string informationTextCssClass = "txt_caract")
        {
            var expressionText = ExpressionHelper.GetExpressionText(expression);

            return CNPJReceitaFederal(htmlHelper, expressionText, inputType, htmlAttributes, placeHolder, isReadOnly, disabled, autoFocus, informationText, informationTextCssClass);
        }

        public static MvcHtmlString CNPJReceitaFederal(this HtmlHelper htmlHelper, string name, InputType inputType = InputType.Text, object htmlAttributes = null, string placeHolder = null, bool isReadOnly = false, bool disabled = false, bool autoFocus = false, string informationText = "Digite os caracteres acima:", string informationTextCssClass = "txt_caract")
        {
            var captchaBuilder = new TagBuilder("div");

            var inputBuilder = ControlInput.BuildInputType(name, inputType.ToString());

            // Add the html attributes
            if (htmlAttributes != null)
                inputBuilder.MergeAttributes(htmlAttributes as IDictionary<string, object> ?? new RouteValueDictionary(htmlAttributes));

            if (!string.IsNullOrWhiteSpace(placeHolder))
                inputBuilder.MergeAttribute("placeholder", placeHolder);

            var hiddenBuilder = new TagBuilder("input");
            hiddenBuilder.MergeAttribute("id", "CaptchaSessionId");
            hiddenBuilder.MergeAttribute("name", "CaptchaSessionId");
            hiddenBuilder.MergeAttribute("type", "hidden");
            hiddenBuilder.MergeAttribute("value", htmlHelper.DadosFormularioReceita().SessionId);

            var imgBuilder = new TagBuilder("img");

            imgBuilder.MergeAttribute("src", string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(htmlHelper.DadosFormularioReceita().ImgCaptcha)));

            var centerImgBuilder = new TagBuilder("center")
            {
                InnerHtml = imgBuilder.ToString()
            };

            captchaBuilder.InnerHtml += centerImgBuilder.ToString();

            var paragraphBuilder = new TagBuilder("p");
            paragraphBuilder.MergeAttribute("class", informationTextCssClass);
            paragraphBuilder.InnerHtml = informationText;

            captchaBuilder.InnerHtml += hiddenBuilder.ToString();
            captchaBuilder.InnerHtml += paragraphBuilder.ToString();
            captchaBuilder.InnerHtml += inputBuilder.ToString(TagRenderMode.SelfClosing);

            return MvcHtmlString.Create(captchaBuilder.ToString());
        }

        public static bool ReceitaOnline(this HtmlHelper htmlHelper)
        {
            return htmlHelper.DadosFormularioReceita().IsReceitaOnline;
        }

        /// <summary>
        ///     Quebra o HTML para posterior processamento
        /// </summary>
        /// <param name="html">O texto html geral</param>
        /// <returns>O texto html tratado</returns>
        private static string StripHtml(string html)
        {
            html = Regex.Replace(html, "<script.*?</script>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "<!--.*?-->", string.Empty, RegexOptions.Singleline);
            html = Regex.Replace(html, "<[^>]*>", string.Empty);

            return html;
        }

        #region Eventos

        /// <summary>
        ///     Handler para problema de comunicacao
        /// </summary>
        /// <param name="ex">Objeto que enviou o evento</param>
        public delegate void ProblemaComunicacaoHandler(Exception ex);

        /// <summary>
        ///     Evento disparado quando houve um problema na comunicação com o site da receita federal
        /// </summary>
        public static event ProblemaComunicacaoHandler ProblemaComunicacao;

        #endregion

        #region Sub classes

        #region FormularioReceita

        /// <summary>
        ///     Representa os dados do formulário html.
        ///     É utilizado para manter os dados de captha, sessão e etc.
        /// </summary>
        [Serializable]
        private class FormularioReceita
        {
            public FormularioReceita()
            {
                InicializarFormulario();
            }

            public string SessionId { get; private set; }

            public byte[] ImgCaptcha { get; private set; }

            public bool IsReceitaOnline
            {
                get { return !string.IsNullOrWhiteSpace(SessionId) && (ImgCaptcha != null); }
            }

            /// <summary>
            ///     Retorna o valor dos cookies enviados na solicitação ao site da Receita Federal
            /// </summary>
            /// <returns>O valor do cookie de segurança</returns>
            private void InicializarFormulario()
            {
                try
                {
                    SessionId = RequestSession();
                    ImgCaptcha = RequestCaptcha(SessionId);
                }
                catch (Exception ex)
                {
                    if (ProblemaComunicacao != null)
                    {
                        ProblemaComunicacao.Invoke(ex);
                    }
                }
            }

            private string RequestSession()
            {
                try
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(URLSolicitacao));
                    httpWebRequest.Timeout = 10000;

                    using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                    {
                        return httpWebResponse.Headers["Set-Cookie"];
                    }
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }

            private byte[] RequestCaptcha(string session)
            {
                try
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(URLGerarCaptcha));
                    httpWebRequest.Timeout = 10000;

                    httpWebRequest.Headers.Add("Pragma", "no-cache");
                    httpWebRequest.Headers.Add("Origin", "http://www.receita.fazenda.gov.br");
                    httpWebRequest.Headers.Add("Accept-Language", "pt-BR,pt;q=0.8,en-US;q=0.5,en;q=0.3");
                    httpWebRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                    httpWebRequest.Headers.Add("Cookie", string.Format("flag=1; {0}", session));
                    httpWebRequest.Host = "www.receita.fazenda.gov.br";
                    httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:32.0) Gecko/20100101 Firefox/32.0";
                    httpWebRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                    httpWebRequest.Referer = URLSolicitacao;
                    httpWebRequest.KeepAlive = true;

                    using (var smM = new MemoryStream())
                    {
                        using (var sm = httpWebRequest.GetResponse().GetResponseStream())
                        {
                            var b = new byte[32768];
                            int r;
                            while ((sm != null) && ((r = sm.Read(b, 0, b.Length)) > 0))
                                smM.Write(b, 0, r);

                            return smM.ToArray();
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        #endregion

        #region DadosCNPJReceitaFederal

        /// <summary>
        ///     Representa os dados do cartão CNPJ extraídos do site da Receita Federal
        /// </summary>
        [Serializable]
        public class DadosCNPJReceitaFederal
        {
            [Flags]
            public enum DataTypes
            {
                Default,
                Datetime,
                CNAE,
                NaturezaJuridica,
                CNPJ,
                CEP
            }

            public enum ErrorTypes
            {
                None,
                InvalidCaptcha,
                InvalidCnpj,
                CommunicationProblem
            }

            /// <summary>
            ///     O número de CNPJ
            /// </summary>
            public decimal Cnpj { get; private set; }

            /// <summary>
            ///     Data de abertura da empresa
            /// </summary>
            public DateTime DataAbertura { get; private set; }

            /// <summary>
            ///     Razão Social da empresa
            /// </summary>
            public string RazaoSocial { get; private set; }

            /// <summary>
            ///     Nome Fantasia da empresa
            /// </summary>
            public string NomeFantasia { get; private set; }

            /// <summary>
            ///     CNAE principal da empresa
            /// </summary>
            public string CNAEPrincipal { get; private set; }

            /// <summary>
            ///     CNAE secundário da empresa
            /// </summary>
            public string CNAESecundario { get; private set; }

            /// <summary>
            ///     Natureza Jurídica
            /// </summary>
            public string NaturezaJuridica { get; private set; }

            /// <summary>
            ///     Porte da empresa
            /// </summary>
            public string PorteEmpresa { get; private set; }

            /// <summary>
            ///     Logradouro (Nome da Rua)
            /// </summary>
            public string Logradouro { get; private set; }

            /// <summary>
            ///     Número
            /// </summary>
            public string Numero { get; private set; }

            /// <summary>
            ///     Complemento de logradouro
            /// </summary>
            public string Complemento { get; private set; }

            /// <summary>
            ///     CEP
            /// </summary>
            public string CEP { get; private set; }

            /// <summary>
            ///     Bairro
            /// </summary>
            public string Bairro { get; private set; }

            /// <summary>
            ///     Municipio
            /// </summary>
            public string Municipio { get; private set; }

            /// <summary>
            ///     Estado
            /// </summary>
            public string UF { get; private set; }

            /// <summary>
            ///     Se a empresa está ou não ativa
            /// </summary>
            public bool Ativa { get; private set; }

            /// <summary>
            ///     Motivo da situação
            /// </summary>
            public string MotivoSituacao { get; private set; }

            /// <summary>
            ///     Usado para situações especiais
            /// </summary>
            public string SituacaoEspecial { get; private set; }

            /// <summary>
            ///     Recupera a página html do site da Receita Federal
            /// </summary>
            /// <returns></returns>
            public DadosCNPJReceitaFederal RecuperarDados(string numeroCNPJ, string captcha, string sessionId, out ErrorTypes erro)
            {
                erro = ErrorTypes.None;

                WebResponse response = null;
                StreamReader reader = null;
                Stream requestStream = null;

                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(URLValida);
                    request.Timeout = 15000;
                    request.Method = WebRequestMethods.Http.Post;
                    request.Headers.Add("Cookie", sessionId);
                    request.ContentType = "application/x-www-form-urlencoded";

                    var urlEncoded = new StringBuilder();

                    urlEncoded.Append("origem=comprovante&");
                    urlEncoded.AppendFormat("cnpj={0}&", numeroCNPJ);
                    urlEncoded.AppendFormat("txtTexto_captcha_serpro_gov_br={0}&", captcha);
                    urlEncoded.Append("submit1=Consultar&");
                    urlEncoded.Append("search_type=cnpj");

                    // codificando em UTF8 (evita que sejam mostrados códigos malucos em caracteres especiais
                    var byteBuffer = Encoding.UTF8.GetBytes(urlEncoded.ToString());

                    request.ContentLength = byteBuffer.Length;
                    requestStream = request.GetRequestStream();
                    requestStream.Write(byteBuffer, 0, byteBuffer.Length);

                    requestStream.Close();

                    try
                    {
                        response = request.GetResponse();
                    }
                    catch (Exception)
                    {
                        erro = ErrorTypes.CommunicationProblem;
                        return null;
                    }

                    // Dados recebidos 
                    var httpWebResponse = response as HttpWebResponse;
                    if ((httpWebResponse != null) && (httpWebResponse.StatusCode != HttpStatusCode.OK))
                    {
                        erro = ErrorTypes.CommunicationProblem;
                        return null;
                    }

                    var responseStream = response.GetResponseStream();

                    // Codifica os caracteres especiais para que possam ser exibidos corretamente
                    var encoding = Encoding.Default;

                    // Preenche o reader
                    if (responseStream != null) reader = new StreamReader(responseStream, encoding);

                    var charBuffer = new char[256];
                    if (reader != null)
                    {
                        var count = reader.Read(charBuffer, 0, charBuffer.Length);

                        var dados = new StringBuilder();

                        while (count > 0)
                        {
                            dados.Append(new string(charBuffer, 0, count));
                            count = reader.Read(charBuffer, 0, charBuffer.Length);
                        }

                        var q = StripHtml(dados.ToString());

                        if (q.IndexOf("Erro na Consulta", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            erro = ErrorTypes.InvalidCaptcha;
                            return null;
                        }

                        if (q.IndexOf("Verifique se o mesmo foi digitado corretamente", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            erro = ErrorTypes.InvalidCnpj;
                            return null;
                        }

                        InterpretarDados(q);
                    }

                    return this;
                }
                catch
                {
                    erro = ErrorTypes.CommunicationProblem;
                    return null;
                }
                finally
                {
                    if (response != null) response.Close();
                    if (reader != null) reader.Close();
                    if (requestStream != null) requestStream.Close();
                }
            }

            private void InterpretarDados(string dados)
            {
                var origem = 0;
                var destino = 0;

                #region  CNPJ

                origem = GetFirstIndex(dados, "NÚMERO DE INSCRIÇÃO", origem);
                destino = GetLastIndex(dados, "MATRIZ", origem);

                if (destino == -1)
                    destino = GetLastIndex(dados, "FILIAL", origem);

                Cnpj = Convert.ToDecimal(GetData(dados, origem, destino, DataTypes.CNPJ));

                #endregion

                #region Data de abertura

                origem = GetFirstIndex(dados, "DATA DE ABERTURA", origem);
                destino = GetLastIndex(dados, "NOME EMPRESARIAL", origem);
                DataAbertura = DateTime.Parse(GetData(dados, origem, destino, DataTypes.Datetime), new CultureInfo("pt-BR"));

                #endregion

                #region Razão Social

                origem = GetFirstIndex(dados, "NOME EMPRESARIAL", origem);
                destino = GetLastIndex(dados, "TÍTULO DO ESTABELECIMENTO (NOME DE FANTASIA)", origem);
                RazaoSocial = GetData(dados, origem, destino);

                #endregion

                #region Nome Fantasia

                origem = GetFirstIndex(dados, "TÍTULO DO ESTABELECIMENTO (NOME DE FANTASIA)", origem);
                destino = GetLastIndex(dados, "CÓDIGO E DESCRIÇÃO DA ATIVIDADE ECONÔMICA PRINCIPAL", origem);
                NomeFantasia = GetData(dados, origem, destino);

                #endregion

                #region Atividade Economica Principal

                origem = GetFirstIndex(dados, "CÓDIGO E DESCRIÇÃO DA ATIVIDADE ECONÔMICA PRINCIPAL", origem);
                destino = GetLastIndex(dados, "CÓDIGO E DESCRIÇÃO DAS ATIVIDADES ECONÔMICAS SECUNDÁRIAS", origem);
                CNAEPrincipal = GetData(dados, origem, destino, DataTypes.CNAE);

                #endregion

                #region Atividade Economica Secundária

                origem = GetFirstIndex(dados, "CÓDIGO E DESCRIÇÃO DAS ATIVIDADES ECONÔMICAS SECUNDÁRIAS", origem);
                destino = GetLastIndex(dados, "CÓDIGO E DESCRIÇÃO DA NATUREZA JURÍDICA", origem);
                CNAESecundario = GetData(dados, origem, destino, DataTypes.CNAE); //);

                #endregion

                #region Código e descrição da natureza jurídica

                origem = GetFirstIndex(dados, "CÓDIGO E DESCRIÇÃO DA NATUREZA JURÍDICA", origem);
                destino = GetLastIndex(dados, "LOGRADOURO", origem);
                NaturezaJuridica = GetData(dados, origem, destino, DataTypes.NaturezaJuridica);

                #endregion

                #region Porte da empresa

                //origem = GetFirstIndex(dados, "PORTE DA EMPRESA", origem);//Inicio(dados, "PORTE DA EMPRESA");
                //destino = GetLastIndex(dados, "LOGRADOURO", origem);//dados.IndexOf("LOGRADOURO", origem);
                /*
                dummy = dados.Substring(origem, destino - origem)
                    .Replace("&nbsp;", " ")
                    .Replace("\r", " ")
                    .Replace("\n", " ")
                    .Replace("\t", " ")
                    .Trim();
                */
                //this.PorteEmpresa = GetData(dados, origem, destino);

                #endregion

                #region Logradouro

                origem = GetFirstIndex(dados, "LOGRADOURO", origem);
                destino = GetLastIndex(dados, "NÚMERO", origem);
                Logradouro = GetData(dados, origem, destino);

                #endregion

                #region Número

                origem = GetFirstIndex(dados, "NÚMERO", origem);
                destino = GetLastIndex(dados, "COMPLEMENTO", origem);
                Numero = GetData(dados, origem, destino);

                #endregion

                #region Complemento

                origem = GetFirstIndex(dados, "COMPLEMENTO", origem);
                destino = GetLastIndex(dados, "CEP", origem);
                Complemento = GetData(dados, origem, destino);

                #endregion

                #region Cep

                origem = GetFirstIndex(dados, "CEP", origem);
                destino = GetLastIndex(dados, "BAIRRO/DISTRITO", origem);
                CEP = GetData(dados, origem, destino, DataTypes.CEP);

                #endregion

                #region Bairro

                origem = GetFirstIndex(dados, "BAIRRO/DISTRITO", origem);
                destino = GetLastIndex(dados, "MUNICÍPIO", origem);
                Bairro = GetData(dados, origem, destino);

                #endregion

                #region Município

                origem = GetFirstIndex(dados, "MUNICÍPIO", origem);
                destino = GetLastIndex(dados, "UF", origem);
                Municipio = GetData(dados, origem, destino);

                #endregion

                #region UF

                origem = GetFirstIndex(dados, "UF", origem);
                destino = GetLastIndex(dados, "ENDEREÇO ELETRÔNICO", origem);

                if (destino == -1)
                    destino = GetLastIndex(dados, "TELEFONE", origem);

                UF = GetData(dados, origem, destino);

                #endregion

                #region Empresa Ativa

                origem = GetFirstIndex(dados, "SITUAÇÃO CADASTRAL", origem);
                destino = GetLastIndex(dados, "DATA DA SITUAÇÃO CADASTRAL", origem);
                Ativa = GetData(dados, origem, destino).Equals("ATIVA", StringComparison.OrdinalIgnoreCase);

                #endregion

                #region Motivo da situação

                origem = GetFirstIndex(dados, "MOTIVO DE SITUAÇÃO CADASTRAL", origem);
                destino = GetLastIndex(dados, "SITUAÇÃO ESPECIAL", origem);
                MotivoSituacao = GetData(dados, origem, destino);

                #endregion

                #region Situação Especial

                origem = GetFirstIndex(dados, "SITUAÇÃO ESPECIAL", origem);
                destino = GetLastIndex(dados, "DATA DA SITUAÇÃO ESPECIAL", origem);
                SituacaoEspecial = GetData(dados, origem, destino);

                #endregion
            }

            #region Métodos

            private int GetFirstIndex(string dados, string fragmento, int startIndex)
            {
                var index = dados.IndexOf(fragmento, startIndex, StringComparison.OrdinalIgnoreCase);
                if (index > -1)
                    return index + fragmento.Length;
                return index;
            }

            private int GetLastIndex(string dados, string fragmento, int startIndex)
            {
                var index = dados.IndexOf(fragmento, startIndex, StringComparison.OrdinalIgnoreCase);
                if (index > -1)
                    return index;
                return index;
            }

            private string GetData(string originalData, int startIndex, int lastIndex, DataTypes dataTypes = DataTypes.Default)
            {
                var isCNAE = (dataTypes & DataTypes.CNAE) == DataTypes.CNAE;
                var isNaturezaJuridica = (dataTypes & DataTypes.NaturezaJuridica) == DataTypes.NaturezaJuridica;
                var isCNPJ = (dataTypes & DataTypes.CNPJ) == DataTypes.CNPJ;
                var isCEP = (dataTypes & DataTypes.CEP) == DataTypes.CEP;

                var data = originalData.Substring(startIndex, lastIndex - startIndex);

                data = Regex.Replace(data, @"\*|\r|\n|\t|&nbsp;", " "); //Para qualquer dado: remover isso

                if (isCNAE || isNaturezaJuridica) //Tratamento específico para recuperar apeans o código do CNAE ou NJ
                    if (data.IndexOf("Não informada", StringComparison.OrdinalIgnoreCase) == -1)
                        data = data.Split(new[] { " - " }, StringSplitOptions.None)[0];

                if (isCNPJ || isCNAE || isNaturezaJuridica || isCEP)
                    data = Regex.Replace(data, @"-|\.|\/", string.Empty); //Tirando caracteres desnecessários

                data = Regex.Replace(data, @"\s+", " "); //Removendo espaços

                return data.Trim();
            }

            #endregion
        }

        #endregion

        #endregion
    }
}