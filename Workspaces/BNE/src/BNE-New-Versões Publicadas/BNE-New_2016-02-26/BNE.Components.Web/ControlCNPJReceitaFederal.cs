using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BNE.Components.Web
{
    public static class ControlCNPJReceitaFederal
    {

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
            {
                return objValue;
            }

            return null;
        }
        public static void SetViewDataValue(this HtmlHelper htmlHelper, string key, object value)
        {
            htmlHelper.ViewData[key] = value;
        }
        public static MvcHtmlString CNPJReceitaFederal<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, InputType inputType = InputType.Text, object htmlAttributes = null, string placeHolder = null, bool isReadOnly = false, bool disabled = false, bool autoFocus = false, string informationText = "Digite os caracteres acima:", string informationTextCssClass = "txt_caract")
        {
            string expressionText = ExpressionHelper.GetExpressionText(expression);

            return CNPJReceitaFederal(helper, expressionText, inputType, htmlAttributes, placeHolder, isReadOnly, disabled, autoFocus, informationText, informationTextCssClass);
        }
        public static MvcHtmlString CNPJReceitaFederal(this HtmlHelper helper, string name, InputType inputType = InputType.Text, object htmlAttributes = null, string placeHolder = null, bool isReadOnly = false, bool disabled = false, bool autoFocus = false, string informationText = "Digite os caracteres acima:", string informationTextCssClass = "txt_caract")
        {
            TagBuilder captchaBuilder = new TagBuilder("div");

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
            hiddenBuilder.MergeAttribute("value", helper.DadosFormularioReceita().SessionId);

            var imgBuilder = new TagBuilder("img");

            imgBuilder.MergeAttribute("src", string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(helper.DadosFormularioReceita().Img)));

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
        public static Uri FullyQualifiedUri(string relativeOrAbsolutePath)
        {
            Uri baseUri = HttpContext.Current.Request.Url;
            string path = UrlHelper.GenerateContentUrl(relativeOrAbsolutePath, new HttpContextWrapper(HttpContext.Current));
            Uri instance;
            Uri.TryCreate(baseUri, path, out instance);
            return instance; // instance will be null if the uri could not be created
        }
        public static string FullyQualifiedPath(string relativeOrAbsolutePath)
        {
            Uri baseUri = HttpContext.Current.Request.Url;
            string path = UrlHelper.GenerateContentUrl(relativeOrAbsolutePath, new HttpContextWrapper(HttpContext.Current));
            Uri instance;
            Uri.TryCreate(baseUri, path, out instance);
            return instance.AbsolutePath; // instance will be null if the uri could not be created
        }

        #region Eventos
        /// <summary>
        /// Handler para problema de comunicacao
        /// </summary>
        /// <param name="ex">Objeto que enviou o evento</param>
        public delegate void ProblemaComunicacaoHandler(Exception ex);
        /// <summary>
        /// Evento disparado quando houve um problema na comunicação com o site da receita federal
        /// </summary>
        public static event ProblemaComunicacaoHandler ProblemaComunicacao;
        #endregion

        #region DadosCnpjReceita
        /// <summary>
        /// Retorna os dados do CNPJ recuperados a partir da Receita Federal
        /// </summary>
        public static DadosCNPJReceitaFederal DadosCnpjReceita
        {
            get;
            private set;
        }
        #endregion

        #region StripHtml
        /// <summary>
        /// Quebra o HTML para posterior processamento
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
        #endregion

        #region RetornarPagina
        /// <summary>
        /// Recupera a página html do site da Receita Federal
        /// </summary>
        /// <returns></returns>
        public static DadosCNPJReceitaFederal RetornarPagina(string cnpj, string captcha, string sessionid, out string erro)
        {
            erro = string.Empty;

            WebResponse response = null;
            StreamReader reader = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.receita.fazenda.gov.br/pessoajuridica/Cnpj/cnpjreva/valida.asp");
                request.Method = WebRequestMethods.Http.Post;
                request.Headers.Add("Cookie", sessionid);
                request.ContentType = "application/x-www-form-urlencoded";

                StringBuilder urlEncoded = new StringBuilder();

                // alocando o bytebuffer

                urlEncoded.Append("origem=comprovante&");
                urlEncoded.AppendFormat("cnpj={0}&", cnpj);
                urlEncoded.AppendFormat("txtTexto_captcha_serpro_gov_br={0}&", captcha);
                urlEncoded.Append("submit1=Consultar&");
                urlEncoded.Append("search_type=cnpj");

                // codificando em UTF8 (evita que sejam mostrados códigos malucos em caracteres especiais
                var byteBuffer = Encoding.UTF8.GetBytes(urlEncoded.ToString());

                request.ContentLength = byteBuffer.Length;
                var requestStream = request.GetRequestStream();
                requestStream.Write(byteBuffer, 0, byteBuffer.Length);

                requestStream.Close();

                try
                {
                    response = request.GetResponse();
                }
                catch (Exception ex)
                {
                    if (ProblemaComunicacao != null)
                        ProblemaComunicacao(ex);
                }

                // Dados recebidos 
                var httpWebResponse = response as HttpWebResponse;
                if (httpWebResponse != null && httpWebResponse.StatusCode != HttpStatusCode.OK)
                    return null;

                if (response != null)
                {
                    Stream responseStream = response.GetResponseStream();

                    // Codifica os caracteres especiais para que possam ser exibidos corretamente
                    Encoding encoding = Encoding.Default;

                    // Preenche o reader
                    if (responseStream != null) reader = new StreamReader(responseStream, encoding);
                }

                Char[] charBuffer = new Char[256];
                if (reader != null)
                {
                    int count = reader.Read(charBuffer, 0, charBuffer.Length);

                    StringBuilder dados = new StringBuilder();

                    // Lê cada byte para preencher meu stringbuilder
                    while (count > 0)
                    {
                        dados.Append(new String(charBuffer, 0, count));
                        count = reader.Read(charBuffer, 0, charBuffer.Length);
                    }

                    string q = StripHtml(dados.ToString());

                    // Fecha tudo
                    requestStream.Close();
                    response.Close();
                    reader.Close();

                    if (q.IndexOf("Erro na Consulta", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        erro = "Captcha inválido!";
                        return null;
                    }

                    if (q.IndexOf("Verifique se o mesmo foi digitado corretamente", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        erro = "CNPJ inválido!";
                        return null;
                    }

                    DadosCnpjReceita = new DadosCNPJReceitaFederal(q);

                    if (!DadosCnpjReceita.Ativa)
                    {
                        erro = "CNPJ não está ativo na Receita Federal!";
                        return null;
                    }
                }

                return DadosCnpjReceita;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region ReceitaOnline
        public static bool ReceitaOnline()
        {
            try
            {
                var uriCaptcha = new Uri("http://www.receita.fazenda.gov.br/pessoajuridica/cnpj/cnpjreva/captcha/gerarCaptcha.asp");
                var httpWebRequestCaptcha = (HttpWebRequest)WebRequest.Create(uriCaptcha);
                httpWebRequestCaptcha.Timeout = 5000;

                bool retorno = false;
                using (var sm = httpWebRequestCaptcha.GetResponse().GetResponseStream())
                {
                    if (sm != null)
                        retorno = true;
                }
                return retorno;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Sub classes

        #region FormularioReceita
        /// <summary>
        /// Representa os dados do formulário html.
        /// É utilizado para manter os dados de captha, sessão e etc.
        /// </summary>
        [Serializable]
        private class FormularioReceita
        {
            private byte[] _img;
            private string _sessionId;

            public FormularioReceita()
            {
                RecuperarCaptcha();
            }

            public string SessionId
            {
                get { return _sessionId; }
            }

            public byte[] Img
            {
                get { return _img; }
            }

            #region RecuperarCaptcha
            /// <summary>
            /// Retorna o valor dos cookies enviados na solicitação ao site da Receita Federal
            /// </summary>
            /// <returns>O valor do cookie de segurança</returns>
            private void RecuperarCaptcha()
            {
                var uri = new Uri("http://www.receita.fazenda.gov.br/pessoajuridica/Cnpj/cnpjreva/cnpjreva_solicitacao2.asp");

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);

                using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    _sessionId = httpWebResponse.Headers["Set-Cookie"];
                }

                var uriCaptcha = new Uri("http://www.receita.fazenda.gov.br/pessoajuridica/cnpj/cnpjreva/captcha/gerarCaptcha.asp");
                var httpWebRequestCaptcha = (HttpWebRequest)WebRequest.Create(uriCaptcha);
                httpWebRequestCaptcha.Timeout = 30000;

                httpWebRequestCaptcha.Headers.Add("Pragma", "no-cache");
                httpWebRequestCaptcha.Headers.Add("Origin", "http://www.receita.fazenda.gov.br");
                httpWebRequestCaptcha.Headers.Add("Accept-Language", "pt-BR,pt;q=0.8,en-US;q=0.5,en;q=0.3");
                httpWebRequestCaptcha.Headers.Add("Accept-Encoding", "gzip, deflate");
                httpWebRequestCaptcha.Headers.Add("Cookie", string.Format("flag=1; {0}", SessionId));
                httpWebRequestCaptcha.Host = "www.receita.fazenda.gov.br";
                httpWebRequestCaptcha.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:32.0) Gecko/20100101 Firefox/32.0";
                httpWebRequestCaptcha.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                httpWebRequestCaptcha.Referer = "http://www.receita.fazenda.gov.br/pessoajuridica/cnpj/cnpjreva/cnpjreva_solicitacao2.asp";
                httpWebRequestCaptcha.KeepAlive = true;

                using (var smM = new MemoryStream())
                using (var sm = httpWebRequestCaptcha.GetResponse().GetResponseStream())
                {
                    var b = new byte[32768];
                    int r;
                    while (sm != null && (r = sm.Read(b, 0, b.Length)) > 0)
                        smM.Write(b, 0, r);

                    var byteArray = smM.ToArray();
                    _img = byteArray;
                }
            }
            #endregion

        }
        #endregion

        #region DadosCNPJReceitaFederal
        /// <summary>
        /// Representa os dados do cartão CNPJ extraídos do site da Receita Federal
        /// </summary>
        [Serializable]
        public class DadosCNPJReceitaFederal
        {

            #region Properties
            /// <summary>
            /// O número de CNPJ
            /// </summary>
            public decimal Cnpj { get; private set; }
            /// <summary>
            /// Data de abertura da empresa
            /// </summary>
            public DateTime DataAbertura { get; private set; }
            /// <summary>
            /// Razão Social da empresa
            /// </summary>
            public String RazaoSocial { get; private set; }
            /// <summary>
            /// Nome Fantasia da empresa
            /// </summary>
            public String NomeFantasia { get; private set; }
            /// <summary>
            /// CNAE principal da empresa
            /// </summary>
            public String CNAEPrincipal { get; private set; }
            /// <summary>
            /// CNAE secundário da empresa
            /// </summary>
            public String CNAESecundario { get; private set; }
            /// <summary>
            /// Natureza Jurídica
            /// </summary>
            public String NaturezaJuridica { get; private set; }
            /// <summary>
            /// Porte da empresa
            /// </summary>
            public String PorteEmpresa { get; private set; }
            /// <summary>
            /// Logradouro (Nome da Rua)
            /// </summary>
            public String Logradouro { get; private set; }
            /// <summary>
            /// Número 
            /// </summary>
            public String Numero { get; private set; }
            /// <summary>
            /// Complemento de logradouro
            /// </summary>
            public String Complemento { get; private set; }
            /// <summary>
            /// CEP
            /// </summary>
            public String CEP { get; private set; }
            /// <summary>
            /// Bairro
            /// </summary>
            public String Bairro { get; private set; }
            /// <summary>
            /// Municipio
            /// </summary>
            public String Municipio { get; private set; }
            /// <summary>
            /// Estado
            /// </summary>
            public String UF { get; private set; }
            /// <summary>
            /// Se a empresa está ou não ativa
            /// </summary>
            public Boolean Ativa { get; private set; }
            /// <summary>
            /// Motivo da situação
            /// </summary>
            public String MotivoSituacao { get; private set; }
            /// <summary>
            /// Usado para situações especiais
            /// </summary>
            public String SituacaoEspecial { get; private set; }
            #endregion

            #region Métodos
            private int GetFirstIndex(String dados, String fragmento, int startIndex)
            {
                var index = dados.IndexOf(fragmento, startIndex, StringComparison.OrdinalIgnoreCase);
                if (index > -1)
                    return index + fragmento.Length;
                return index;
            }
            private int GetLastIndex(String dados, String fragmento, int startIndex)
            {
                var index = dados.IndexOf(fragmento, startIndex, StringComparison.OrdinalIgnoreCase);
                if (index > -1)
                    return index;
                return index;
            }
            private string GetData(string originalData, int startIndex, int lastIndex, DataTypes dataTypes = DataTypes.Default)
            {
                bool isCNAE = (dataTypes & DataTypes.CNAE) == DataTypes.CNAE;
                bool isNaturezaJuridica = (dataTypes & DataTypes.NaturezaJuridica) == DataTypes.NaturezaJuridica;
                bool isCNPJ = (dataTypes & DataTypes.CNPJ) == DataTypes.CNPJ;
                bool isCEP = (dataTypes & DataTypes.CEP) == DataTypes.CEP;

                var data = originalData.Substring(startIndex, lastIndex - startIndex);

                data = Regex.Replace(data, @"\*|\r|\n|\t|&nbsp;", " "); //Para qualquer dado: remover isso

                if (isCNAE || isNaturezaJuridica) //Tratamento específico para recuperar apeans o código do CNAE ou NJ
                {
                    if (data.IndexOf("Não informada", StringComparison.OrdinalIgnoreCase) == -1)
                        data = data.Split(new[] { " - " }, StringSplitOptions.None)[0];
                }

                if (isCNPJ || isCNAE || isNaturezaJuridica || isCEP)
                    data = Regex.Replace(data, @"-|\.|\/", string.Empty); //Tirando caracteres desnecessários

                data = Regex.Replace(data, @"\s+", " "); //Removendo espaços

                return data.Trim();
            }
            #endregion

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

            #region Ctor
            /// <summary>
            /// Construtor - Recebe uma string HTML e extrái os dados do Cartão CNPJ
            /// </summary>
            /// <param name="dados">A página de cartão CNPJ do site da Receita Federal</param>
            public DadosCNPJReceitaFederal(String dados)
            {
                var origem = 0;
                var destino = 0;

                #region  CNPJ
                origem = GetFirstIndex(dados, "NÚMERO DE INSCRIÇÃO", origem);
                destino = GetLastIndex(dados, "MATRIZ", origem);

                if (destino == -1)
                    destino = GetLastIndex(dados, "FILIAL", origem);

                this.Cnpj = Convert.ToDecimal(GetData(dados, origem, destino, DataTypes.CNPJ));
                #endregion

                #region Data de abertura
                origem = GetFirstIndex(dados, "DATA DE ABERTURA", origem);
                destino = GetLastIndex(dados, "NOME EMPRESARIAL", origem);
                this.DataAbertura = DateTime.Parse(GetData(dados, origem, destino, DataTypes.Datetime), new CultureInfo("pt-BR"));
                #endregion

                #region Razão Social
                origem = GetFirstIndex(dados, "NOME EMPRESARIAL", origem);
                destino = GetLastIndex(dados, "TÍTULO DO ESTABELECIMENTO (NOME DE FANTASIA)", origem);
                this.RazaoSocial = GetData(dados, origem, destino);
                #endregion

                #region Nome Fantasia
                origem = GetFirstIndex(dados, "TÍTULO DO ESTABELECIMENTO (NOME DE FANTASIA)", origem);
                destino = GetLastIndex(dados, "CÓDIGO E DESCRIÇÃO DA ATIVIDADE ECONÔMICA PRINCIPAL", origem);
                this.NomeFantasia = GetData(dados, origem, destino);
                #endregion

                #region Atividade Economica Principal
                origem = GetFirstIndex(dados, "CÓDIGO E DESCRIÇÃO DA ATIVIDADE ECONÔMICA PRINCIPAL", origem);
                destino = GetLastIndex(dados, "CÓDIGO E DESCRIÇÃO DAS ATIVIDADES ECONÔMICAS SECUNDÁRIAS", origem);
                this.CNAEPrincipal = GetData(dados, origem, destino, DataTypes.CNAE);
                #endregion

                #region Atividade Economica Secundária
                origem = GetFirstIndex(dados, "CÓDIGO E DESCRIÇÃO DAS ATIVIDADES ECONÔMICAS SECUNDÁRIAS", origem);
                destino = GetLastIndex(dados, "CÓDIGO E DESCRIÇÃO DA NATUREZA JURÍDICA", origem);
                this.CNAESecundario = GetData(dados, origem, destino, DataTypes.CNAE); //);
                #endregion

                #region Código e descrição da natureza jurídica
                origem = GetFirstIndex(dados, "CÓDIGO E DESCRIÇÃO DA NATUREZA JURÍDICA", origem);
                destino = GetLastIndex(dados, "LOGRADOURO", origem);
                this.NaturezaJuridica = GetData(dados, origem, destino, DataTypes.NaturezaJuridica);
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
                this.Logradouro = GetData(dados, origem, destino);
                #endregion

                #region Número
                origem = GetFirstIndex(dados, "NÚMERO", origem);
                destino = GetLastIndex(dados, "COMPLEMENTO", origem);
                this.Numero = GetData(dados, origem, destino);
                #endregion

                #region Complemento
                origem = GetFirstIndex(dados, "COMPLEMENTO", origem);
                destino = GetLastIndex(dados, "CEP", origem);
                this.Complemento = GetData(dados, origem, destino);
                #endregion

                #region Cep
                origem = GetFirstIndex(dados, "CEP", origem);
                destino = GetLastIndex(dados, "BAIRRO/DISTRITO", origem);
                this.CEP = GetData(dados, origem, destino, DataTypes.CEP);
                #endregion

                #region Bairro
                origem = GetFirstIndex(dados, "BAIRRO/DISTRITO", origem);
                destino = GetLastIndex(dados, "MUNICÍPIO", origem);
                this.Bairro = GetData(dados, origem, destino);
                #endregion

                #region Município
                origem = GetFirstIndex(dados, "MUNICÍPIO", origem);
                destino = GetLastIndex(dados, "UF", origem);
                this.Municipio = GetData(dados, origem, destino);
                #endregion

                #region UF
                origem = GetFirstIndex(dados, "UF", origem);
                destino = GetLastIndex(dados, "ENDEREÇO ELETRÔNICO", origem);

                if (destino == -1)
                    destino = GetLastIndex(dados, "TELEFONE", origem);

                this.UF = GetData(dados, origem, destino);
                #endregion

                #region Empresa Ativa
                origem = GetFirstIndex(dados, "SITUAÇÃO CADASTRAL", origem);
                destino = GetLastIndex(dados, "DATA DA SITUAÇÃO CADASTRAL", origem);
                this.Ativa = GetData(dados, origem, destino).Equals("ATIVA", StringComparison.OrdinalIgnoreCase);
                #endregion

                #region Motivo da situação
                origem = GetFirstIndex(dados, "MOTIVO DE SITUAÇÃO CADASTRAL", origem);
                destino = GetLastIndex(dados, "SITUAÇÃO ESPECIAL", origem);
                this.MotivoSituacao = GetData(dados, origem, destino);
                #endregion

                #region Situação Especial
                origem = GetFirstIndex(dados, "SITUAÇÃO ESPECIAL", origem);
                destino = GetLastIndex(dados, "DATA DA SITUAÇÃO ESPECIAL", origem);
                this.SituacaoEspecial = GetData(dados, origem, destino);
                #endregion

            }
            #endregion

        }
        #endregion

        #endregion


    }
}
