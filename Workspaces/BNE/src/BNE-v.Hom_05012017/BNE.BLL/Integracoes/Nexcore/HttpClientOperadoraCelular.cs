using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

namespace BNE.BLL.Integracoes.Nexcore
{
    public class HttpClientOperadoraCelular
    {
        #region Campos e constantes

        private WebRequest request;
#if DEBUG
        private static readonly string URI = "http://10.1.0.15/ncall/servicos/getop.php?magic=1333&nums={0}";
#else
        private static readonly string URI = "http://" + 
            Parametro.RecuperaValorParametro(Enumeradores.Parametro.IPNexcoreOperadoraCelular) + 
            "/ncall/servicos/getop.php?magic=1333&nums={0}";
#endif
        private static readonly Encoding ENCODING = Encoding.GetEncoding("iso-8859-1");
        private const string PATTERN = @"^numero {0}::([^:]+)::(\d{{1,2}})::(P|nP)$";
        private const int TIMEOUT = 60000;  // 1 minuto

        #endregion

        #region Construtores
        public HttpClientOperadoraCelular()
        {
            ServicePointManager.DefaultConnectionLimit = 100;
            ServicePointManager.Expect100Continue = false;
        }
        #endregion

        #region Métodos

        #region ObterResponse
        private bool ObterResponse(out string[] response, params string[] celulares)
        {
            string separadoPorVirgulas = String.Join(",", celulares);

            string uri = String.Format(URI, separadoPorVirgulas);

            string rawResponse = GetResponse(uri);

            response =
                rawResponse
                    .Split('\n')
                    .Where(operadora => !String.IsNullOrWhiteSpace(operadora))  // remove o vazio após o último celular do vetor
                    .ToArray();

            // se a qtde de celulares não for igual a qtde de operadoras, provavelmente ocorreu um erro sério no webservice; abortar tudo
            return celulares.Length == response.Length;
        }
        #endregion

        #region GetNomeOperadora
        public bool GetNomeOperadora(out string nomeOperadora, string dddNumero)
        {
            nomeOperadora = String.Empty; 
            
            string[] arrayRawOperadoras;
            if (!ObterResponse(out arrayRawOperadoras, dddNumero))
                return false;

            string rawOperadora = arrayRawOperadoras.First();
            string pattern = String.Format(PATTERN, dddNumero);

            if (Regex.IsMatch(rawOperadora, pattern))
                nomeOperadora = Regex.Replace(rawOperadora, pattern, "$1");

            return true;
        }
        #endregion

        #region GetOperadoraCelular
        public bool GetOperadoraCelular(out OperadoraCelular objOperadoraCelular, string dddNumero)
        {
            string[] operadora;
            objOperadoraCelular = null;

            if (GetListaOperadoras(out operadora, dddNumero))
            {
                int id = Convert.ToInt32(operadora.First());

                try
                {
                    objOperadoraCelular = OperadoraCelular.LoadObject(id);
                    return true;
                }
                catch (EL.RecordNotFoundException) 
                {
                    // cria e salva nova operadora
                    string nomeOperadora;
                    if (GetNomeOperadora(out nomeOperadora, dddNumero))
                    {
                        objOperadoraCelular = new OperadoraCelular()
                        {
                            IdOperadoraCelular = id,
                            FlagInativo = false,
                            NomeOperadoraCelular = nomeOperadora
                        };
                        objOperadoraCelular.Save();
                    }
                }
            }

            return false;
        }
        #endregion

        #region GetListaOperadoras
        public bool GetListaOperadoras(out string[] arrayOperadoras, params string[] arrayCelulares)
        {
            if (!ObterResponse(out arrayOperadoras, arrayCelulares))
                return false;

            for (int i = 0; i < arrayCelulares.Length; ++i)
            {
                string operadora = arrayOperadoras[i];
                string celular = arrayCelulares[i];
                string pattern = String.Format(PATTERN, celular);

                if (Regex.IsMatch(operadora, pattern))
                    arrayOperadoras[i] = Regex.Replace(operadora, pattern, "$2");
                else
                    // provavelmente houve erro na consulta do celular; retornar NULL ("0" significa NULL, no caso)
                    arrayOperadoras[i] = "0";
            }

            return true;
        }
        #endregion

        #region GetResponse
        private string GetResponse(string uri)
        {
            string retorno = null;

            request = WebRequest.Create(uri);
            request.Method = "GET";
            request.Timeout = TIMEOUT;

            using (WebResponse response = request.GetResponse()) // pode dar exceção de timeout WebException
            {
                using (Stream stream = response.GetResponseStream()) 
                {
                    using (StreamReader sr = new StreamReader(stream, ENCODING))
                    {
                        retorno = sr.ReadToEnd();
                    }
                }
            }

            return retorno;
        }
        #endregion

        #endregion
    }
}
