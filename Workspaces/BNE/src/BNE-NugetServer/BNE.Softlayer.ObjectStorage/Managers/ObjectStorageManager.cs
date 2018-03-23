using BNE.StorageManager.Config;
using BNE.StorageManager.Config.ObjectStorage;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BNE.StorageManager.Managers
{
    public class ObjectStorageManager : IFileManager, IFileConfig
    {
        private string _url;
        private string _account;
        private string _container;
        private string _containerUrl;
        private string _user;
        private string _apiKey;
        private string _folder;
        private Regex _regexToPath;
        internal static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void LoadConfig(StorageConfiguration storage)
        {
            try
            {
                StoragesConfigSection config = System.Configuration.ConfigurationManager.GetSection("StoragesConfig") as StoragesConfigSection;

                ObjectStorageConfiguration osStorage = config.Storages[storage.StorageName];

                _url = osStorage.Url;
                _account = osStorage.Account;
                _container = osStorage.Container;
                _containerUrl = Helper.CombineUrl(osStorage.Url, "v1", osStorage.Account, osStorage.Container);
                _user = osStorage.User;
                _apiKey = osStorage.ApiKey;
                _folder = storage.StorageFolder;

                if (!String.IsNullOrEmpty(storage.RegexPath))
                {
                    _regexToPath = new Regex(storage.RegexPath);
                }
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException("Ocorreu uma falha ao tentar configurar o Object Storage", ex);
            }
        }

        /// <summary>
        /// Cria ou substitui um objeto no Object Storage
        /// </summary>
        /// <param name="objectCompletePath">Nome do arquivo com o caminho relativo ao container</param>
        /// <param name="byteArray">Array de bytes do objeto</param>
        /// <returns>Verdadeiro em caso de sucesso</returns>
        /// <exception cref="Exception">Exceção retornada caso o arquivo não possa ser salvo</exception>
        public bool Save(String objectCompletePath, byte[] byteArray)
        {
            objectCompletePath = ResolvePath(objectCompletePath);

            HttpWebRequest request = GetRequest(objectCompletePath);

            request.Method = "PUT";
            request.ContentLength = byteArray.Length;
            request.ContentType = Helper.GetMIMEType(objectCompletePath);
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            HttpWebResponse response = EfetuarRequisicao(request);

            if (response.StatusCode != HttpStatusCode.Created)
                throw new Exception("Não foi possível criar o objeto no storage: " + request.RequestUri);

            response.Close();
            return true;
        }

        /// <summary>
        /// Apaga um objeto no Object Storage
        /// </summary>
        /// <param name="objectPath">Caminho do objeto a ser excluído</param>
        /// <returns>Verdadeiro em caso de sucesso</returns>
        /// <exception cref="FileNotFoundException">Exceção retornada caso o arquivo não exista no Storage</exception>
        public bool Delete(String objectPath)
        {
            objectPath = ResolvePath(objectPath);

            HttpWebRequest request = GetRequest(objectPath);

            request.Method = "DELETE";

            HttpWebResponse response = EfetuarRequisicao(request);
            try
            {
                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                        throw new FileNotFoundException(String.Format("O arquivo {0} não existe", request.RequestUri));
                    else
                        throw new Exception("Não foi possível excluir o objeto " + request.RequestUri);
                }

                return true;
            }
            finally { response.Close(); }
        }

        /// <summary>
        /// Verifica a existência um objeto no Object Storage
        /// </summary>
        /// <param name="objectPath">Caminho do objeto a ser verificado</param>
        /// <returns>Verdadeiro em caso de sucesso</returns>
        /// <exception cref="Exception">Exceção retornada caso o arquivo não possa ser carregado</exception>
        public bool Exists(String objectPath)
        {
            if (log.IsDebugEnabled)
                log.Debug($"ObjectStorageManager: Checking if '{objectPath}'");

            objectPath = ResolvePath(objectPath);
            if (log.IsDebugEnabled)
                log.Debug($"ObjectStorageManager: Path resolved to '{objectPath}'");

            HttpWebRequest request = GetRequest(objectPath);

            request.Method = "HEAD";

            HttpWebResponse response = EfetuarRequisicao(request);
            try
            {
                if (response.StatusCode == HttpStatusCode.OK)
                    return true;
                else if (response.StatusCode == HttpStatusCode.NotFound)
                    return false;
                else
                    throw new Exception(String.Format("Não foi possível verificar o objeto {0}. A requisição retornou: {1} {2}", request.RequestUri, response.StatusCode, response.StatusDescription));
            }
            finally { response.Close(); }

        }

        /// <summary>
        /// Verifica a existência um objeto no Object Storage
        /// </summary>
        /// <param name="objectPath">Caminho do objeto a ser verificado</param>
        /// <returns>Verdadeiro em caso de sucesso</returns>
        /// <exception cref="Exception">Exceção retornada caso o arquivo não possa ser carregado</exception>
        public async Task<bool> ExistsAsync(String objectPath)
        {
            if (log.IsDebugEnabled)
                log.Debug($"ObjectStorageManager: Checking if '{objectPath}'");

            objectPath = ResolvePath(objectPath);
            if (log.IsDebugEnabled)
                log.Debug($"ObjectStorageManager: Path resolved to '{objectPath}'");

            HttpWebRequest request = GetRequest(objectPath);

            request.Method = "HEAD";

            HttpWebResponse response = await EfetuarRequisicaoAsync(request);
            try
            {
                if (response.StatusCode == HttpStatusCode.OK)
                    return true;
                else if (response.StatusCode == HttpStatusCode.NotFound)
                    return false;
                else
                    throw new Exception(String.Format("Não foi possível verificar o objeto {0}. A requisição retornou: {1} {2}", request.RequestUri, response.StatusCode, response.StatusDescription));
            }
            finally { response.Close(); }

        }

        /// <summary>
        /// Verifica a existência um objeto no Object Storage
        /// </summary>
        /// <param name="objectPath">Caminho do objeto a ser verificado</param>
        /// <returns>Verdadeiro em caso de sucesso</returns>
        /// <exception cref="Exception">Exceção retornada caso o arquivo não possa ser carregado</exception>
        public byte[] GetBytes(String objectPath)
        {
            objectPath = ResolvePath(objectPath);
            HttpWebRequest request = GetRequest(objectPath);

            request.Method = "GET";

            HttpWebResponse response = EfetuarRequisicao(request);
            try
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream dataStream = response.GetResponseStream();
                    try
                    {
                        byte[] buffer = new byte[16 * 1024];
                        using (MemoryStream ms = new MemoryStream())
                        {
                            int read;
                            while ((read = dataStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                ms.Write(buffer, 0, read);
                            }
                            return ms.ToArray();
                        }
                    }
                    finally { dataStream.Close(); dataStream.Dispose(); }
                }
                else
                    throw new Exception(String.Format("Não foi possível verificar o objeto {0}. A requisição retornou: {1} {2}", request.RequestUri, response.StatusCode, response.StatusDescription));

            }
            finally
            {
                response.Close();
            }

        }
        /// <summary>
        /// Copia um objeto no Object Storage
        /// </summary>
        /// <param name="objectOriginPath">Caminho do arquivo origem</param>
        /// <param name="objectDestinationPath">Caminho do arquivo Destino</param>
        /// <returns>Verdadeiro em caso de sucesso</returns>
        /// <exception cref="Exception">Exceção retornada caso o arquivo não possa ser copiado</exception>
        public bool Copy(String objectOriginPath, String objectDestinationPath)
        {
            objectOriginPath = ResolvePath(objectOriginPath);
            objectDestinationPath = ResolvePath(objectDestinationPath, false);

            HttpWebRequest request = GetRequest(objectOriginPath);

            request.Headers.Add("Destination", objectDestinationPath);
            request.Method = "COPY";

            HttpWebResponse response = EfetuarRequisicao(request);
            try
            {
                if (response.StatusCode != HttpStatusCode.Created)
                    throw new Exception("Não foi possível copiar o object " + request.RequestUri);

                return true;
            }
            finally { response.Close(); }
        }

        /// <summary>
        /// Move um objeto no Object Storage
        /// </summary>
        /// <param name="objectOriginPath">Caminho do arquivo origem</param>
        /// <param name="objectDestinationPath">Caminho do arquivo Destino</param>
        /// <returns>Verdadeiro em caso de sucesso</returns>
        public bool Move(String objectOriginPath, String objectDestinationPath)
        {
            return Copy(objectOriginPath, objectDestinationPath) && Delete(objectOriginPath);
        }

        /// <summary>
        /// Retorna a URL completa do objeto no object storage para download direto
        /// </summary>
        /// <param name="objectPath">Caminho do objeto a ser verificado</param>
        /// <returns>Endereço completo do objeto</returns>
        /// <exception cref="FileNotFoundException">Exceção retornada caso o arquivo não exista no Storage</exception>
        public String GetUrl(String objectPath)
        {
            if (Exists(objectPath))
                return ResolvePath(objectPath);
            else
                throw new FileNotFoundException(String.Format("Arquivo '{0}' não encontrado", Helper.CombineUrl(_containerUrl, _folder, objectPath)));
        }

        /// <summary>
        /// Implementa o tratamento ao tratamento da url incluindo as pastas a serem criadas com os grupos da Regex
        /// </summary>
        /// <param name="objectPath">Caminho do arquivo</param>
        /// <returns></returns>
        public String ResolvePath(String objectPath, bool includeURL = true)
        {
            var split = objectPath.Split('?');
            var url = string.Empty;

            foreach (var item in split[0].Split('/'))
            {
                url += "/" + Uri.EscapeDataString(item);
            }
            url = Regex.Replace(url, "(^/)|(/$)", string.Empty);
            objectPath = url + (split.Count() > 1 ? split[1] : string.Empty);

            if (_regexToPath == null)
            {
                if (includeURL)
                    return Helper.CombineUrl(_containerUrl, _folder, objectPath);
                else
                    return Helper.CombineUrl(_container, _folder, objectPath);
            }

            String folderPath;
            if (includeURL)
                return Helper.CombineUrl(_containerUrl, _folder, Helper.ResolvePath(objectPath, '/', _regexToPath, out folderPath));
            else
                return Helper.CombineUrl(_container, _folder, Helper.ResolvePath(objectPath, '/', _regexToPath, out folderPath));
        }

        /// <summary>
        /// Gets the number of files
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            HttpWebRequest request = GetRequest(_containerUrl + $"?prefix={_folder}");
            request.Method = "GET";
            HttpWebResponse response = EfetuarRequisicao(request);
            try
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (response.Headers.AllKeys.Any(h => h == "X-Container-Object-Count"))
                        return Convert.ToInt32(response.Headers["X-Container-Object-Count"]);
                }
                else
                    throw new Exception(String.Format("Não foi possível verificar o objeto {0}. A requisição retornou: {1} {2}", request.RequestUri, response.StatusCode, response.StatusDescription));

            }
            finally
            {
                response.Close();
            }

            return -1;
        }

        /// <summary>
        /// Gets the file name list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> List()
        {
            var lastCount = 1;
            var result = new List<string>();
            var lastFile = "";

            while (lastCount > 0)
            {
                var strResponse = string.Empty;

                HttpWebRequest request = GetRequest(_containerUrl + $"?marker={lastFile}&prefix={_folder}");
                request.Method = "GET";
                HttpWebResponse response = EfetuarRequisicao(request);
                try
                {
                    if (response.StatusCode == HttpStatusCode.NoContent)
                        break;

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Stream dataStream = response.GetResponseStream();
                        try
                        {
                            using (StreamReader sr = new StreamReader(dataStream))
                            {
                                strResponse = sr.ReadToEnd();
                            }
                        }
                        finally { dataStream.Close(); dataStream.Dispose(); }
                    }
                    else
                        throw new Exception(String.Format("Não foi possível verificar o objeto {0}. A requisição retornou: {1} {2}", request.RequestUri, response.StatusCode, response.StatusDescription));

                }
                finally
                {
                    response.Close();
                }

                lastCount = 0;
                using (var stringReader = new StringReader(strResponse))
                {
                    var filePath = stringReader.ReadLine();
                    while (filePath != null)
                    {
                        lastFile = filePath;
                        lastCount++;

                        // Ignoring folders
                        if (Regex.IsMatch(filePath, ".*\\.[a-z]{3,4}$"))
                        {
                            // Removing folder from path
                            filePath = string.IsNullOrEmpty(_folder) ? filePath : filePath.Replace(_folder + "/", string.Empty);
                            yield return filePath;
                        }

                        filePath = stringReader.ReadLine();
                    }
                }
            }
        }

        #region Private

        /// <summary>
        /// Recupera o token de autenticação para o Object Storage da Softlayer
        /// </summary>
        /// <returns>Token a ser utilizado na requisição</returns>
        private String GetAuthToken()
        {
            String cacheKey = String.Format("SoftlayerToken:s:{0};u:{1};k:{2};", this._url, this._user, this._apiKey);

            if (MemoryCache.Default[cacheKey] != null)
                return MemoryCache.Default.Get(cacheKey).ToString();

            string token;
            int expires;
            CacheItemPolicy policy = new CacheItemPolicy();

            if (!Authenticate(out token, out expires))
                throw new AuthenticationException("Não foi possível autenticar no storage " + _url);

            policy.AbsoluteExpiration = DateTime.Now.AddSeconds(expires);
            MemoryCache.Default.Add(cacheKey, token, policy);

            return token;
        }

        /// <summary>
        /// Realiza a autenticação no servidor.
        /// </summary>
        /// <returns>Instancia da classe AuthToken com as informações da autenticacao</returns>
        private bool Authenticate(out string token, out int expires)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Helper.CombineUrl(_url, "auth/v1.0"));

            request.Headers.Add("X-Auth-User", _user);
            request.Headers.Add("X-Auth-Key", _apiKey);

            HttpWebResponse response = EfetuarRequisicao(request);
            try
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new AuthenticationException("Não foi possível autenticar no storage " + _url);

                token = response.Headers.Get("X-Auth-Token");
                expires = Convert.ToInt32(response.Headers.Get("X-Auth-Token-Expires"));
                return true;
            }
            finally { response.Close(); }
        }

        /// <summary>
        /// Efetua um request persistente, repetindo a requisição em caso de falha por 3 vezes.
        /// </summary>
        /// <param name="request">Request a ser efetuado</param>
        /// <returns>HttpWebResponse da requisição</returns>
        private static HttpWebResponse EfetuarRequisicao(HttpWebRequest request)
        {
            int retries = 3;
            HttpWebResponse response;

            while (true)
            {
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch (WebException we)
                {
                    HttpWebResponse errorResponse = we.Response as HttpWebResponse;
                    if (errorResponse != null && errorResponse.StatusCode == HttpStatusCode.NotFound)
                    {
                        return errorResponse;
                    }
                    else { throw we; }
                }
                catch (Exception ex)
                {
                    if (--retries == 0)
                    {
                        throw (ex);
                    }
                    else
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                }

                //Verificando se o código de resposta é de sucesso
                if (((int)response.StatusCode >= 200 && (int)response.StatusCode <= 299) || response.StatusCode == HttpStatusCode.NotFound)
                    break;

                if (--retries == 0)
                    throw new HttpRequestException(String.Format("Request returned {0} ({1})", (int)response.StatusCode, response.StatusCode.ToString()));
            }

            return response;
        }

        /// <summary>
        /// Efetua um request persistente, repetindo a requisição em caso de falha por 3 vezes.
        /// </summary>
        /// <param name="request">Request a ser efetuado</param>
        /// <returns>HttpWebResponse da requisição</returns>
        private async static Task<HttpWebResponse> EfetuarRequisicaoAsync(HttpWebRequest request)
        {
            int retries = 3;
            HttpWebResponse response;

            while (true)
            {
                try
                {
                    response = (HttpWebResponse) await request.GetResponseAsync();
                }
                catch (WebException we)
                {
                    HttpWebResponse errorResponse = we.Response as HttpWebResponse;
                    if (errorResponse != null && errorResponse.StatusCode == HttpStatusCode.NotFound)
                    {
                        return errorResponse;
                    }
                    else { throw we; }
                }
                catch (Exception ex)
                {
                    if (--retries == 0)
                    {
                        throw (ex);
                    }
                    else
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                }

                //Verificando se o código de resposta é de sucesso
                if (((int)response.StatusCode >= 200 && (int)response.StatusCode <= 299) || response.StatusCode == HttpStatusCode.NotFound)
                    break;

                if (--retries == 0)
                    throw new HttpRequestException(String.Format("Request returned {0} ({1})", (int)response.StatusCode, response.StatusCode.ToString()));
            }

            return response;
        }


        private HttpWebRequest GetRequest(String Url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            var authToken = GetAuthToken();
            if (log.IsDebugEnabled)
                log.Debug($"ObjectStorageManager: Authorization token 'X-Auth-Token' = '{authToken}'");
            request.Headers.Add("X-Auth-Token", authToken);

            return request;
        }


        #endregion Private
    }
}
