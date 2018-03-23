using System;
using System.Configuration;

namespace BNE.StorageManager.Config.AmazonS3
{
    public class BucketConfiguration : ConfigurationSection
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

        [ConfigurationProperty("bucket", IsRequired = true)]
        public string Bucket
        {
            get
            {
                return this["bucket"].ToString();
            }
            set
            {
                this["bucket"] = value;
            }
        }

        [ConfigurationProperty("accessKeyID", IsRequired = true)]
        public string AccessKeyID
        {
            get
            {
                return this["accessKeyID"].ToString();
            }
            set
            {
                this["accessKeyID"] = value;
            }
        }

        [ConfigurationProperty("secretAccessKey", IsRequired = true)]
        public string SecretAccessKey
        {
            get
            {
                return this["secretAccessKey"].ToString();
            }
            set
            {
                this["secretAccessKey"] = value;
            }
        }

        [ConfigurationProperty("regionEndpoint", IsRequired = true)]
        public string RegionEndpoint
        {
            get
            {
                return this["regionEndpoint"].ToString();
            }
            set
            {
                this["regionEndpoint"] = value;
            }
        }

        
    }
}
