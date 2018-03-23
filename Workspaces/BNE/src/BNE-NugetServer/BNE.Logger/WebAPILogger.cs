using System;
using System.Configuration;
using BNE.Logger.Interface;
using Newtonsoft.Json;
using Exception = System.Exception;

namespace BNE.Logger
{
    public class WebAPILogger : ILogger
    {
        private static WebAPILoggerConfigurationSection _config;
        public static WebAPILoggerConfigurationSection Config
        {
            get
            {
                if (_config == null)
                    _config = ConfigurationManager.GetSection("WebAPILoggerConfig") as WebAPILoggerConfigurationSection;

                if (_config == null)
                {
#if DEBUG
                    _config = new WebAPILoggerConfigurationSection
                    {
                        ApplicationName = "Aplicação não configurada " + System.Reflection.Assembly.GetExecutingAssembly().CodeBase,
                        EndpointURL = "http://teste.api.erro.bne.com.br/api"
                    };
#else
                     _config = new WebAPILoggerConfigurationSection
                    {
                        ApplicationName = "Aplicação não configurada " + System.Reflection.Assembly.GetExecutingAssembly().CodeBase,
                        EndpointURL = "http://api.erro.bne.com.br/api"
                    };
#endif
                }

                return _config;
            }
        }

        public WebAPILogger()
        {
            Name = "WebAPI Logger";
        }
        public string Name { get; set; }
        public void Error(Exception ex)
        {
            Error(ex, string.Empty, string.Empty);
        }
        public void Error(Exception ex, string customMessage)
        {
            Error(ex, customMessage, string.Empty);
        }
        public void Error(Exception ex, string customMessage, string payload)
        {
            var model = new
            {
                Aplicacao = Config.ApplicationName,
                Exception = ex,
                CustomMessage = customMessage,
                Payload = payload
            };

            SendToAPI(model, "/error");
        }

        public void Information(string message)
        {
            var model = new
            {
                Aplicacao = Config.ApplicationName,
                Message = message,
            };

            SendToAPI(model, "/information");
        }

        public void Information(string message, string payload)
        {
            var model = new
            {
                Aplicacao = Config.ApplicationName,
                Message = message,
                Payload = payload
            };

            SendToAPI(model, "/information");
        }

        public void Warning(string message)
        {
            var model = new
            {
                Aplicacao = Config.ApplicationName,
                Message = message,
            };

            SendToAPI(model, "/warning");
        }

        public void Warning(string message, string payload)
        {
            var model = new
            {
                Aplicacao = Config.ApplicationName,
                Message = message,
                Payload = payload
            };

            SendToAPI(model, "/warning");
        }

        private void SendToAPI(dynamic model, string path)
        {
            try
            {
                var service = new HttpService();

                var payloadAPI = JsonConvert.SerializeObject(model, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                service.Post(new Uri(Config.EndpointURL), path, payloadAPI);
            }
            catch { }
        }

    }

    public class WebAPILoggerConfigurationSection : ConfigurationSection
    {

        [ConfigurationProperty("ApplicationName", IsRequired = true)]
        public string ApplicationName
        {
            set { this["ApplicationName"] = value; }
            get { return this["ApplicationName"].ToString(); }
        }

        [ConfigurationProperty("EndpointURL", IsRequired = true)]
        public string EndpointURL
        {
            set { this["EndpointURL"] = value; }
            get { return this["EndpointURL"].ToString(); }
        }

    }

}
