using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace BNE.StorageManager.Config.AzureStorage
{
    [ConfigurationCollection(typeof(AzureStorageConfiguration))]
    public class AzureStorageConfigurationCollection : ConfigurationElementCollection
    {
        internal const string PropertyName = "AzureStorage";

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
            return new AzureStorageConfiguration();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AzureStorageConfiguration)(element)).Name;
        }

        public AzureStorageConfiguration this[string name]
        {
            get
            {
                return (AzureStorageConfiguration)BaseGet(name);
            }
        }

        public AzureStorageConfiguration this[int index]
        {
            get
            {
                return (AzureStorageConfiguration)BaseGet(index);
            }
        }
    }
}
