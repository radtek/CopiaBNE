using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Security.Cryptography;
using System.Collections.Generic;
using BNE.BLL.Common;

namespace BNE.BLL.Custom
{
    public static class Helper
    {

        #region ConverterCPFCNPJStringDecimal
        /// <summary>
        /// Recebe uma string de cpf/cnpj com caracteres especiais (./-) e retorna um decimal
        /// </summary>
        /// <param name="numeroCPFCNPJ"></param>
        /// <returns>CPF/CNPJ decimal</returns>
        public static decimal ConverterCPFCNPJStringDecimal(string numeroCPFCNPJ)
        {
            return Convert.ToDecimal(LimparMascaraCPFCNPJ(numeroCPFCNPJ));
        }
        #endregion

        #region LimparMascaraCPFCNPJ
        /// <summary>
        /// Limpar os caracteres especiais do CPF ou CNPJ
        /// </summary>
        /// <returns>CPF/CNPJ limpo</returns>
        public static string LimparMascaraCPFCNPJ(string numeroCPFCNPJ)
        {
            numeroCPFCNPJ = numeroCPFCNPJ.Trim();
            return new Regex("[.\\/-]").Replace(numeroCPFCNPJ, string.Empty);
        }
        #endregion

        #region FormatarCPF
        public static string FormatarCPF(decimal numeroCPF)
        {
            return FormatarCPF(numeroCPF.ToString(CultureInfo.CurrentCulture));
        }
        public static string FormatarCPF(string numeroCPF)
        {
            if (string.IsNullOrEmpty(numeroCPF))
                return string.Empty;

            return numeroCPF.PadLeft(11, '0').Insert(3, ".").Insert(7, ".").Insert(11, "-");
        }
        #endregion

        #region FormatarTelefone
        public static String FormatarTelefone(string ddd, string telefone)
        {
            telefone = FormatarFone(telefone);

            if (string.IsNullOrWhiteSpace(ddd) && string.IsNullOrWhiteSpace(telefone))
                return string.Empty;

            return string.Format("({0}) {1}", ddd, telefone);
        }
        private static string FormatarFone(string fone)
        {
            string primeiro = string.Empty;
            string segundo = string.Empty;

            if (!string.IsNullOrWhiteSpace(fone))
            {
                //remove o que não for digito
                fone = new Regex(@"[^\d]").Replace(fone,string.Empty);
                if (fone.Length.Equals(8))
                {
                    primeiro = fone.Substring(0, 4);
                    segundo = fone.Substring(4, 4);
                }
                else if (fone.Length.Equals(9))
                {
                    primeiro = fone.Substring(0, 5);
                    segundo = fone.Substring(5, 4);
                }
            }

            if (string.IsNullOrWhiteSpace(primeiro) && string.IsNullOrWhiteSpace(segundo))
                return string.Empty;

            return string.Format("{0}-{1}", primeiro, segundo);
        }
        #endregion

        #region LimparMascaraTelefone
        /// <summary>
        /// Limpar os caracteres especiais do telefone
        /// </summary>
        /// <returns>Telefone Limpo</returns>
        public static string LimparMascaraTelefone(string numeroTelefone)
        {
            numeroTelefone = numeroTelefone.Trim();
            //return new Regex("[()-]\\s").Replace(numeroTelefone, string.Empty);
            return new String(numeroTelefone.Where(char.IsDigit).ToArray());
        }
        #endregion

        #region FormatarCidade
        public static String FormatarCidade(string nomeCidade, string siglaEstado)
        {
            return Common.Helpers.Formatting.FormatarCidade(nomeCidade, siglaEstado);
        }
        #endregion

        #region RetornarStream
        public static Stream RetornarStream(Object objRetorno, TipoRetorno tipoRetorno)
        {
            string retorno = string.Empty;

            if (WebOperationContext.Current != null)
            {
                switch (tipoRetorno)
                {
                    case TipoRetorno.XML:
                        WebOperationContext.Current.OutgoingResponse.ContentType = "application/xml; charset=utf-8";
                        retorno = ToXML(objRetorno);
                        break;
                    case TipoRetorno.JSON:
                        WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                        retorno = ToJSON(objRetorno);
                        break;
                    case TipoRetorno.Text:
                        WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain; charset=utf-8";
                        retorno = objRetorno.ToString();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("tipoRetorno");
                }
            }

            return new MemoryStream(Encoding.UTF8.GetBytes(retorno));
        }
        public enum TipoRetorno
        {
            XML,
            JSON,
            Text
        }
        #endregion

        #region AjustarString
        public static string AjustarString(string entrada)
        {
            //Remove espaços iniciais e finais.
            entrada = entrada.Trim();

            while (entrada.Contains("  "))
            {
                entrada = entrada.Replace("  ", " ");
            }

            //Coloca primeira letra para maiúscula.
            entrada = FormatarPrimeiraMaiscula(entrada);
            //removendo palavras auxiliares
            entrada = entrada.Replace(" De ", " de ");
            entrada = entrada.Replace(" Do ", " do ");
            entrada = entrada.Replace(" Dos ", " dos ");
            entrada = entrada.Replace(" Da ", " da ");
            entrada = entrada.Replace(" Para ", " para ");

            return entrada;
        }
        /// <summary>
        /// Método que formata a string para apenas a primeira letra maiúscula.
        /// </summary>
        /// <param name="entrada">Texto de entrada.</param>
        /// <returns>Texto formatado.</returns>
        private static string FormatarPrimeiraMaiscula(string entrada)
        {
            string[] palavras = entrada.Split(new[] { ' ' });

            entrada = palavras.Where(palavra => !palavra.Length.Equals(0)).Aggregate(string.Empty, (current, palavra) => current + (" " + palavra.Substring(0, 1).ToUpper() + palavra.Substring(1).ToLower()));

            if (entrada.Length.Equals(0))
                return string.Empty;

            return entrada.Substring(1);
        }
        #endregion

