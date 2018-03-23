using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Runtime.Caching;
using System.Web.UI;

namespace BNE.Web.Code
{
    enum AdapterType
    {
        Memcached,
        Disk,
        HiddenField //Default
    }

    public class GlobalPageAdapter : System.Web.UI.Adapters.PageAdapter
    {
        private static AdapterType? _adapterType;
        private static AdapterType AdapterType
        {
            get
            {
                if (!_adapterType.HasValue)
                {
                    string customPageState = ConfigurationManager.AppSettings["PageStatePersister"];

                    AdapterType adapterType;
                    if (!Enum.TryParse(customPageState, out adapterType))
                        adapterType = AdapterType.HiddenField;

                    _adapterType = adapterType;
                }

                return _adapterType.Value;
            }
        }

        public override PageStatePersister GetStatePersister()
        {
            switch (AdapterType)
            {
                case AdapterType.Memcached:
                    //Check se o processo de Memcached está executando
                    if (MemoryCachedPersister.IsMemcachedRunning())
                        return new MemoryCachedPersister(Page);

                    EL.GerenciadorException.GravarExcecao(new Exception("Processo do Memcached não encontrado. Utilizando DiskPersister"));
                    return new DiskPersister(Page);
                case AdapterType.Disk:
                    return new DiskPersister(Page);
                default:
                    return new HiddenFieldPageStatePersister(Page);
            }
        }
    }

    /// <summary>
    /// Esta classe grava o viewstate em uma pasta VIEWSTATE na raiz da aplicação deixando de enviar 
    /// p/ o cliente.
    /// </summary>
    public class DiskPersister : CustomPageStatePersister
    {
        private const string FileExtension = "viewstate";
        private static int? _cacheTimeSpan;

        public static int CacheTimeSpan
        {
            get
            {
                if (!_cacheTimeSpan.HasValue)
                {
                    string timeSpanConfig = ConfigurationManager.AppSettings["MemcachedTimeSpan"];

                    int timeSpan;
                    if (int.TryParse(timeSpanConfig, out timeSpan))
                        timeSpan = 30;

                    _cacheTimeSpan = timeSpan;
                }

                return _cacheTimeSpan.Value;
            }
        }

        public DiskPersister(Page page) : base(page) { }

        #region Load
        public override void Load()
        {
            string key = GetKey();
            ObjectCache cache = MemoryCache.Default;
            var cacheItem = cache.Get("ViewState:" + key);
            if (cacheItem != null)
            {
                var cacheStatePair = (Pair)cacheItem;
                ViewState = cacheStatePair.First;
                ControlState = cacheStatePair.Second;
                return;
            }

            string strFolder = GetFolder();

            string sFile = string.Format("{0}{1}.{2}", strFolder, key, FileExtension);

            if (!File.Exists(sFile))
                return;

            string bytes;
            using (var sr = new StreamReader(sFile))
            {
                bytes = sr.ReadToEnd(); //Ler do arquivo 
            }

            var formatter = new LosFormatter();
            var statePair = (Pair)formatter.Deserialize(bytes);

            ViewState = statePair.First;
            ControlState = statePair.Second;

            cache.Set("ViewState:" + key, statePair, new CacheItemPolicy() { AbsoluteExpiration = DateTime.Now.AddMinutes(CacheTimeSpan) });
        }
        #endregion

        #region Save
        public override void Save()
        {
            if ((ViewState != null || ControlState != null))
            {
                string key = GetKey();
                string strFolder = GetFolder();
                if (String.IsNullOrEmpty(key))
                    key = Guid.NewGuid().ToString().Replace("-", "");

                var statePair = new Pair(ViewState, ControlState);

                MemoryCache.Default.Set("ViewState:" + key, statePair, new CacheItemPolicy() { AbsoluteExpiration = DateTime.Now.AddMinutes(CacheTimeSpan) });

                string strNomeArquivo = string.Format("{0}.{1}", key, FileExtension);

                // Salvando arquivo assincronamente para não travar a thread
                // Tirando da thread pois dá erro de concorrencia se outra tentar ler ou escrever no vw
                //Thread thread = new Thread(() => WriteText(string.Format("{0}{1}", strFolder, strNomeArquivo), statePair));
                //thread.Start();
                try
                {
                    WriteText(string.Format("{0}{1}", strFolder, strNomeArquivo), statePair);
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex, "VIEWSTATE");
                }

                Page.ClientScript.RegisterHiddenField("__VSTATE", key);
            }
        }
        #endregion

