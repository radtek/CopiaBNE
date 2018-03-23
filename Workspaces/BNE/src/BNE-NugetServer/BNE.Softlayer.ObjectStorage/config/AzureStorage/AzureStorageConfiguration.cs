using System.Configuration;

namespace BNE.StorageManager.Config.AzureStorage
{
    public class AzureStorageConfiguration : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get
            {
                return this["name"].ToString();
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("container", IsKey = true, IsRequired = true)]
        public string Container
        {
            get
            {
                return this["container"].ToString();
            }
            set
            {
                this["container"] = value;
            }
        }

        [ConfigurationProperty("connectionString", IsRequired = true)]
        public string ConnectionString
        {
            get
            {
                return this["connectionString"].ToString();
            }
            set
            {
                this["connectionString"] = value;
            }
        }
    }
}