        #region NormalizarStringLINQ
        /// <summary>
        /// Normaliza o linq para efetuar buscas
        /// </summary>
        /// <param name="texto">Texto</param>
        /// <returns>Texto</returns>
        public static string NormalizarStringLINQ(this string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            return new string(texto.Replace('-', ' ').ToLowerInvariant().Normalize(NormalizationForm.FormD).Where(t => CharUnicodeInfo.GetUnicodeCategory(t) != UnicodeCategory.NonSpacingMark).ToArray()).Trim();
        }
        #endregion

        #region RemoverCaracteresBrancos
        /// <summary>
        /// Método que remove caracteres brancos em uma string
        /// </summary>
        /// <param name="texto">Texto</param>
        /// <returns>Retorna o texto tratado</returns>
        public static string RemoverCaracteresBrancos(string texto)
        {
            string retorno = texto;
            string caracterReplace;

            retorno = retorno.Replace(char.ConvertFromUtf32(8194), "");
            retorno = retorno.Replace(char.ConvertFromUtf32(8195), "");
            retorno = retorno.Replace(char.ConvertFromUtf32(8201), "");
            retorno = retorno.Replace(char.ConvertFromUtf32(8204), "");
            retorno = retorno.Replace(char.ConvertFromUtf32(8205), "");
            retorno = retorno.Replace(char.ConvertFromUtf32(8206), "");
            retorno = retorno.Replace(char.ConvertFromUtf32(8207), "");

            return retorno;
        }
        #endregion RemoverCaracteresBrancos

        #region RemoverAcentos
        /// <summary>
        /// Método que remove os acentos de uma string
        /// </summary>
        /// <param name="texto">Texto a ser removido a acentuação</param>
        /// <returns>String sem acentos</returns>
        public static string RemoverAcentos(string texto)
        {
            return Common.Helpers.Formatting.RemoverAcentos(texto);
        }
        #endregion

        #region ToXML
        /// <summary>
        /// Convert a coleção para XML
        /// </summary>
        /// <returns>A string xml que representa a coleção</returns>
        public static String ToXML(object anObject)
        {
            var serializer = new XmlSerializer(anObject.GetType());

            var settings = new XmlWriterSettings
            {
                Encoding = new UnicodeEncoding(false, false),
                Indent = false,
                OmitXmlDeclaration = true
            };

            using (var textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, anObject);
                }

                String res = textWriter.ToString();
                if (String.IsNullOrEmpty(res))
                    return String.Empty;

                return res;
            }
        }
        #endregion

        #region ToJSON
        /// <summary>
        /// Convert a coleção para JSON
        /// </summary>
        /// <returns>A string xml que representa a coleção</returns>
        public static string ToJSON(object anObject)
        {
            return Common.Helpers.JSON.ToJson(anObject);
        }
        #endregion

        #region ToBase64
        /// <summary>
        /// Convert a string para base64
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string ToBase64(string texto)
        {
            return Common.Helpers.Base64.ToBase64(texto);
        }
        #endregion

        #region FromBase64
        /// <summary>
        /// Convert a string para base64
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string FromBase64(string texto)
        {
            byte[] bytes = Convert.FromBase64String(texto);

            return Encoding.ASCII.GetString(bytes);
        }
        #endregion

        #region EfetuarRequisicao
        public static string EfetuarRequisicaoApplicationForm(string url, string dadosRequisicao, string userAgent)
        {
            // Create a request using a URL that can receive a post. 
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.UserAgent = !string.IsNullOrEmpty(userAgent) ? userAgent : "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.101 Safari/537.36";
            request.ContentType = "application/x-www-form-urlencoded";

            request.Proxy = WebRequest.DefaultWebProxy;
            request.Proxy.Credentials = CredentialCache.DefaultCredentials;

            byte[] byteArray = Encoding.UTF8.GetBytes(dadosRequisicao);
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            // Get the response.
            WebResponse response = request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("iso-8859-1"));
                return reader.ReadToEnd();
            }
        }
        #endregion

        #region EfetuarRequisicao
        public static string EfetuarRequisicao(string url, string dadosRequisicao)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(dadosRequisicao);

            var webRequest = WebRequest.Create(url);
            webRequest.Method = "POST";
            webRequest.ContentLength = buffer.Length;

            var postData = webRequest.GetRequestStream();
            postData.Write(buffer, 0, buffer.Length);
            postData.Close();

            var webResponse = webRequest.GetResponse();

            string retorno;
            using (var sr = new StreamReader(webResponse.GetResponseStream(), Encoding.GetEncoding("ISO-8859-1")))
            {
                retorno = sr.ReadToEnd();
            }

            return retorno;
        }
        #endregion

        #region EfetuarRequisicao
        public static string EfetuarRequisicao(string url, NameValueCollection parameters, string requestContentType, string requestHeader = null)
        {
            //Montando dado da requisição
            var sb = new StringBuilder();
            foreach (var key in parameters.AllKeys)
                sb.Append(key + "=" + parameters[key] + "&");

            if (sb.Length > 0)
                sb.Length = sb.Length - 1;

            byte[] buffer = Encoding.UTF8.GetBytes(sb.ToString());

            var webRequest = WebRequest.Create(url);
            webRequest.Method = "POST";
            webRequest.ContentLength = buffer.Length;

            if (!string.IsNullOrEmpty(requestContentType))
                webRequest.ContentType = requestContentType;

            if (!string.IsNullOrEmpty(requestHeader))
                webRequest.Headers.Add(requestHeader);

            using (var requestStream = webRequest.GetRequestStream())
            {
                requestStream.Write(buffer, 0, buffer.Length);
                requestStream.Close();
            }

            string retorno = string.Empty;
            Task<WebResponse> responseTask = Task.Factory.FromAsync<WebResponse>(webRequest.BeginGetResponse, webRequest.EndGetResponse, null);
            using (var responseStream = responseTask.Result.GetResponseStream())
            {
                retorno = new StreamReader(responseStream).ReadToEnd();
            }
            return retorno;
        }
        public static string EfetuarRequisicao(string url, string parameters, string requestContentType, string requestHeader = null)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(parameters);

            var webRequest = WebRequest.Create(url);
            webRequest.Method = "POST";
            webRequest.ContentLength = buffer.Length;

            if (!string.IsNullOrEmpty(requestContentType))
                webRequest.ContentType = requestContentType;

            if (!string.IsNullOrEmpty(requestHeader))
                webRequest.Headers.Add(requestHeader);

            using (var requestStream = webRequest.GetRequestStream())
            {
                requestStream.Write(buffer, 0, buffer.Length);
                requestStream.Close();
            }

            string retorno = string.Empty;
            Task<WebResponse> responseTask = Task.Factory.FromAsync<WebResponse>(webRequest.BeginGetResponse, webRequest.EndGetResponse, null);
            using (var responseStream = responseTask.Result.GetResponseStream())
            {
                retorno = new StreamReader(responseStream).ReadToEnd();
            }
            return retorno;
        }
        #endregion

        public static bool EstaNoAlcance<T>(T valorBase, T rangeCimaOuBaixo, T paraComparar)
        {
            dynamic valorBaseDinamico = valorBase;
            dynamic rangeDinamico = rangeCimaOuBaixo;
            dynamic paraCompararDinamico = paraComparar;

            if (valorBaseDinamico >= (paraCompararDinamico - rangeDinamico))
            {
                if (valorBaseDinamico <= (paraCompararDinamico + rangeDinamico))
                {
                    return true;
                }
            }

            return false;
        }

        #region RecuperarURLAmbiente
        /// <summary>
        /// Recupera a url do ambiente
        /// </summary>
        public static string RecuperarURLAmbiente()
        {
#if DEBUG
            return "localhost:2000";
#else
            return Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.URLAmbiente);