        private void WriteText(string filePath, Pair statePair)
        {
            var formatter = new LosFormatter();
            StringWriter sw = null;

            try
            {
                sw = new StringWriter();
                formatter.Serialize(sw, statePair);
                using (var fWriter = new StreamWriter(filePath))
                {
                    fWriter.Write(sw.ToString());
                }
            }
            finally
            {
                if (sw != null)
                    sw.Dispose();
            }
        }

        #region GetFolder
        private string GetFolder()
        {
            string strFolder = Page.Server.MapPath(@"\VIEWSTATE\");

            if (!Directory.Exists(strFolder))
                Directory.CreateDirectory(strFolder);

            return strFolder;
        }
        #endregion

    }

    /// <summary>
    /// Esta classe grava o viewstate utilizando Memorycached
    /// </summary>
    public class MemoryCachedPersister : CustomPageStatePersister
    {
        private static int? _memcachedTimeSpan;
        private static MemcachedClientConfiguration _memcachedConfig;

        public static int MemcachedTimeSpan
        {
            get
            {
                if (!_memcachedTimeSpan.HasValue)
                {
                    string timeSpanConfig = ConfigurationManager.AppSettings["MemcachedTimeSpan"];

                    int timeSpan;
                    if (int.TryParse(timeSpanConfig, out timeSpan))
                        timeSpan = 30;

                    _memcachedTimeSpan = timeSpan;
                }

                return _memcachedTimeSpan.Value;
            }
        }

        public static MemcachedClientConfiguration MemcachedConfig
        {
            get
            {
                if (_memcachedConfig == null)
                {
                    _memcachedConfig = new MemcachedClientConfiguration();
                    _memcachedConfig.Servers.Add(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11211));
                    _memcachedConfig.Protocol = MemcachedProtocol.Binary;
                }

                return _memcachedConfig;
            }
        }

        public MemoryCachedPersister(Page page) : base(page) { }

        public static bool IsMemcachedRunning()
        {
            //Verifica se o processo de Memcached esta funcionando como o esperado
            using (MemcachedClient client = new MemcachedClient(MemcachedConfig))
            {
                return client.Store(StoreMode.Set, "testeRunning", "teste", new TimeSpan(0, 1, 0)) && client.Get<string>("testeRunning") == "teste";
            }
        }

        #region Load
        public override void Load()
        {
            string key = GetKey();

            Pair statePair;
            using (MemcachedClient client = new MemcachedClient(MemcachedConfig))
            {
                statePair = client.Get<Pair>(key);
            }

            if (statePair == null)
                return;

            ViewState = statePair.First;
            ControlState = statePair.Second;
        }
        #endregion

        #region Save
        public override void Save()
        {
            if ((ViewState != null || ControlState != null))
            {
                string key = GetKey();
                if (String.IsNullOrEmpty(key))
                    key = Guid.NewGuid().ToString().Replace("-", "");

                var statePair = new Pair(ViewState, ControlState);

                using (MemcachedClient client = new MemcachedClient(MemcachedConfig))
                {
                    if (!client.Store(StoreMode.Set, key, statePair, new TimeSpan(0, MemcachedTimeSpan, 0)))
                        EL.GerenciadorException.GravarExcecao(new Exception("Erro ao salvar VIEWSTATE. Verifique se o processo do MemCached está rodando."));
                }

                Page.ClientScript.RegisterHiddenField("__VSTATE", key);
            }
        }
        #endregion

    }

    public abstract class CustomPageStatePersister : PageStatePersister
    {

        #region GetKey
        public string GetKey()
        {
            string strViewState = Page.Request.Form["__VSTATE"];

            if (!string.IsNullOrEmpty(strViewState) && strViewState.Contains(","))
                return strViewState.Substring(0, strViewState.IndexOf(",", StringComparison.Ordinal));

            return strViewState;
        }
        #endregion

        protected CustomPageStatePersister(Page page)
            : base(page)
        {
        }
    }
}
