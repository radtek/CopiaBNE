using System;
using System.Configuration;
using BNE.StorageManager.Config.ObjectStorage;
using BNE.StorageManager.Config.AmazonS3;
using BNE.StorageManager.Config.AzureStorage;

namespace BNE.StorageManager.Config
{
    public class StoragesConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("ObjectStorages", IsRequired = false, IsDefaultCollection = true)]
        public ObjectStorageConfigurationCollection Storages 
        {
            get { return (ObjectStorageConfigurationCollection)this["ObjectStorages"]; }
            set { this["ObjectStorages"] = value; }
        }

        [ConfigurationProperty("AzureStorages", IsRequired = false, IsDefaultCollection = true)]
        public AzureStorageConfigurationCollection AzureStorages
        {
            get { return (AzureStorageConfigurationCollection)this["AzureStorages"]; }
            set { this["AzureStorages"] = value; }
        }

        [ConfigurationProperty("AmazonS3Buckets", IsRequired = false, IsDefaultCollection = true)]
        public AmazonS3BucketsConfigurationCollection AmazonS3Buckets
        {
            get { return (AmazonS3BucketsConfigurationCollection)this["AmazonS3Buckets"]; }
            set { this["AmazonS3Buckets"] = value; }
        }

        [ConfigurationProperty("Folders", IsDefaultCollection = true)]
        public FolderConfigurationCollection Folders
        {
            get { return (FolderConfigurationCollection)this["Folders"]; }
            set { this["Folders"] = value; }
        }
    }

    #region FolderConfigurationCollection
    [ConfigurationCollection(typeof(FolderConfiguration))]
    public class FolderConfigurationCollection : ConfigurationElementCollection
    {
        internal const string PropertyName = "Folder";

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMapAlternate;
            }
        }
        protected override string ElementName
        {
            get
            {
                return PropertyName;
            }
        }

        protected override bool IsElementName(string elementName)
        {
            return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase);
        }


        public override bool IsReadOnly()
        {
            return false;
        }


        protected override ConfigurationElement CreateNewElement()
        {
            return new FolderConfiguration();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FolderConfiguration)(element)).Name;
        }

        public FolderConfiguration this[string name]
        {
            get
            {
                return (FolderConfiguration)BaseGet(name);
            }
        }

        public FolderConfiguration this[int index]
        {
            get
            {
                return (FolderConfiguration)BaseGet(index);
            }
        }
    }
    #endregion FolderConfigurationCollection

    #region StorageConfigurationCollection
    [ConfigurationCollection(typeof(StorageConfigurationCollection))]
    public class StorageConfigurationCollection : ConfigurationElementCollection
    {
        internal const string PropertyName = "Storage";

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMapAlternate;
            }
        }
        protected override string ElementName
        {
            get
            {
                return PropertyName;
            }
        }

        protected override bool IsElementName(string elementName)
        {
            return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase);
        }


        public override bool IsReadOnly()
        {
            return false;
        }


        protected override ConfigurationElement CreateNewElement()
        {
            return new StorageConfiguration();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((StorageConfiguration)(element)).Name;
        }

        //public StorageConfiguration this[StorageTypes type]
        //{
        //    get
        //    {
        //        return (StorageConfiguration)BaseGet(type);
        //    }
        //}

        public StorageConfiguration this[int index]
        {
            get
            {
                return (StorageConfiguration)BaseGet(index);
            }
        }
    }
    #endregion StorageConfigurationCollection

    public class FolderConfiguration : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public String Name
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

        [ConfigurationProperty("enableCulture", IsRequired = false)]
        public Boolean EnableCulture
        {
            get
            {
                return Convert.ToBoolean(this["enableCulture"]);
            }
            set
            {
                this["enableCulture"] = value;
            }
        }

        [ConfigurationProperty("Storages", IsDefaultCollection = true)]
        public StorageConfigurationCollection Storages
        {
            get { return (StorageConfigurationCollection)this["Storages"]; }
            set { this["Storages"] = value; }
        }
    }

    public class StorageConfiguration : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public String Name
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

        [ConfigurationProperty("type", IsRequired = true)]
        public StorageTypes Type
        {
            get
            {
                StorageTypes type;
                if (Enum.TryParse<StorageTypes>(this["type"].ToString(), out type))
                    return type;

                return StorageTypes.Local;
            }
            set
            {
                this["type"] = value.ToString();
            }
        }

        [ConfigurationProperty("path", IsRequired = false)]
        public String Path
        {
            get
            {
                return this["path"].ToString();
            }
            set
            {
                this["path"] = value;
            }
        }

        [ConfigurationProperty("publicUrl")]
        public String PublicUrl
        {
            get
            {
                return this["publicUrl"].ToString();
            }
            set
            {
                this["publicUrl"] = value;
            }
        }

        [ConfigurationProperty("storageFolder", IsRequired = false)]
        public String StorageFolder
        {
            get
            {
                return this["storageFolder"].ToString();
            }
            set
            {
                this["storageFolder"] = value;
            }
        }

        [ConfigurationProperty("storageName", IsRequired = false)]
        public String StorageName
        {
            get
            {
                return this["storageName"].ToString();
            }
            set
            {
                this["storageName"] = value;
            }
        }

        [ConfigurationProperty("Enabled", IsRequired = true)]
        public Boolean Enabled
        {
            get
            {
                return Convert.ToBoolean(this["Enabled"].ToString());
            }
            set
            {
                this["Enabled"] = value;
            }
        }

        [ConfigurationProperty("RegexPath", IsRequired = false)]
        public String RegexPath
        {
            get
            {
                return this["RegexPath"].ToString();
            }
            set
            {
                this["RegexPath"] = value;
            }
        }

    }

}