#endif
        }
        #endregion

        #region RecuperarURLVagas
        /// <summary>
        /// Recupera a url do ambiente
        /// </summary>
        public static string RecuperarURLVagas()
        {
#if DEBUG
            return "localhost:2500";
#else
            return Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.URLVagas);
#endif
        }
        #endregion

        #region RecuperarURLNovoVip
        /// <summary>
        /// Recupera a url do ambiente
        /// </summary>
        public static string RecuperarURLNovoVip()
        {
#if DEBUG
            return "localhost:3000";
#else   // to do
            throw new NotImplementedException();
#endif
        }
        #endregion

        #region RecuperarURLVagasEmpresa
        /// <summary>
        /// Recupera a url do ambiente
        /// </summary>
        public static string RecuperarURLVagasEmpresa(string nomeEmpresa, int? idFilial)
        {
            nomeEmpresa = nomeEmpresa.Replace("/", string.Empty);
            return string.Concat("http://", Helper.RecuperarURLVagas(), "/vagas-de-emprego-na-empresa-", nomeEmpresa.NormalizarURL(), "/", idFilial);
        }
        #endregion

        public static string GetVirtualPath(string physicalPath)
        {
            String applicationPath = System.Web.Hosting.HostingEnvironment.MapPath("~/");
            string url = physicalPath.Substring(applicationPath.Length).Replace('\\', '/').Insert(0, "~/");
            return (url);
        }

        #region RetornarDesricaoSalario
        public static string RetornarDesricaoSalario(decimal? valorSalarioDe, decimal? valorSalarioAte)
        {
            if (valorSalarioDe == null && valorSalarioAte == null)
                return "Salário a combinar";

            if (valorSalarioDe.Equals(valorSalarioAte))
                return string.Format("Salário de R$ {0}", ((decimal)valorSalarioDe).ToString("N2"));

            if (valorSalarioDe == null || valorSalarioAte == null)
            {
                return string.Format("Salário de R$ {0}", (valorSalarioDe != null ? ((decimal)valorSalarioDe).ToString("N2") : ((decimal)valorSalarioAte).ToString("N2")));
            }

            return string.Format(CultureInfo.CurrentCulture, "Salário de R$ {0} a R$ {1}", ((decimal)valorSalarioDe).ToString("N2"), ((decimal)valorSalarioAte).ToString("N2"));
        }
        #endregion

        #region RetornarDesricaoQuantidadeVaga
        public static string RetornarDesricaoQuantidadeVaga(int quantidadeVaga)
        {
            quantidadeVaga = quantidadeVaga <= 0 ? 1 : quantidadeVaga; //Evitar que seja impresso 0 Vagas

            if (quantidadeVaga.Equals(1))
                return string.Concat(quantidadeVaga, " vaga");

            return string.Concat(quantidadeVaga, " vagas");
        }
        #endregion

        public static string LimparCaracteresInvalidosEmXML(string text)
        {
            var validXmlChars = text.Where(ch => XmlConvert.IsXmlChar(ch)).ToArray();
            return new string(validXmlChars);
        }

        public static string StringParaTamanhoExato(String input, Int32 tamanho)
        {
            return input.PadRight(tamanho).Substring(0, tamanho);
        }

        public static string NumericoParaTamanhoExato(Int32 input, Int32 tamanho)
        {
            return input.ToString().PadLeft(tamanho, '0').Substring(0, tamanho);
        }

        public static string NumericoParaTamanhoExato(Decimal input, Int32 tamanho)
        {
            return input.ToString().PadLeft(tamanho, '0').Substring(0, tamanho);
        }

        public static string NumericoParaTamanhoExato(String input, Int32 tamanho)
        {
            return input.PadLeft(tamanho, '0').Substring(0, tamanho);
        }

        public static string MonetarioParaTamanhoExato(Decimal input, Int32 tamanho)
        {
            //Retira as casas decimais
            return (input * 100).ToString("F0").PadLeft(tamanho, '0').Substring(0, tamanho);
        }

        #region Validar Informações bancarias

        #region ValidacaoBancoDoBrasil
        public static bool ValidarContaBancoDoBrasil(string agencia, string conta)
        {
            #region ValidacaoAgencia
            string dvAgenciaBBOriginal = (agencia[agencia.Length - 1]).ToString();
            string agenciaBB = Helper.NumericoParaTamanhoExato(agencia.Substring(0, agencia.Length - 1), 4);
            int somatorioAgencia = 0;
            int countAg = 0;

            //PESO 5,4,3,2 -- Padrão Banco do Brasil
            for (int peso = 5; peso > 1; peso--)
            {
                somatorioAgencia += peso * (int)Char.GetNumericValue(agenciaBB[countAg]);
                countAg++;
            }

            //Calculo dvCalculado
            int dvAgenciaCalculado = 11 - (somatorioAgencia % 11);
            string dvAgenciaBBCalculcado = string.Empty;

            //Caso seja 10 ou 11 a subtração
            if (dvAgenciaCalculado.Equals(10)) dvAgenciaBBCalculcado = "X";
            else if (dvAgenciaCalculado.Equals(11)) dvAgenciaBBCalculcado = "0";
            else dvAgenciaBBCalculcado = dvAgenciaCalculado.ToString();

            if (!dvAgenciaBBOriginal.ToUpper().Equals(dvAgenciaBBCalculcado))
                return false;
            #endregion

            #region ValidacaoConta
            string dvContaBBOriginal = conta[conta.Length - 1].ToString();
            string contaBB = Helper.NumericoParaTamanhoExato(conta.Substring(0, conta.Length - 1), 8);
            int somatorioConta = 0;
            int countCt = 0;

            //PESO 5,4,3,2 -- Padrão Banco do Brasil
            for (int peso = 9; peso > 1; peso--)
            {
                somatorioConta += peso * (int)Char.GetNumericValue(contaBB[countCt]);
                countCt++;
            }

            //Calculo dvCalculado
            int dvContaCalculado = 11 - (somatorioConta % 11); ;
            string dvContaBBCalculcado = string.Empty;

            //Caso seja 10 ou 11 a subtração
            if (dvContaCalculado.Equals(10)) dvContaBBCalculcado = "X";
            else if (dvContaCalculado.Equals(11)) dvContaBBCalculcado = "0";
            else dvContaBBCalculcado = dvContaCalculado.ToString();

            if (!dvContaBBOriginal.ToUpper().Equals(dvContaBBCalculcado))
                return false;
            #endregion

            return true;
        }
        #endregion

        #region ValidacaoHSBC
        public static bool ValidarContaHSBC(string agencia, string conta)
        {
            int dvOriginal = (int)Char.GetNumericValue(conta[conta.Length - 1]);
            conta = conta.Substring(0, conta.Length - 1);
            string numeroConta = Helper.NumericoParaTamanhoExato(agencia, 4) + NumericoParaTamanhoExato(conta, 6);

            int peso = 8;
            int somatoria = 0;

            for (int i = 0; i < numeroConta.Length; i++)
            {
                somatoria += peso * (int)Char.GetNumericValue(numeroConta[i]);
                peso++;
                if (peso > 9)
                {
                    peso = 2;
                }
            }

            int dvCalculado = somatoria % 11;
            if (dvCalculado == 0 || dvCalculado >= 10)
            {
                dvCalculado = 0;
            }

            if (dvCalculado == dvOriginal)
            {
                return true;
            }

            return false;
        }
        #endregion

        #endregion

        /// <summary>
        /// Valida o número do cartão e reconhece a operadora a partir do número informado.
        /// </summary>
        /// <param name="numeroCartao">Número a ser validado.</param>
        /// <param name="operadoraCartao">Parametro de saída com a operadora do cartão. Null se não reconhecido.</param>
        /// <returns>True se o cartão foi validado com sucesso.</returns>
        public static bool ValidarCartaoCredito(string numeroCartao, out Enumeradores.Operadora? operadoraCartao)
        {
            var isValid = false;
            string cardNumbersOnly = numeroCartao.Replace(" ", "");
            int cardNumberLength = cardNumbersOnly.Length;
            isValid = !Regex.IsMatch(numeroCartao, "[^0-9 ]");
            operadoraCartao = null;
            if (isValid)
            {
                Regex prefixRegExpMastercard = new Regex(@"^5[1-5][0-9][0-9]");
                Regex prefixRegExpVisa = new Regex(@"^4(?!38935|011|51416|576)\d{12}(?:\d{3})?$");
                Regex prefixRegExpAmex = new Regex(@"^3[47]");
                Regex prefixRegExpDinners = new Regex(@"^3(?:0[0-5]|[68][0-9])");
                Regex prefixRegExpAura = new Regex(@"^50[0-9]");
                Regex prefixRegExpJCB = new Regex(@"^(?:2131|1800|35\d{3})\d{11}");
                Regex prefixRegExpElo = new Regex(@"^401178|^401179|^431274|^438935|^451416|^457393|^457631|^457632|^504175|^627780|^636297|^636368|^(506699|5067[0-6]\d|50677[0-8])|^(50900\d|5090[1-9]\d|509[1-9]\d{2})|^65003[1-3]|^(65003[5-9]|65004\d|65005[0-1])|^(65040[5-9]|6504[1-3]\d)|^(65048[5-9]|65049\d|6505[0-2]\d|65053     [0-8])|^(65054[1-9]|6505[5-8]\d|65059[0-8])|^(65070\d|65071[0-8])|^65072[0-7]|^(65090[1-9]|65091\d|650920)|^(65165[2-9]|6516[6-7]\d)|^(65500\d|65501\d)|^(65502[1-9]|6550[3-4]\d|65505[0-8])");
                Regex prefixRegExpDiscover = new Regex(@"^6(?:011|5[0-9]{2})");

                if (prefixRegExpMastercard.IsMatch(cardNumbersOnly))
                {
                    operadoraCartao = Enumeradores.Operadora.Master;
                }

                if (prefixRegExpVisa.IsMatch(cardNumbersOnly))
                {
                    operadoraCartao = Enumeradores.Operadora.Visa;
                }

                if (prefixRegExpAmex.IsMatch(cardNumbersOnly))
                {
                    operadoraCartao = Enumeradores.Operadora.Amex;
                }

                if (prefixRegExpDinners.IsMatch(cardNumbersOnly))
                {
                    operadoraCartao = Enumeradores.Operadora.Dinners;
                }
                if (prefixRegExpAura.IsMatch(cardNumbersOnly))
                {
                    operadoraCartao = Enumeradores.Operadora.Aura;
                }

                if (prefixRegExpJCB.IsMatch(cardNumbersOnly))
                {
                    operadoraCartao = Enumeradores.Operadora.JCB;
                }

                if (prefixRegExpElo.IsMatch(cardNumbersOnly))
                {
                    operadoraCartao = Enumeradores.Operadora.ELO;
                }
                if (prefixRegExpDiscover.IsMatch(cardNumbersOnly))
                {
                    operadoraCartao = Enumeradores.Operadora.Discover;
                }

                if (operadoraCartao == null)
                    return false;

                switch (operadoraCartao)
                {
                    case BNE.BLL.Enumeradores.Operadora.Visa:
                        isValid = (cardNumberLength == 16 || cardNumberLength == 13);
                        break;
                    case BNE.BLL.Enumeradores.Operadora.Master:
                        isValid = (cardNumberLength == 16);
                        break;
                    case BNE.BLL.Enumeradores.Operadora.Dinners:
                        isValid = (cardNumberLength == 14 || cardNumberLength == 16);
                        break;
                    case BNE.BLL.Enumeradores.Operadora.Amex:
                        isValid = (cardNumberLength == 15);
                        break;
                    case BNE.BLL.Enumeradores.Operadora.Aura:
                        isValid = (cardNumberLength == 16);
                        break;
                    case BNE.BLL.Enumeradores.Operadora.JCB:
                        isValid = (cardNumberLength == 16);
                        break;
                    case BNE.BLL.Enumeradores.Operadora.ELO:
                        isValid = (cardNumberLength == 16);
                        break;
                    case BNE.BLL.Enumeradores.Operadora.Discover:
                        isValid = (cardNumberLength == 16);
                        break;
                    default:
                        //Operadora não reconhecida
                        return false;
                }
            }
            if (isValid)
            {
                double numberProduct;
                double checkSumTotal = 0;
                int digitCounter;
                for (digitCounter = cardNumberLength - 1; digitCounter >= 0; digitCounter--)
                {
                    checkSumTotal += Char.GetNumericValue(cardNumbersOnly[digitCounter]);
                    digitCounter--;
                    numberProduct = Char.GetNumericValue(cardNumbersOnly[digitCounter]) * 2;
                    for (var productDigitCounter = 0; productDigitCounter < numberProduct.ToString().Length; productDigitCounter++)
                    {
                        checkSumTotal += Char.GetNumericValue(numberProduct.ToString()[productDigitCounter]);
                    }
                }
                isValid = (checkSumTotal % 10 == 0);
            }

            return isValid;
        }

        #region CalcularTempoEmprego

        /// <summary>
        /// Calcular o tempo de empresa de uma experiência Profissional do Candidato
        /// </summary>
        /// <param name="DtaAdmissao"></param>
        /// <param name="DtaDemissao"></param>
        /// <returns>string com o tempo de empresa</returns>
        public static string CalcularTempoEmprego(string DtaAdmissao, string DtaDemissao)
        {
            string retorno = string.Empty;
            string strSeparadorMes = string.Empty;
            DateTime dtaAdmissao = (!string.IsNullOrEmpty(DtaAdmissao) ? Convert.ToDateTime(DtaAdmissao) : DateTime.Now);
            DateTime dtaDemissao = (!string.IsNullOrEmpty(DtaDemissao) ? Convert.ToDateTime(DtaDemissao) : DateTime.Now);

            TimeSpan ts = dtaDemissao.Subtract(dtaAdmissao);
            DateTime periodo = new DateTime((ts.Ticks > 0 ? ts.Ticks : (ts.Ticks) * -1));

            string strAno = (periodo.Year - 1 > 0 ? (periodo.Year - 1 == 1 ? string.Format("{0} ano", periodo.Year - 1) : string.Format("{0} anos", periodo.Year - 1)) : "");
            string strMes = (periodo.Month - 1 > 0 ? (periodo.Month - 1 == 1 ? string.Format("{0} mês", periodo.Month - 1) : string.Format("{0} meses", periodo.Month - 1)) : "");

            if (strAno != "" && strMes != "")
            {
                strSeparadorMes = " e ";
            }

            return retorno = string.Format("{0}{1}{2}", strAno, strSeparadorMes, strMes);
        }
        #endregion

        #region RetornarMesExtenso
        /// <summary>
        /// Retorna o mês por extendo a partir no número do mês.
        /// </summary>
        /// <param name="idfMes"></param>
        /// <returns></returns>
        public static string RetornarMesExtenso(int idfMes)
        {
            string mesExtenso = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(idfMes).ToLower();
            mesExtenso = char.ToUpper(mesExtenso[0]) + mesExtenso.Substring(1);
            return mesExtenso;
        }

        #endregion

        #region CalcularIdade
        /// <summary>
        /// Calcular idade
        /// </summary>
        /// <returns>int com a quantidade de anos de idade</returns>
        public static int CalcularIdade(DateTime dataNascimento)
        {
            try
            {
                TimeSpan ts = DateTime.Today - dataNascimento;
                return new DateTime(ts.Ticks).Year - 1;
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region Detecta Cidade
        public static Cidade DetectaCidade(string nomeCidade)
        {
            try
            {
                Cidade oCidade;
                Cidade.CarregarPorNome(nomeCidade, out oCidade);

                if (oCidade != null)
                    return oCidade;


                //Caso nao tenha sucesso na primeira tentativa, faz a busca da cidade de forma diferente
                string cidade = "";
                string estado = "";

                string[] arrTeste = new string[2];

                nomeCidade = nomeCidade.Replace(", ", "/");
                nomeCidade = nomeCidade.Replace(" - ", "/");
                nomeCidade = nomeCidade.Replace("-", "/");
                nomeCidade = nomeCidade.Replace("’", "'");
                arrTeste = nomeCidade.Split('/');

                if (arrTeste.Count() != 2)
                    return null;

                //Verificar se é sigla ou nome, para verificar qual tipo de busca faz
                cidade = arrTeste[0];
                estado = arrTeste[1];

                //Verificar se é sigla ou nome, para verificar qual tipo de busca faz
                if (estado.Length == 2) //Recebeu a sigla do estado
                    Cidade.CarregarPorNomeCidadeSiglaEstado(cidade, estado, out oCidade);
                else                    //Recebeu o nome do estado
                    Cidade.CarregarPorNomeCidadeEstado(cidade, estado, out oCidade);

                return oCidade;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion DetectaCidade

        #region TratarPesquisaLogradouro
        /// <summary>
        /// Tratar a string de pesquisa por logradouro
        /// Remover os tipos de logradouro (rua, avenida,
        /// </summary>
        /// <param name="logradouro"></param>
        /// <returns></returns>
        public static string TratarPesquisaLogradouro(string logradouro)
        {
            if (logradouro != "")
            {
                string[] tipos = new string[] { "Rua", "Avenida", "av", "av.", "Travessa", "Quadra", "Praça", "Alameda", "Beco", "Vila", "Estrada", "Rodovia" };

                for (int i = 0; i <= tipos.Length - 1; i++)
                {
                    logradouro = logradouro.ToLower().Replace(tipos[i].ToLower(), "");
                }
            }

            return logradouro.Trim();
        }
        #endregion

        #region TratarUrlOrigem
        /// <summary>
        /// Tratar Url de string para URI
        /// </summary>
        /// <param name="urlOrigem"></param>
        /// <returns>Array com os parametros</returns>
        public static string[] TratarUrlOrigem(string urlOrigem)
        {
            try
            {
                if (string.IsNullOrEmpty(urlOrigem))
                    return null;

                Uri uri = new Uri(urlOrigem);
                string[] retorno = new string[] { uri.Host, uri.PathAndQuery };

                return retorno;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro ao Tratar Url Origem");
                return null;
            }

        }
        #endregion

        #region GetMIMEType
        private static readonly Dictionary<string, string> MIMETypesDictionary = new Dictionary<string, string>
        {
            {"ai", "application/postscript"},
            {"aif", "audio/x-aiff"},
            {"aifc", "audio/x-aiff"},
            {"aiff", "audio/x-aiff"},
            {"asc", "text/plain"},
            {"atom", "application/atom+xml"},
            {"au", "audio/basic"},
            {"avi", "video/x-msvideo"},
            {"bcpio", "application/x-bcpio"},
            {"bin", "application/octet-stream"},
            {"bmp", "image/bmp"},
            {"cdf", "application/x-netcdf"},
            {"cgm", "image/cgm"},
            {"class", "application/octet-stream"},
            {"cpio", "application/x-cpio"},
            {"cpt", "application/mac-compactpro"},
            {"csh", "application/x-csh"},
            {"css", "text/css"},
            {"dcr", "application/x-director"},
            {"dif", "video/x-dv"},
            {"dir", "application/x-director"},
            {"djv", "image/vnd.djvu"},
            {"djvu", "image/vnd.djvu"},
            {"dll", "application/octet-stream"},
            {"dmg", "application/octet-stream"},
            {"dms", "application/octet-stream"},
            {"doc", "application/msword"},
            {"docx","application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
            {"dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template"},
            {"docm","application/vnd.ms-word.document.macroEnabled.12"},
            {"dotm","application/vnd.ms-word.template.macroEnabled.12"},
            {"dtd", "application/xml-dtd"},
            {"dv", "video/x-dv"},
            {"dvi", "application/x-dvi"},
            {"dxr", "application/x-director"},
            {"eps", "application/postscript"},
            {"etx", "text/x-setext"},
            {"exe", "application/octet-stream"},
            {"ez", "application/andrew-inset"},
            {"gif", "image/gif"},
            {"gram", "application/srgs"},
            {"grxml", "application/srgs+xml"},
            {"gtar", "application/x-gtar"},
            {"hdf", "application/x-hdf"},
            {"hqx", "application/mac-binhex40"},
            {"htm", "text/html"},
            {"html", "text/html"},
            {"ice", "x-conference/x-cooltalk"},
            {"ico", "image/x-icon"},
            {"ics", "text/calendar"},
            {"ief", "image/ief"},
            {"ifb", "text/calendar"},
            {"iges", "model/iges"},
            {"igs", "model/iges"},
            {"jnlp", "application/x-java-jnlp-file"},
            {"jp2", "image/jp2"},
            {"jpe", "image/jpeg"},
            {"jpeg", "image/jpeg"},
            {"jpg", "image/jpeg"},
            {"js", "application/x-javascript"},
            {"kar", "audio/midi"},
            {"latex", "application/x-latex"},
            {"lha", "application/octet-stream"},
            {"lzh", "application/octet-stream"},
            {"m3u", "audio/x-mpegurl"},
            {"m4a", "audio/mp4a-latm"},
            {"m4b", "audio/mp4a-latm"},
            {"m4p", "audio/mp4a-latm"},
            {"m4u", "video/vnd.mpegurl"},
            {"m4v", "video/x-m4v"},
            {"mac", "image/x-macpaint"},
            {"man", "application/x-troff-man"},
            {"mathml", "application/mathml+xml"},
            {"me", "application/x-troff-me"},
            {"mesh", "model/mesh"},
            {"mid", "audio/midi"},
            {"midi", "audio/midi"},
            {"mif", "application/vnd.mif"},
            {"mov", "video/quicktime"},
            {"movie", "video/x-sgi-movie"},
            {"mp2", "audio/mpeg"},
            {"mp3", "audio/mpeg"},
            {"mp4", "video/mp4"},
            {"mpe", "video/mpeg"},
            {"mpeg", "video/mpeg"},
            {"mpg", "video/mpeg"},
            {"mpga", "audio/mpeg"},
            {"ms", "application/x-troff-ms"},
            {"msh", "model/mesh"},
            {"mxu", "video/vnd.mpegurl"},
            {"nc", "application/x-netcdf"},
            {"oda", "application/oda"},
            {"ogg", "application/ogg"},
            {"pbm", "image/x-portable-bitmap"},
            {"pct", "image/pict"},
            {"pdb", "chemical/x-pdb"},
            {"pdf", "application/pdf"},
            {"pgm", "image/x-portable-graymap"},
            {"pgn", "application/x-chess-pgn"},
            {"pic", "image/pict"},
            {"pict", "image/pict"},
            {"png", "image/png"},
            {"pnm", "image/x-portable-anymap"},
            {"pnt", "image/x-macpaint"},
            {"pntg", "image/x-macpaint"},
            {"ppm", "image/x-portable-pixmap"},
            {"ppt", "application/vnd.ms-powerpoint"},
            {"pptx","application/vnd.openxmlformats-officedocument.presentationml.presentation"},
            {"potx","application/vnd.openxmlformats-officedocument.presentationml.template"},
            {"ppsx","application/vnd.openxmlformats-officedocument.presentationml.slideshow"},
            {"ppam","application/vnd.ms-powerpoint.addin.macroEnabled.12"},
            {"pptm","application/vnd.ms-powerpoint.presentation.macroEnabled.12"},
            {"potm","application/vnd.ms-powerpoint.template.macroEnabled.12"},
            {"ppsm","application/vnd.ms-powerpoint.slideshow.macroEnabled.12"},
            {"ps", "application/postscript"},
            {"qt", "video/quicktime"},
            {"qti", "image/x-quicktime"},
            {"qtif", "image/x-quicktime"},
            {"ra", "audio/x-pn-realaudio"},
            {"ram", "audio/x-pn-realaudio"},
            {"ras", "image/x-cmu-raster"},
            {"rdf", "application/rdf+xml"},
            {"rgb", "image/x-rgb"},
            {"rm", "application/vnd.rn-realmedia"},
            {"roff", "application/x-troff"},
            {"rtf", "text/rtf"},
            {"rtx", "text/richtext"},
            {"sgm", "text/sgml"},
            {"sgml", "text/sgml"},
            {"sh", "application/x-sh"},
            {"shar", "application/x-shar"},
            {"silo", "model/mesh"},
            {"sit", "application/x-stuffit"},
            {"skd", "application/x-koan"},
            {"skm", "application/x-koan"},
            {"skp", "application/x-koan"},
            {"skt", "application/x-koan"},
            {"smi", "application/smil"},
            {"smil", "application/smil"},
            {"snd", "audio/basic"},
            {"so", "application/octet-stream"},
            {"spl", "application/x-futuresplash"},
            {"src", "application/x-wais-source"},
            {"sv4cpio", "application/x-sv4cpio"},
            {"sv4crc", "application/x-sv4crc"},
            {"svg", "image/svg+xml"},
            {"swf", "application/x-shockwave-flash"},
            {"t", "application/x-troff"},
            {"tar", "application/x-tar"},
            {"tcl", "application/x-tcl"},
            {"tex", "application/x-tex"},
            {"texi", "application/x-texinfo"},
            {"texinfo", "application/x-texinfo"},
            {"tif", "image/tiff"},
            {"tiff", "image/tiff"},
            {"tr", "application/x-troff"},
            {"tsv", "text/tab-separated-values"},
            {"txt", "text/plain"},
            {"ustar", "application/x-ustar"},
            {"vcd", "application/x-cdlink"},
            {"vrml", "model/vrml"},
            {"vxml", "application/voicexml+xml"},
            {"wav", "audio/x-wav"},
            {"wbmp", "image/vnd.wap.wbmp"},
            {"wbmxl", "application/vnd.wap.wbxml"},
            {"wml", "text/vnd.wap.wml"},
            {"wmlc", "application/vnd.wap.wmlc"},
            {"wmls", "text/vnd.wap.wmlscript"},
            {"wmlsc", "application/vnd.wap.wmlscriptc"},
            {"wrl", "model/vrml"},
            {"xbm", "image/x-xbitmap"},
            {"xht", "application/xhtml+xml"},
            {"xhtml", "application/xhtml+xml"},
            {"xls", "application/vnd.ms-excel"},
            {"xml", "application/xml"},
            {"xpm", "image/x-xpixmap"},
            {"xsl", "application/xml"},
            {"xlsx","application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
            {"xltx","application/vnd.openxmlformats-officedocument.spreadsheetml.template"},
            {"xlsm","application/vnd.ms-excel.sheet.macroEnabled.12"},
            {"xltm","application/vnd.ms-excel.template.macroEnabled.12"},
            {"xlam","application/vnd.ms-excel.addin.macroEnabled.12"},
            {"xlsb","application/vnd.ms-excel.sheet.binary.macroEnabled.12"},
            {"xslt", "application/xslt+xml"},
            {"xul", "application/vnd.mozilla.xul+xml"},
            {"xwd", "image/x-xwindowdump"},
            {"xyz", "chemical/x-xyz"},
            {"zip", "application/zip"}
        };

        /// <summary>
        /// Obtém o Mime Type baseado na extensão do arquivo
        /// </summary>
        /// <param name="fileName">Nome do arquivo</param>
        /// <returns>Mime type detectado ou "unknown/unknown" se não reconhecido</returns>
        public static string GetMIMEType(string fileName)
        {
            //get file extension
            string extension = Path.GetExtension(fileName).ToLowerInvariant();

            if (extension.Length > 0 &&
                MIMETypesDictionary.ContainsKey(extension.Remove(0, 1)))
            {
                return MIMETypesDictionary[extension.Remove(0, 1)];
            }
            return "unknown/unknown";
        }

        #endregion GetMIMEType

        #region Criptografia
        /// <summary>
        /// Criptografa uma string de acordo com a chave inserida no banco
        /// </summary>
        /// <param name="txtCriptografa"></param>
        /// <returns></returns>
        public static string Criptografa(string txtCriptografa)
        {
            string salt = System.Configuration.ConfigurationManager.AppSettings["ChaveCriptografia"];

            byte[] utfData = UTF8Encoding.UTF8.GetBytes(txtCriptografa);

            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

            string encryptedString = string.Empty;

            using (AesManaged aes = new AesManaged())
            {

                Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(salt, saltBytes);

                aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;

                aes.KeySize = aes.LegalKeySizes[0].MaxSize;

                aes.Key = rfc.GetBytes(aes.KeySize / 8);

                aes.IV = rfc.GetBytes(aes.BlockSize / 8);

                using (ICryptoTransform encryptTransform = aes.CreateEncryptor())
                {

                    using (MemoryStream encryptedStream = new MemoryStream())
                    {

                        using (CryptoStream encryptor = new CryptoStream(encryptedStream, encryptTransform, CryptoStreamMode.Write))
                        {

                            encryptor.Write(utfData, 0, utfData.Length);

                            encryptor.Flush();

                            encryptor.Close();

                            byte[] encryptBytes = encryptedStream.ToArray();

                            encryptedString = Convert.ToBase64String(encryptBytes);

                        }

                    }

                }

            }

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(encryptedString);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        /// <summary>
        /// Descriptografa uma string de acordo com a chave inserida no banco
        /// </summary>
        /// <param name="txtDesCriptografa"></param>
        /// <returns></returns>
        public static string Descriptografa(string txtDesCriptografa)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(txtDesCriptografa);
            txtDesCriptografa = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

            string Salt = System.Configuration.ConfigurationManager.AppSettings["ChaveCriptografia"];
            // string se = WebUtility.HtmlDecode();
            byte[] encryptedBytes = Convert.FromBase64String(txtDesCriptografa.Replace(" ", "+"));
            byte[] saltBytes = System.Text.Encoding.UTF8.GetBytes(Salt);
            string decryptedString = string.Empty;

            using (var aes = new AesManaged())
            {

                Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(Salt, saltBytes);

                aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;

                aes.KeySize = aes.LegalKeySizes[0].MaxSize;

                aes.Key = rfc.GetBytes(aes.KeySize / 8);

                aes.IV = rfc.GetBytes(aes.BlockSize / 8);

                using (ICryptoTransform decryptTransform = aes.CreateDecryptor())
                {

                    using (MemoryStream decryptedStream = new MemoryStream())
                    {

                        CryptoStream decryptor = new CryptoStream(decryptedStream, decryptTransform, CryptoStreamMode.Write);

                        decryptor.Write(encryptedBytes, 0, encryptedBytes.Length);

                        decryptor.Flush();

                        decryptor.Close();

                        byte[] decryptBytes = decryptedStream.ToArray();

                        decryptedString = UTF8Encoding.UTF8.GetString(decryptBytes, 0, decryptBytes.Length);

                    }

                }

            }
            return decryptedString;
        }

        #endregion

        #region RetornarPrimeiroNome
        /// <param name="nomeCompleto">String nome completo</param>
        /// <returns>Primeiro nome</returns>
        public static string RetornarPrimeiroNome(string nomeCompleto)
        {
            return Common.Helpers.Formatting.RetornarPrimeiroNome(nomeCompleto);
        }
        #endregion

        #region EncurtarUrl
        /// <summary>
        /// Encurta URL
        /// </summary>
        /// <param name="url">Url a ser encurtada</param>
        /// <returns>url</returns>
        public static string EncurtarUrl(string url)
        {
            string encurtaLink = "https://www.googleapis.com/urlshortener/v1/url?key=AIzaSyARwQouAe4kELvd8w-rn4U72-gG4AIcal0";
            string result = string.Empty;
            string urlShort = string.Empty;

            try
            {
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    result = client.UploadString(encurtaLink, "POST", "{'longUrl':'" + url + "'}");

                    dynamic urlDeserialize = Newtonsoft.Json.JsonConvert.DeserializeObject(result);

                    if (urlDeserialize.id.Value != null)
                        urlShort = urlDeserialize.id.Value;
                }


            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro ao encurtar URL");
            }

            return urlShort;

        }
        #endregion
    }
}
