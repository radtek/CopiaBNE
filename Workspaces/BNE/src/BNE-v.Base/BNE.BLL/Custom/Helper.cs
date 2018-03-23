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
                fone = fone.Trim();

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
            if (string.IsNullOrEmpty(nomeCidade))
                return string.Empty;

            if (string.IsNullOrEmpty(siglaEstado))
                return nomeCidade;

            return string.Format("{0}/{1}", nomeCidade, siglaEstado);
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
            return new string(texto.Replace('-',' ').ToLowerInvariant().Normalize(NormalizationForm.FormD).Where(t => CharUnicodeInfo.GetUnicodeCategory(t) != UnicodeCategory.NonSpacingMark).ToArray());
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
            string s = texto.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (char t in s)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(t);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(t);
                }
            }
            return sb.ToString();
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
        public static String ToJSON(object anObject)
        {
            String res = JsonConvert.SerializeObject(anObject, new IsoDateTimeConverter());
            if (String.IsNullOrEmpty(res))
                return String.Empty;

            return res;
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
            byte[] bytes = Encoding.ASCII.GetBytes(texto);

            return Convert.ToBase64String(bytes);
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
            for (int peso = 5; peso > 1; peso--){
                somatorioAgencia += peso * (int)Char.GetNumericValue(agenciaBB[countAg]);
                countAg++;
            }

            //Calculo dvCalculado
            int dvAgenciaCalculado = 11-(somatorioAgencia % 11);
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
            if (isValid){
                Regex prefixRegExpMastercard = new Regex("^5[1-5]");
                Regex prefixRegExpVisa = new Regex("^4");
                Regex prefixRegExpAmex = new Regex("^3(4|7)");
                Regex prefixRegExpDinners = new Regex("^(30[1-5])|(2014)|(2149)|(36)");

                if (prefixRegExpMastercard.IsMatch(cardNumbersOnly))
                {
                    operadoraCartao = Enumeradores.Operadora.Master;
                }else
                if (prefixRegExpVisa.IsMatch(cardNumbersOnly))
                {
                    operadoraCartao = Enumeradores.Operadora.Visa;
                }else
                if (prefixRegExpAmex.IsMatch(cardNumbersOnly))
                {
                    operadoraCartao = Enumeradores.Operadora.Amex;
                }else
                if (prefixRegExpDinners.IsMatch(cardNumbersOnly))
                {
                    operadoraCartao = Enumeradores.Operadora.Dinners;
                }else{
                    //Operadora não reconhecida
                    return false;
                }

                switch (operadoraCartao)
                {
                    case BNE.BLL.Enumeradores.Operadora.Visa:
                        isValid = (cardNumberLength == 16);
                        break;
                    case BNE.BLL.Enumeradores.Operadora.Master:
                        isValid = (cardNumberLength == 16 || cardNumberLength == 13);
                        break;
                    case BNE.BLL.Enumeradores.Operadora.Dinners:
                        isValid = (cardNumberLength == 14);
                        break;
                    case BNE.BLL.Enumeradores.Operadora.Amex:
                        isValid = (cardNumberLength == 15);
                        break;
                    default:
                        //Operadora não reconhecida
            return false;
                        break;
                }
            }
            if (isValid){
                double numberProduct;
                double checkSumTotal = 0;
                int digitCounter;
                for (digitCounter = cardNumberLength - 1; digitCounter >= 0; digitCounter--){
                    checkSumTotal += Char.GetNumericValue(cardNumbersOnly[digitCounter]);
                    digitCounter--;
                    numberProduct =  Char.GetNumericValue(cardNumbersOnly[digitCounter]) * 2;
                    for (var productDigitCounter = 0; productDigitCounter < numberProduct.ToString().Length; productDigitCounter++){
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
            DateTime periodo = new DateTime((ts.Ticks > 0 ? ts.Ticks: (ts.Ticks)*-1));

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

    }
}
