using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;

namespace BNE.Cdn
{
    public class CdnConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("img")]
        public CdnItemConfiguration Img
        {
            get
            {
                return (CdnItemConfiguration)this["img"];
            }
            set
            {
                this["img"] = value;
            }
        }

        [ConfigurationProperty("css")]
        public CdnItemConfiguration Css
        {
            get
            {
                return (CdnItemConfiguration)this["css"];
            }
            set
            {
                this["css"] = value;
            }
        }

        [ConfigurationProperty("js")]
        public CdnItemConfiguration Js
        {
            get
            {
                return (CdnItemConfiguration)this["js"];
            }
            set
            {
                this["js"] = value;
            }
        }
    }

    public class CdnItemConfiguration : ConfigurationElement
    {
        [ConfigurationProperty("enable", IsKey = true, IsRequired = true)]
        public Boolean Enable
        {
            get
            {
                return Convert.ToBoolean(this["enable"]);
            }
            set
            {
                this["enable"] = value;
            }
        }

        [ConfigurationProperty("path", IsRequired = true)]
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
    }
}
