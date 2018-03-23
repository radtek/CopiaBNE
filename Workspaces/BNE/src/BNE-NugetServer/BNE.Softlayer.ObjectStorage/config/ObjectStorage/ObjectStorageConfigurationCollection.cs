using System;
using System.Configuration;

namespace BNE.StorageManager.Config.ObjectStorage
{
    [ConfigurationCollection(typeof(ObjectStorageConfiguration))]
    public class ObjectStorageConfigurationCollection : ConfigurationElementCollection
    {
        internal const string PropertyName = "ObjectStorage";

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
            return new ObjectStorageConfiguration();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ObjectStorageConfiguration)(element)).Name;
        }

        public ObjectStorageConfiguration this[string name]
        {
            get
            {
                return (ObjectStorageConfiguration)BaseGet(name);
            }
        }

        public ObjectStorageConfiguration this[int index]
        {
            get
            {
                return (ObjectStorageConfiguration)BaseGet(index);
            }
        }
    }
}
