using System;
using System.Configuration;
using BNE.Cielo.Enums;

namespace BNE.Cielo.Configuration
{
    public class DefaultCieloConfiguration : IConfiguration
    {
        public string CustomerKey
        {
            get { return ConfigurationManager.AppSettings["cielo.customer.key"]; }
        }

        public string CustomerId
        {
            get { return ConfigurationManager.AppSettings["cielo.customer.id"]; }
        }

        public string ReturnUrl
        {
            get { return ConfigurationManager.AppSettings["cielo.return.url"]; }
        }

        public Language Language
        {
            get { return (Language) Convert.ToInt32(ConfigurationManager.AppSettings["cielo.language.id"]); }
        }

        public string CurrencyId
        {
            get { return ConfigurationManager.AppSettings["cielo.currency.id"]; }
        }
    }
}