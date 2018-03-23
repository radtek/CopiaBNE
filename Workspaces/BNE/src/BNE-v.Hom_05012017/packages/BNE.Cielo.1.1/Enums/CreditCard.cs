using System.ComponentModel;

namespace BNE.Cielo.Enums
{
    public enum CreditCard
    {
        [Description("visa")] Visa,
        [Description("mastercard")] MasterCard,
        [Description("diners")] Diners,
        [Description("discover")] Discover,
        [Description("elo")] Elo,
        [Description("amex")] Amex,
        [Description("jcb")] Jcb,
        [Description("aura")] Aura
    }
}