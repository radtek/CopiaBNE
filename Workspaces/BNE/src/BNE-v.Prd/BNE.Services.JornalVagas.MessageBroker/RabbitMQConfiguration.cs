using System;
using System.Configuration;

namespace BNE.Services.JornalVagas.MessageBroker
{
    public class RabbitMQConfiguration : ConfigurationSection
    {
        public static RabbitMQConfiguration Default => GetConfig("RabbitMQConfiguration");

        [ConfigurationProperty("host", IsRequired = true)]
        public string Host => (string) this["host"];

        [ConfigurationProperty("port", IsRequired = true)]
        public int Port => (int) this["port"];

        [ConfigurationProperty("username", IsRequired = true)]
        public string Username => (string) this["username"];

        [ConfigurationProperty("password", IsRequired = true)]
        public string Password => (string) this["password"];

        [ConfigurationProperty("virtualHost", IsRequired = true)]
        public string VirtualHost => (string) this["virtualHost"];

        [ConfigurationProperty("AutomaticRecoveryEnabled", IsRequired = true)]
        public bool AutomaticRecoveryEnabled => (bool)this["AutomaticRecoveryEnabled"];

        [ConfigurationProperty("TopologyRecoveryEnabled", IsRequired = true)]
        public bool TopologyRecoveryEnabled => (bool)this["TopologyRecoveryEnabled"];

        [ConfigurationProperty("RequestedHeartbeat", IsRequired = true)]
        public ushort RequestedHeartbeat => (ushort)this["RequestedHeartbeat"];

        [ConfigurationProperty("NetworkRecoveryInterval", IsRequired = false)]
        public TimeSpan NetworkRecoveryInterval => (TimeSpan)this["NetworkRecoveryInterval"];

        [ConfigurationProperty("ContinuationTimeout", IsRequired = false)]
        public TimeSpan ContinuationTimeout => (TimeSpan)this["ContinuationTimeout"];
        
        public static RabbitMQConfiguration GetConfig()
        {
            var section = (RabbitMQConfiguration) ConfigurationManager.GetSection("RabbitMQConfiguration");

            if (section == null)
            {
                throw new ConfigurationErrorsException("RabbitMQConfiguration not found.");
            }

            return GetConfig("RabbitMQConfiguration");
        }

        public static RabbitMQConfiguration GetConfig(string sectionName)
        {
            var section = (RabbitMQConfiguration) ConfigurationManager.GetSection(sectionName) ?? GetConfig();

            if (section == null)
            {
                throw new ConfigurationErrorsException("RabbitMQConfiguration not found.");
            }

            return section;
        }
    }
}