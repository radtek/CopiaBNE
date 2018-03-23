using System;
using System.Configuration;

namespace BNE.Log
{
    /// <summary>
    /// Sessão de configuração que indica o tamanho do buffer e timeout dele.
    /// Ex: 
    ///  <BufferLogConfig Tamanho="100" Timeout="00:00:30"></BufferLogConfig>
    /// </summary>
    public class Configuration : ConfigurationSection
    {
        /// <summary>
        /// Tamanho do buffer
        /// </summary>
        [ConfigurationProperty("Tamanho", DefaultValue = 10)]
        public int Tamanho
        {
            get
            {
                if (this["Tamanho"] == null)
                    return 10;
                return (int)this["Tamanho"];
            }
            set { this["Tamanho"] = value; }
        }

        /// <summary>
        /// Tempo de expiração do buffer. Padrão 1 minuto
        /// </summary>
        [ConfigurationProperty("Timeout", DefaultValue = "00:01:00")]
        public TimeSpan Timeout
        {
            get
            {
                if (this["Timeout"] == null)
                    return new TimeSpan(0, 0, 60);
                return (TimeSpan)this["Timeout"];
            }
            set { this["Timeout"] = value; }
        }

        /// <summary>
        /// Tamanho do buffer
        /// </summary>
        [ConfigurationProperty("PersistenceLibrary")]
        public string TypeLibraryDbWrites
        {
            get { return this["PersistenceLibrary"] as string; }
            set { this["PersistenceLibrary"] = value; }
        }
    }
}
