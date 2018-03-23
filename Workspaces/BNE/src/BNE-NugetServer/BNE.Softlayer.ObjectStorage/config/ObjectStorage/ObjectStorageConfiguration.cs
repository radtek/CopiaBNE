using System;
using System.Configuration;

namespace BNE.StorageManager.Config.ObjectStorage
{
    public class ObjectStorageConfiguration : ConfigurationElement
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

        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get
            {
                return this["url"].ToString();
            }
            set
            {
                this["url"] = value;
            }
        }

        [ConfigurationProperty("account", IsRequired = true)]
        public string Account
        {
            get
            {
                return this["account"].ToString();
            }
            set
            {
                this["account"] = value;
            }
        }

        [ConfigurationProperty("container", IsRequired = true)]
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

        [ConfigurationProperty("user", IsRequired = true)]
        public string User
        {
            get
            {
                return this["user"].ToString();
            }
            set
            {
                this["user"] = value;
            }
        }

        [ConfigurationProperty("apiKey", IsRequired = true)]
        public string ApiKey
        {
            get
            {
                return this["apiKey"].ToString();
            }
            set
            {
                this["apiKey"] = value;
            }
        }
    }
}
