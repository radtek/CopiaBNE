using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace BNE.StorageManager.Config.AmazonS3
{
    [ConfigurationCollection(typeof(BucketConfiguration))]
    public class AmazonS3BucketsConfigurationCollection : ConfigurationElementCollection
    {
        internal const string PropertyName = "Bucket";

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
            return new BucketConfiguration();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((BucketConfiguration)(element)).Name;
        }

        public BucketConfiguration this[string name]
        {
            get
            {
                return (BucketConfiguration)BaseGet(name);
            }
        }

        public BucketConfiguration this[int index]
        {
            get
            {
                return (BucketConfiguration)BaseGet(index);
            }
        }
    }
}
