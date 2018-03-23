using BNE.Cielo.Enums;

namespace BNE.Cielo.Configuration
{
    public class CustomCieloConfiguration : IConfiguration
    {
        public string CustomerKey { get; set; }
        public string CustomerId { get; set; }
        public string ReturnUrl { get; set; }
        public Language Language { get; set; }
        public string CurrencyId { get; set; }
    }
}